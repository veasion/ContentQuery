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
            return FileUtils.hasTextByWordExcel(fileInfo, text);
        }
    }
}
