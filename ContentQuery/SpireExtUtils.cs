using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace ContentQuery
{
    class SpireExtUtils
    {

        private static bool loaded = false;
        private static string licenseDll = "Spire.License.dll";
        private static string currentDirectory = Directory.GetCurrentDirectory();
        private static string downloadUrl = "https://veasion-oss.oss-cn-shanghai.aliyuncs.com/dll/";
        private static Dictionary<string, Assembly> dllMap = new Dictionary<string, Assembly>();

        public static void check()
        {
            if (!File.Exists(currentDirectory + "\\ContentQuery.exe"))
            {
                return;
            }
            new Thread(() =>
            {
                try
                {
                    LoadSpireFile("Spire.Doc.dll");
                    LoadSpireFile("Spire.XLS.dll");
                    LoadSpireFile("Spire.Pdf.dll");
                }
                catch (Exception) { }
            }).Start();
        }

        public static Assembly LoadFile(string dll)
        {
            if (dllMap.ContainsKey(dll))
            {
                return dllMap[dll];
            }
            return null;
        }

        private static Assembly LoadSpireFile(string dll)
        {
            if (dllMap.ContainsKey(dll))
            {
                return dllMap[dll];
            }
            if (dllMap.ContainsKey(dll))
            {
                return dllMap[dll];
            }
            if (!loaded)
            {
                loaded = true;
                LoadSpireFile(licenseDll);
            }
            string path = currentDirectory + "\\" + dll;
            if (!File.Exists(path))
            {
                // 下载dll
                downloadFile(downloadUrl + dll, path);
            }
            Assembly assembly = Assembly.LoadFile(path);
            if (assembly != null)
            {
                dllMap.Add(dll, assembly);
            }
            return assembly;
        }

        public static bool downloadFile(string url, string path)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(path);
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                Stream stream = new FileStream(path, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, bArr.Length);
                }
                stream.Close();
                responseStream.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("下载文件异常：" + e.Message);
            }
            return false;
        }
    }
}
