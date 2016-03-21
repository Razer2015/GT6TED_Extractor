using PS3FileSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GT6TED_Extractor
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() == 1)
            {
                // get the file attributes for file or directory
                FileAttributes attr = File.GetAttributes(args[0]);

                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    byte[] tmp = null;

                    if (IsEncrypted(File.ReadAllBytes(Path.Combine(args[0], "REPLAY.0"))))
                        tmp = Decrypt(args[0]);
                    else
                        tmp = File.ReadAllBytes(Path.Combine(args[0], "REPLAY.0"));

                    if (tmp == null)
                    {
                        Console.WriteLine("Decryption Failed!");
                        return;
                    }
                    int pos = ContainsTED(tmp);
                    if (pos < 4)
                    {
                        Console.WriteLine("Couldn't find GT6TED file!");
                        return;
                    }

                    using (MemoryStream ms = new MemoryStream(tmp))
                    {
                        EndianBinReader reader = new EndianBinReader(ms);
                        reader.BaseStream.Position = (pos - 4);
                        int length = reader.ReadInt32();
                        byte[] data = reader.ReadBytes(length);
                        File.WriteAllBytes(Path.Combine(args[0], "extracted.ted"), data);
                    }
                    File.WriteAllBytes(Path.Combine(args[0], "REPLAY.0"), tmp);
                }
                else
                {
                    byte[] tmp = null;

                    if (IsEncrypted(File.ReadAllBytes(args[0])))
                        tmp = Decrypt(Path.GetDirectoryName(args[0]));
                    else
                        tmp = File.ReadAllBytes(args[0]);

                    if (tmp == null)
                    {
                        Console.WriteLine("Decryption Failed!");
                        return;
                    }
                    int pos = ContainsTED(tmp);
                    if (pos < 4)
                    {
                        Console.WriteLine("Couldn't find GT6TED file!");
                        return;
                    }

                    using (MemoryStream ms = new MemoryStream(tmp))
                    {
                        EndianBinReader reader = new EndianBinReader(ms);
                        reader.BaseStream.Position = (pos - 4);
                        int length = reader.ReadInt32();
                        byte[] data = reader.ReadBytes(length);
                        File.WriteAllBytes(Path.Combine(Path.GetDirectoryName(args[0]), "extracted.ted"), data);
                    }
                    File.WriteAllBytes(args[0], tmp);
                }
            }
            else
            {
                WriteInfo();
            }
        }

        private static void WriteInfo()
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            var version = versionInfo.FileVersion;
            var companyName = versionInfo.CompanyName;
            var desc = versionInfo.FileDescription;
            var copyright = versionInfo.LegalCopyright;

            Assembly assembly = Assembly.GetExecutingAssembly();
            var descriptionAttribute = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false).OfType<AssemblyDescriptionAttribute>().FirstOrDefault();

            Console.WriteLine(String.Empty);
            Console.WriteLine("+--------------+-------------------------------------------+");
            Console.WriteLine("| " + companyName + " | " + descriptionAttribute.Description + " " + version + " |");
            Console.WriteLine("+--------------+-------------------------------------------+");

            Console.WriteLine("Usage: GT6TED_Extractor.exe <filepath or replay dir>");
            Console.WriteLine("Examples:");
            Console.WriteLine("Drag and Drop the file/folder over this GT6TED_Extractor.exe");
            Console.WriteLine("GT6TED_Extractor.exe \"C:\\BCJS37016-RPLY001\"");
            Console.WriteLine("GT6TED_Extractor.exe \"C:\\BCJS37016-RPLY001\\REPLAY.0\"");
        }

        static bool IsEncrypted(byte[] data)
        {
            var pattern = new byte[] { 0x52, 0x50, 0x4C, 0x59 };

            if (data.Locate(pattern).Length > 0)
                return (false);
            return (true);
        }
        static int ContainsTED(byte[] data)
        {
            var pattern = new byte[] { 0x52, 0x50, 0x4C, 0x59 };
            var pattern2 = new byte[] { 0x47, 0x54, 0x36, 0x54, 0x45, 0x44 };

            if (data.Locate(pattern).Length > 0)
            {
                var position = data.Locate(pattern2);
                if (position.Length > 0)
                {
#if DEBUG
                    Console.WriteLine(position[0]); 
#endif
                    return (position[0]);
                }
                return (-1);
            }
            return (-1);
        }

        static byte[] Decrypt(String dirpath)
        {
            //define the securefile id
            byte[] key = new byte[] { 0x77, 0x1D, 0x1C, 0x71, 0xE7, 0x5B, 0x4E, 0x70, 0x80, 0x38, 0x73, 0xF7, 0x40, 0x25, 0x11, 0xA7 };
            //declare ps3 manager using the directory path, and the secure file id
            Ps3SaveManager manager = new Ps3SaveManager(dirpath, key);
            //get file entry using name
            Ps3File file = manager.Files.FirstOrDefault(t => t.PFDEntry.file_name == "REPLAY.0");
            //define byte array that the decrypted data should be allocated
            byte[] filedata = null;
            //check if file is not null
            if (file != null)
                filedata = file.DecryptToBytes();
            return (filedata);
        }
    }

    static class ByteArrayRocks
    {

        static readonly int[] Empty = new int[0];

        public static int[] Locate(this byte[] self, byte[] candidate)
        {
            if (IsEmptyLocate(self, candidate))
                return Empty;

            var list = new List<int>();

            for (int i = 0; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate))
                    continue;

                list.Add(i);
            }

            return list.Count == 0 ? Empty : list.ToArray();
        }

        static bool IsMatch(byte[] array, int position, byte[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;

            return true;
        }

        static bool IsEmptyLocate(byte[] array, byte[] candidate)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length;
        }
    }
}
