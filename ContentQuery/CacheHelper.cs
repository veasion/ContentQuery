using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentQuery
{
    class CacheHelper
    {
        private static string dir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private static string file = dir.Substring(0, dir.LastIndexOf("\\") + 1) + "queryCache.confg";

        public static void cacheOtherText(string text)
        {
            File.WriteAllText(file, text);
        }

        public static string getOtherText()
        {
            if (!File.Exists(file))
            {
                return "";
            }
            else
            {
                return File.ReadAllText(file);
            }
        }

    }
}
