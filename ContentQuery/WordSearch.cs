﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContentQuery
{
    class WordSearch : Search
    {
        public bool hasText(FileInfo fileInfo, string text)
        {
            return false;
        }
    }
}
