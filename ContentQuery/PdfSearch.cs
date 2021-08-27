using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ContentQuery
{
    class PdfSearch : Search
    {
        public bool hasText(FileInfo fileInfo, string text)
        {
            try
            {
                Assembly assem = SpireExtUtils.LoadFile("Spire.Pdf.dll");
                var type = assem.GetType("Spire.Pdf.PdfDocument");
                var obj = Activator.CreateInstance(type);
                try
                {
                    type.GetMethod("LoadFromFile", new Type[] { typeof(string) }).Invoke(obj, new string[] { fileInfo.FullName });
                    var pages = type.GetProperty("Pages").GetValue(obj, null) as IEnumerable;
                    if (pages == null)
                    {
                        return false;
                    }
                    foreach (var page in pages)
                    {
                        string context = page.GetType().GetMethod("ExtractText", new Type[] { }).Invoke(page, null) as string;
                        if (context != null && context.Contains(text))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                finally
                {
                    type.GetMethod("Close").Invoke(obj, null);
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += "；" + e.InnerException.Message;
                }
                Console.Error.WriteLine("加载pdf异常: " + message + " > " + fileInfo.FullName);
                return false;
            }
        }
    }
}
