using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentQuery
{
    class FileSplitUtils
    {

        private static HashSet<string> extDifficultySet = new HashSet<string>();

        static FileSplitUtils()
        {
            extDifficultySet.Add(".doc");
            extDifficultySet.Add(".xls");
            extDifficultySet.Add(".pdf");
        }

        public static List<FileInfo[]> optimizeSplit(FileInfo[] frr, int simpleMaxCount, int difficultyMaxCount)
        {
            List<FileInfo> simpleList = new List<FileInfo>();
            List<FileInfo> difficultyList = new List<FileInfo>();
            foreach (var item in frr)
            {
                if (extDifficultySet.Contains(item.Extension))
                {
                    difficultyList.Add(item);
                }
                else
                {
                    simpleList.Add(item);
                }
            }
            List<FileInfo[]> result = new List<FileInfo[]>();
            List<FileInfo[]> simple = split(simpleList, simpleMaxCount);
            List<FileInfo[]> difficulty = split(difficultyList, difficultyMaxCount);
            if (simple != null)
            {
                result.AddRange(simple);
            }
            if (difficulty != null)
            {
                result.AddRange(difficulty);
            }
            return result;
        }

        public static List<FileInfo[]> split(FileInfo[] frr, int maxCount)
        {
            return split(new List<FileInfo>(frr), maxCount);
        }

        public static List<FileInfo[]> split(List<FileInfo> list, int maxCount)
        {
            if (list == null || list.Count == 0)
            {
                return null;
            }
            List<FileInfo[]> result = new List<FileInfo[]>();
            for (int idx = 0; idx < list.Count; idx += maxCount)
            {
                FileInfo[] arr = split(list, idx, maxCount);
                result.Add(arr);
            }
            return result;
        }

        private static FileInfo[] split(List<FileInfo> frr, int startIndex, int length)
        {
            if (startIndex + length > frr.Count)
            {
                length = frr.Count - startIndex;
            }
            FileInfo[] arr = new FileInfo[length];
            for (int i = 0; i < length; i++)
            {
                arr[i] = frr[startIndex + i];
            }
            return arr;
        }

    }
}
