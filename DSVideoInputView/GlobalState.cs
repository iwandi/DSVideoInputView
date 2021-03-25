using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DSVideoInputView
{
    public class GlobalState : IDisposable
    {
        const string AppName = "DSVideoInputView";
        const string SettingsFileName = "config.json";

        static readonly JsonSerializerSettings JsonOptions = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            Culture = System.Globalization.CultureInfo.InvariantCulture,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            TypeNameHandling = TypeNameHandling.None,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore,
        };

        string SettingsPath;
        string SettingsFilePath;

        public CaptureSettings Settings;

        public VideoInputDeviceList SourceList { get; protected set; }
        public AudioInputDeviceList AudioList { get; protected set; }

        public GlobalState()
        {
            SettingsPath = Application.LocalUserAppDataPath;
            SettingsFilePath = Path.Combine(SettingsPath, SettingsFileName);

            SourceList = new VideoInputDeviceList();
            AudioList = new AudioInputDeviceList();

            Settings = new CaptureSettings();
            /*{
                //SourceName = "3",
                //SourceName = "CY",
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

                //AudioName = "Line In",
            };*/
        }

        public void LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    var json = File.ReadAllText(SettingsFilePath);
                    Settings = JsonConvert.DeserializeObject<CaptureSettings>(json, JsonOptions);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                Settings = new CaptureSettings();
            }
        }

        public void SetSourceDevice(string deciceName)
        {
            var settings = Settings;
            settings.SourceName = deciceName;
            Settings = settings;
        }

        public void SetAudioDevice(string deciceName)
        {
            var settings = Settings;
            settings.AudioName = deciceName;
            Settings = settings;
        }

        public void SetSettings(string deviceName, IDeviceSettings deviceSettings)
        {
            var settings = Settings;

            if(deviceSettings is SourceSettings)
            {
                if (settings.SourceSettings == null)
                    settings.SourceSettings = new Dictionary<string, SourceSettings>();
                settings.SourceSettings[deviceName] = (SourceSettings)deviceSettings;
            }
            else if ( deviceSettings is AudioSettings)
            {
                if (settings.AudioSettings == null)
                    settings.AudioSettings = new Dictionary<string, AudioSettings>();
                settings.AudioSettings[deviceName] = (AudioSettings)deviceSettings;
            }

            Settings = settings;
        }

        public void SaveSettings()
        {
            try
            {
                var json = JsonConvert.SerializeObject(Settings, JsonOptions);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                Settings = new CaptureSettings();
            }
        }

        public void Dispose()
        {
            SourceList.Dispose();
            AudioList.Dispose();
        }
    }
}
