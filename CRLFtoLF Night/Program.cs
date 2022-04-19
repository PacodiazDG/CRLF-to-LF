using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRLFtoLF_Night
{
    internal class Program
    {
        static IEnumerable<string> dirString(string dir, bool subDir)
        {
            return !subDir ? Directory.EnumerateFiles(dir, "*.*", SearchOption.TopDirectoryOnly) : Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories);
        }
        static void convertLF(string dir)
        {
            string contents;
            try
            {
                contents = File.ReadAllText(@dir);
                File.WriteAllText(dir, contents.Replace("\r\n", "\n"));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;

            }
        }
        static async void go(string patch, bool SubDirectory, string excude)
        {
            string[] ExcudeNames = excude.Split(",");
            FileAttributes attr;
            try
            {
                attr = File.GetAttributes(patch);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    foreach (string file in dirString(patch, SubDirectory))
                    {

                        if (Array.Find(ExcudeNames, element => file.Contains((element))) !=null)
                        {
                            Console.WriteLine("Omitted file: " + file);
                            continue;
                        }
                        Console.WriteLine(file);

                        convertLF(file);
                    }
                else
                {
                    convertLF(patch);
                }

            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            return;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("CRLF to LF");
            if (args.Length <2 )
            {
                Console.WriteLine("missing options (directory or file, SubDirectory) Ej: CRLFtoLF.exe c:/example true FoloderNameExcude1,FoloderNameExcude2,FoloderNameExcude3");
                return;
            }
            bool subDir = false;
            if (args[1] == "true")
            {
                subDir = true;
            }
            string excude = String.Empty;
            if (args.Length == 3)
                excude = args[2];
            go(args[0], subDir, excude);

        }
    }
}

