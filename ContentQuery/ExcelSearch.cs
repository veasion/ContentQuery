using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentQuery
{
    class ExcelSearch : Search
    {

        public bool hasText(FileInfo fileInfo, string text)
        {
            try
            {
                if (".xls".Equals(fileInfo.Extension.ToLower().Trim()))
                {
                    return hasTextByOld(fileInfo, text);
                }
                return FileUtils.hasTextByPackage(fileInfo, text, "/xl/sharedStrings.xml");
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool hasTextByOld(FileInfo fileInfo, string text)
        {
            return false;
        }
    }
}
