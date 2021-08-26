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

        public static bool hasTextByWordExcel(FileInfo fileInfo, string text)
        {
            string ext = fileInfo.Extension.ToLower();
            string uriString;
            if (ext.Contains("xls"))
            {
                uriString = "/xl/sharedStrings.xml";
            }
            else
            {
                uriString = "/word/document.xml";
            }
            try
            {
                using (Package package = Package.Open(fileInfo.FullName))
                {
                    Uri docxUri = new Uri(uriString, UriKind.Relative);

                    PackagePart docxPart = package.GetPart(docxUri);

                    // XmlDocument docxXmlDocument = new XmlDocument();

                    // docxXmlDocument.Load(docxPart.GetStream());

                    // return docxXmlDocument.InnerText.ToString().Contains(text);

                    string line;
                    StreamReader sr = new StreamReader(docxPart.GetStream());
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
                Console.Error.WriteLine("加载" + fileInfo.Name + "异常：" + e.Message);
                return false;
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
    }
}
