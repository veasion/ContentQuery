using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentQuery
{

    interface Search
    {

        bool hasText(FileInfo fileInfo, string text);

    }

    class SearchFactory
    {

        private static TextSearch textSearch = new TextSearch();
        private static WordSearch wordSearch = new WordSearch();
        private static ExcelSearch excelSearch = new ExcelSearch();
        private static PptSearch pptSearch = new PptSearch();
        private static PdfSearch pdfSearch = new PdfSearch();

        public static Search GetSearch(FileInfo fileInfo)
        {
            string name = fileInfo.Name;
            int index = name.LastIndexOf(".");
            string suffix = null;
            if (index > 0)
            {
                suffix = name.Substring(index + 1).ToLower();
            }
            if (suffix == null || suffix == "")
            {
                return textSearch;
            }
            switch (suffix)
            {
                case "txt": return textSearch;
                case "doc": return wordSearch;
                case "docx": return wordSearch;
                case "xls": return excelSearch;
                case "xlsx": return excelSearch;
                case "ppt": return pptSearch;
                case "pptx": return pptSearch;
                case "pdf": return pdfSearch;
            }
            return textSearch;
        }

    }
}
