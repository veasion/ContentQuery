using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace ContentQuery
{
    class SpireExtUtils
    {

        private static bool loaded = false;
        private static string licenseDll = "Spire.License.dll";
        private static string currentDirectory = Directory.GetCurrentDirectory();
        private static string downloadUrl = "https://veasion-oss.oss-cn-shanghai.aliyuncs.com/dll/";
        private static Dictionary<string, Assembly> dllMap = new Dictionary<string, Assembly>();

        public static Assembly LoadFile(string dll)
        {
            if (dllMap.ContainsKey(dll))
            {
                return dllMap[dll];
            }
            lock (dllMap)
            {
                if (dllMap.ContainsKey(dll))
                {
                    return dllMap[dll];
                }
                if (!loaded)
                {
                    loaded = true;
                    LoadFile(licenseDll);
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
