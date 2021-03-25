using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSVideoInputView
{
    public class GlobalState : IDisposable
    {
        public CaptureSettings Settings;

        public VideoInputDeviceList SourceList { get; protected set; }
        public AudioInputDeviceList AudioList { get; protected set; }

        public GlobalState()
        {
            SourceList = new VideoInputDeviceList();
            AudioList = new AudioInputDeviceList();

            Settings = new CaptureSettings
            {
                //SourceName = "3",
                SourceName = "CY",
                //SourceName = "Unknown (3)",

                SourceSettings = new Dictionary<string, SourceSettings>
                {
                    {  "CY3014 USB, Analog 01 Capture", new SourceSettings
                        {
                            SourceAspectFit = SourceAspectFit.LetterBox,
                            FixedResolution = true,

                            ResolutionWidth = 1024,
                            ResolutionHeight = 768,

                            MultiplyerHeight = 1,
                            MultiplyerWidth = 1,
                        }
                    },
                },

                AudioName = "Line In",
            };
        }

        public void Dispose()
        {
            SourceList.Dispose();
            AudioList.Dispose();
        }
    }
}
