using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace ContentQuery
{
    class WordSearch : Search
    {

        public bool hasText(FileInfo fileInfo, string text)
        {
            try
            {
                if (".doc".Equals(fileInfo.Extension.ToLower().Trim()))
                {
                    return hasTextByOld(fileInfo, text);
                }
                return FileUtils.hasTextByPackage(fileInfo, text, "/word/document.xml");
            }
            catch (Exception e)
            {
                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += "；" + e.InnerException.Message;
                }
                Console.Error.WriteLine("加载doc异常: " + message + " > " + fileInfo.FullName);
                return false;
            }
        }

        private bool hasTextByOld(FileInfo fileInfo, string text)
        {
            Assembly assem = SpireExtUtils.LoadFile("Spire.Doc.dll");
            if (assem == null) return false;
            var type = assem.GetType("Spire.Doc.Document");
            var obj = Activator.CreateInstance(type);
            try
            {
                type.GetMethod("LoadFromFile", new Type[] { typeof(string) }).Invoke(obj, new string[] { fileInfo.FullName });
                // return (type.GetMethod("GetText").Invoke(obj, null) as string).Contains(text);
                // type.GetMethod("LoadFromFileInReadMode", new Type[] { typeof(string), assem.GetType("Spire.Doc.FileFormat") }).Invoke(obj, new string[] { fileInfo.FullName, null });
                var textSelection = type.GetMethod("FindString", new Type[] { typeof(string), typeof(bool), typeof(bool) }).Invoke(obj, new object[] { text, true, false });
                return textSelection != null;
            }
            finally
            {
                type.GetMethod("Close").Invoke(obj, null);
            }
        }

    }
}
