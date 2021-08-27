using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            catch (Exception e)
            {
                string message = e.Message;
                if (e.InnerException != null)
                {
                    message += "；" + e.InnerException.Message;
                }
                Console.Error.WriteLine("加载xls异常: " + message);
                return false;
            }
        }

        private bool hasTextByOld(FileInfo fileInfo, string text)
        {
            Assembly assem = SpireExtUtils.LoadFile("Spire.XLS.dll");
            if (assem == null) return false;
            var type = assem.GetType("Spire.Xls.Workbook");
            var obj = Activator.CreateInstance(type);
            try
            {
                type.GetMethod("LoadFromFile", new Type[] { typeof(string) }).Invoke(obj, new string[] { fileInfo.FullName });
                var sheets = type.GetProperty("Worksheets").GetValue(obj, null) as IEnumerable;
                foreach (var sheet in sheets)
                {
                    var range = sheet.GetType()
                        .GetMethod("FindString", new Type[] { typeof(string), typeof(bool), typeof(bool) })
                        .Invoke(sheet, new object[] { text, false, false });
                    if (range != null)
                    {
                        return true;
                    }
                }
            }
            finally
            {
                type.GetMethod("Close").Invoke(obj, null);
            }
            return false;
        }
    }
}
