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
            try
            {
                return FileUtils.hasText(fileInfo, text, FileUtils.GetFileEncodeType(fileInfo.FullName));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
