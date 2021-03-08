﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSVideoInputView
{
    public struct CaptureSettings
    {
        public string SourceName;
        public Dictionary<string,SourceSettings> SourceSettings;
    }

    public struct SourceSettings
    {
        public SourceAspectFit SourceAspectFit;
        public bool FixedResolution;

        public int ResolutionWidth;
        public int ResolutionHeight;

        public int MultiplyerWidth;
        public int MultiplyerHeight;
    }

    public enum SourceAspectFit
    {
        LetterBox = 0,
        Strech = 1,
    }
}
