using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSVideoInputView
{
    [Serializable]
    public struct CaptureSettings
    {
        public string SourceName;
        public Dictionary<string,SourceSettings> SourceSettings;

        public string AudioName;
        public Dictionary<string, AudioSettings> AudioSettings;
    }

    public interface IDeviceSettings
    {

    }

    [Serializable]
    public struct SourceSettings : IDeviceSettings
    {
        public SourceAspectFit SourceAspectFit;
        public bool FixedResolution;

        public int ResolutionWidth;
        public int ResolutionHeight;

        public int MultiplyerWidth;
        public int MultiplyerHeight;
    }

    [Serializable]
    public struct AudioSettings : IDeviceSettings
    {
        public bool Mute;
    }

    public enum SourceAspectFit
    {
        LetterBox = 0,
        Strech = 1,
    }
}
