﻿using System;
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
            var type = assem.GetType("Spire.Doc.Document");
            var obj = Activator.CreateInstance(type);
            try
            {
                type.GetMethod("LoadFromFile", new Type[] { typeof(string) }).Invoke(obj, new string[] { fileInfo.FullName });
                var context = type.GetMethod("GetText").Invoke(obj, null) as string;
                return context.Contains(text);
            }
            finally
            {
                type.GetMethod("Close").Invoke(obj, null);
            }
        }

    }
}
