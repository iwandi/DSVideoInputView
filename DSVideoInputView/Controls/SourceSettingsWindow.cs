using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DirectShowLib;
using DirectShowLib.Utils;

namespace DSVideoInputView
{
    public partial class SourceSettingsWindow : Form
    {
        static readonly string[] Multi = new string[]
        {
            "Custom",

            "1x1",
            "1x2",
            "1x3",
            "1x4",

            "2x1",
            "2x2",
            "2x3",
            "2x4",

            "3x1",
            "3x2",
            "3x3",
            "3x4",

            "4x1",
            "4x2",
            "4x3",
            "4x4",
        };

        static readonly string[] Resolutins = new string[]
        {
            "Custom",

            // HD resolutions
            "1280 x 720", // 720p
            "1360 x 768",
            "1366 x 768",
            "1920 x 1080", // 1080p
            "2560 x 1440", // 1440p
            "3840 x 2160 ", // 4k


            // VGA 
            "240 x 160", // HQVGA 3:2
            "320 x 240", // QVGA 4:3
            "480 x 320", // HVGA 3:2
            "640 x 480", // VGA 4:3
            "720 x 400", // WVGA 3:2
            "800 x 600", // SVGA 4:3
            "1024 x 768", // XGA 4:3
            "1152 x 864", // XGA+ 4:3
            "1280 x 1024", // SXGA 5:4
            "1400 x 1050", // SXGA+ 4:3
            "1600 x 1200", // UXGA 4:3

            // Wide Screen 16:9 & 16:10            
            "1600 x 768",

            // Old stuff
            "140 x 192",
            "160 x 200",
            "320 x 200",
            "320 x 240",
            "480 x 272", // PSP
            "640 x 200",
            "640 x 256",
            "640 x 350",
            "720 x 348",
            "720 x 350",
            "1280 x 960", // Atari TT
        };

        SourceSettings resetSettings;
        public SourceSettings SourceSettings
        {
            get
            {
                var settings = new SourceSettings();
                WriteSourceSettings(ref settings);
                return settings;
            }
            set
            {
                resetSettings = value;
                ReadSourceSettings(value);
            }
        }

        AudioSettings resetAudioSettings;
        public AudioSettings AudioSettings
        {
            get
            {
                var settings = new AudioSettings();
                WriteAudioSettings(ref settings);
                return settings;
            }
            set
            {
                resetAudioSettings = value;
                ReadAudioSettings(value);
            }
        }

        DeviceEntry entry;
        public DeviceEntry SourceEntry
        {
            get { return entry; }
            set
            {
                entry = value;
                UpdateEntryUi();
            }
        }

        DeviceEntry audioEntry;
        public DeviceEntry AudioEntry
        {
            get { return audioEntry; }
            set
            {
                audioEntry = value;
                UpdateEntryUi();
            }
        }

        public event Action<SourceSettings, AudioSettings, bool> OnApply;

        bool customMulti = false;
        bool customResolution = false;

        public SourceSettingsWindow()
        {
            InitializeComponent();
            FillLists();
        }

        void FillLists()
        {
            listBoxSource.Items.Clear();
            listBoxSource.Items.AddRange(Multi);

            listBoxOutput.Items.Clear();
            listBoxOutput.Items.AddRange(Resolutins);
        }

        void ReadSourceSettings(SourceSettings settings)
        {
            SuspendLayout();
            radioButtonLetterBox.Checked = settings.SourceAspectFit == SourceAspectFit.LetterBox;
            radioButtonStrech.Checked = settings.SourceAspectFit == SourceAspectFit.Strech;
            radioButtonSourceBased.Checked = !settings.FixedResolution;
            radioButtonOutputBased.Checked = settings.FixedResolution;

            textWidth.Text = settings.ResolutionWidth.ToString();
            textHeight.Text = settings.ResolutionHeight.ToString();

            textWidthMulti.Text = settings.MultiplyerWidth.ToString();
            textHeightMulti.Text = settings.MultiplyerHeight.ToString();

            customMulti = !TrySelectWidthHeightFromList(listBoxSource, 
                settings.MultiplyerWidth, 
                settings.MultiplyerHeight);

            customResolution = !TrySelectWidthHeightFromList(listBoxOutput,
                settings.ResolutionWidth,
                settings.ResolutionHeight);

            UpdateEnabled();
            ResumeLayout();
        }        

        void WriteSourceSettings(ref SourceSettings settings)
        {
            settings.SourceAspectFit = radioButtonLetterBox.Checked ? SourceAspectFit.LetterBox : SourceAspectFit.Strech;
            settings.FixedResolution = radioButtonOutputBased.Checked;

            GetWidthHeight(listBoxSource,
                textWidthMulti,
                textHeightMulti,
                out settings.MultiplyerWidth,
                out settings.MultiplyerHeight);

            GetWidthHeight(listBoxOutput,
                textWidth,
                textHeight,
                out settings.ResolutionWidth,
                out settings.ResolutionHeight);
        }

        void ReadAudioSettings(AudioSettings settings)
        {

        }

        void WriteAudioSettings(ref AudioSettings settings)
        {

        }

        void Apply(bool store)
        {
            var settings = new SourceSettings();
            var audioSettings = new AudioSettings();
            WriteSourceSettings(ref settings);
            WriteAudioSettings(ref audioSettings);

            if (OnApply != null)
                OnApply(settings, audioSettings, store);
        }

        void Reset(bool show)
        {
            if(show)
            {
                ReadSourceSettings(resetSettings);
                ReadAudioSettings(resetAudioSettings);
            }

            if (OnApply != null)
                OnApply(resetSettings, resetAudioSettings, false);
        }

        private void ApplyClick(object sender, EventArgs e)
        {
            Apply(false);
        }

        private void CancelClick(object sender, EventArgs e)
        {
            Reset(false);
            Close();
        }

        private void OkClick(object sender, EventArgs e)
        {
            Apply(true);
            Close();
        }

        private void ResolutionSourceCheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        void UpdateEnabled()
        {
            bool sourceBased = radioButtonSourceBased.Checked;
            bool custom = sourceBased ? customMulti : customResolution;

            textWidthMulti.Enabled = sourceBased && custom;
            textHeightMulti.Enabled = sourceBased && custom;
            listBoxSource.Enabled = sourceBased;

            textWidth.Enabled = !sourceBased && custom;
            textHeight.Enabled = !sourceBased && custom;
            listBoxOutput.Enabled = !sourceBased;
        }

        void UpdateEntryUi()
        {
            SuspendLayout();

            if (entry != null)
            {
                labelSourceName.Text = $"Name: {entry.Name}";
                labelSourceDisplayName.Text = $"DisplayName: {entry.DisplayName}";
                buttonConfig.Enabled = true;

                bool hasDialogue = FilterGraphTools.HasPropertyPages(entry.Filter);
                buttonConfig.Enabled = hasDialogue;
            }
            else
            {
                ClearSourceUI();
            }

            if(audioEntry != null)
            {
                labelAudioName.Text = $"Name: {audioEntry.Name}";
                labelAudioDisplayName.Text = $"DisplayName: {audioEntry.DisplayName}";
                buttonAudioConfig.Enabled = true;
            }
            else
            {
                ClearAudioUI();
            }

            ResumeLayout();
        }

        void ClearSourceUI()
        {
            labelSourceName.Text = "";
            labelSourceDisplayName.Text = "";
            buttonConfig.Enabled = false;
        }

        void ClearAudioUI()
        {
            labelAudioName.Text = "";
            labelAudioDisplayName.Text = "";
            buttonAudioConfig.Enabled = false;
        }

        bool TryGetWidthHeightFromList(ListBox listBox, out int width, out int height)
        {
            width = 0;
            height = 0;

            var itemText = listBox.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(itemText))
                return false;

            string[] token = itemText.Split('x');
            if(token.Length >= 2)
            {
                return int.TryParse(token[0], out width) && int.TryParse(token[1], out height);
            }
            return false;
        }

        void GetWidthHeight(ListBox listBox, 
            TextBox textWidth, TextBox textHeight, 
            out int width, out int height)
        {
            if(!TryGetWidthHeightFromList(listBox, out width, out height))
            {
                int.TryParse(textWidth.Text, out width);
                int.TryParse(textHeight.Text, out height);
            }
        }

        bool TrySelectWidthHeightFromList(ListBox listBox, int width, int height)
        {
            string selectedtext = $"{width} x {height}";
            listBox.SelectedItem = selectedtext;
            return listBox.SelectedIndex >= 0;
        }

        private void SourceSelectionChnaged(object sender, EventArgs e)
        {
            int width = 0;
            int height = 0;

            customMulti = !TryGetWidthHeightFromList(listBoxSource, out width, out height);

            if (!customMulti)
            {
                textWidthMulti.Text = width.ToString();
                textHeightMulti.Text = height.ToString();
            }

            UpdateEnabled();
        }

        private void OutputSelectionChnaged(object sender, EventArgs e)
        {
            int width = 0;
            int height = 0;

            customResolution = !TryGetWidthHeightFromList(listBoxOutput, out width, out height);

            if (!customResolution)
            {
                textWidth.Text = width.ToString();
                textHeight.Text = height.ToString();
            }

            UpdateEnabled();
        }

        private void ConfigClick(object sender, EventArgs e)
        {
            ShowPropertyPage(entry);
        }

        private void AudioConfigClick(object sender, EventArgs e)
        {

            ShowPropertyPage(audioEntry);
        }

        private void ShowPropertyPage(DeviceEntry device)
        {
            if (device != null && device.Filter != null)
                FilterGraphTools.ShowFilterPropertyPage(device.Filter, Handle);
        }
    }
}
