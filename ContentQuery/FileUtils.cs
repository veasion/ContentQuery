using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;

namespace ContentQuery
{
    class FileUtils
    {

        public static HashSet<string> skipPath = new HashSet<string>();

        static FileUtils()
        {
            skipPath.Add(@"C:\Program Files".ToLower());
            skipPath.Add(@"C:\Program Files (x86)".ToLower());
            skipPath.Add(@"C:\ProgramData".ToLower());
            skipPath.Add(@"C:\Windows".ToLower());
        }

        public static bool hasTextByPackage(FileInfo fileInfo, string text, string uriString)
        {
            try
            {
                using (Package package = Package.Open(fileInfo.FullName))
                {
                    Uri docxUri = new Uri(uriString, UriKind.Relative);
                    PackagePart part = package.GetPart(docxUri);
                    string line;
                    StreamReader sr = new StreamReader(part.GetStream());
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.IndexOf(text) != -1)
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("加载" + fileInfo.FullName + "异常：" + e.Message);
                throw e;
            }
        }

        public static bool hasText(FileInfo fileInfo, string text, Encoding encoding)
        {
            string line;
            StreamReader sr = new StreamReader(fileInfo.FullName, encoding);
            while ((line = sr.ReadLine()) != null)
            {
                if (line.IndexOf(text) != -1)
                {
                    return true;
                }
            }
            return false;
        }

        public static Encoding GetFileEncodeType(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; // BOM
                Encoding reVal = Encoding.Default;
                BinaryReader br = new BinaryReader(fs);
                int length;
                int.TryParse(fs.Length.ToString(), out length);
                if (length == 0 || length < 3)
                {
                    br.Close();
                    return Encoding.UTF8;
                }
                byte[] buffer = br.ReadBytes(length);
                if (buffer[0] == UnicodeBIG[0] && buffer[1] == UnicodeBIG[1] && buffer[2] == UnicodeBIG[2])
                {
                    reVal = Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == Unicode[0] && buffer[1] == Unicode[1] && buffer[2] == Unicode[2])
                {
                    reVal = Encoding.Unicode;
                }
                else if ((buffer[0] == UTF8[0] && buffer[1] == UTF8[1] && buffer[2] == UTF8[2]) || IsUTF8Bytes(buffer))
                {
                    reVal = Encoding.UTF8;
                }
                br.Close();
                return reVal;
            }
        }

        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;
            byte curByte;
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                return false;
            }
            return true;
        }

        public static int checkOffice()
        {
            int officeVersion = -1;

            RegistryKey rk = Registry.LocalMachine;
            RegistryKey akey07 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\12.0\Excel\InstallRoot\");//查询2007
            RegistryKey akey10 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\14.0\Excel\InstallRoot\");//查询2010
            RegistryKey akey13 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\15.0\Excel\InstallRoot\");//查询2013
            RegistryKey akey16 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\16.0\Excel\InstallRoot\");//查询2016

            //检查本机是否安装Office2007
            if (akey07 != null)
            {
                string office07 = akey07.GetValue("Path").ToString();
                if (File.Exists(office07 + "Excel.exe"))
                {
                    officeVersion = 2007;
                }
            }

            //检查本机是否安装Office2010
            if (akey10 != null)
            {
                string office10 = akey10.GetValue("Path").ToString();
                if (File.Exists(office10 + "Excel.exe"))
                {
                    officeVersion = 2010;
                }
            }

            //检查本机是否安装Office2013
            if (akey13 != null)
            {
                string office13 = akey13.GetValue("Path").ToString();
                if (File.Exists(office13 + "Excel.exe"))
                {
                    officeVersion = 2013;
                }

            }

            //检查本机是否安装Office2016       
            if (akey16 != null)
            {
                string office16 = akey16.GetValue("Path").ToString();
                if (File.Exists(office16 + "Excel.exe"))
                {
                    officeVersion = 2016;
                }
            }
            return officeVersion;
        }

    }
}
