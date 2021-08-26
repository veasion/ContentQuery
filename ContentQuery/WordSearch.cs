using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Xml;

namespace ContentQuery
{
    class WordSearch : Search
    {
        public bool hasText(FileInfo fileInfo, string text)
        {
            return FileUtils.hasTextByWordExcel(fileInfo, text);
        }
    }
}
