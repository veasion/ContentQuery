using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentQuery
{
    class PptSearch : Search
    {
        public bool hasText(FileInfo fileInfo, string text)
        {
            try
            {
                if (".ppt".Equals(fileInfo.Extension.ToLower().Trim()))
                {
                    return hasTextByOld(fileInfo, text);
                }
                for (int i = 1; i <= 30; i++)
                {
                    bool result = FileUtils.hasTextByPackage(fileInfo, text, "/ppt/slides/slide" + i + ".xml");
                    if (result)
                    {
                        return true;
                    }
                }
            }
            catch (Exception) { }
            return false;
        }

        private bool hasTextByOld(FileInfo fileInfo, string text)
        {
            return false;
        }
    }
}
