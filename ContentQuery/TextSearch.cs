using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentQuery
{
    class TextSearch : Search
    {
        public bool hasText(FileInfo fileInfo, string text)
        {
            return hasTextIgnoreEncoding(fileInfo, text);
        }

        public static bool hasTextIgnoreEncoding(FileInfo fileInfo, string text)
        {
            bool has = hasText(fileInfo, text, Encoding.UTF8);
            if (!has)
            {
                has = hasText(fileInfo, text, Encoding.ASCII);
            }
            return has;
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
    }
}
