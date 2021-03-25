using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DirectShowLib;

namespace DSVideoInputView
{
    // TODO : Load and store Slected source to config
    // TODO : Set SourceSettings from config based on Selected Source
    public partial class MainWindow : Form
    {
        SourceSettingsWindow sourceSettingsWindow;

        DeviceEntry activeSource;
        DeviceEntry activeAudio;

        Timer hideTimer = new Timer();

        bool mouseDrag;
        int mouseDragX;
        int mouseDragY;

        bool borderVisible;
        bool BorderVisible
        {
            get { return borderVisible; }
            set
            {
                if(borderVisible != value)
                {
                    borderVisible = value;
                    SuspendLayout();
                    FormBorderStyle = borderVisible ? FormBorderStyle.Sizable : FormBorderStyle.None;
                    MainMenuStrip.Visible = borderVisible;

                    var loc = Location;
                    loc.Y += borderVisible ? -55 : +55;
                    loc.X += borderVisible ? -7 : +7;
                    Location = loc;

                    ResumeLayout();
                }
            }
        }

        public MainWindow()
        {
            InitSourceSettingsWindow();
            InitializeComponent();
            
            captureView.Init();

            LoadSettings(Program.State.Settings);

            InitHideMechanic();
        }

        void LoadSettings(CaptureSettings settings)
        {
            var sourceName = settings;

            if(Program.State.SourceList.TryGetByName(settings.SourceName, out var source) &&
                Program.State.AudioList.TryGetByName(settings.AudioName, out var audioSource))
            {
                SetSourceWithSettings(settings, source, audioSource);
                return;
            }

            foreach(var entry in Program.State.SourceList.List)
            {
                try
                {
                    SetSourceWithSettings(settings, entry, activeAudio);
                    return;
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
            }

            foreach (var entry in Program.State.AudioList.List)
            {
                try
                {
                    SetSourceWithSettings(settings, activeSource, entry);
                    return;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
            }
        }

        void SetSourceWithSettings(CaptureSettings settings, DeviceEntry source, DeviceEntry audioSource)
        {
            activeSource = source;
            activeAudio = audioSource;

            captureView.Play(source?.Filter, audioSource?.Filter);
            if (settings.SourceSettings != null && 
                settings.SourceSettings.TryGetValue(source?.Name, out var sourceSettings))
            {
                ApplySettings(sourceSettings, captureView.AudioSettings);
            }
            if (settings.AudioSettings != null &&
                settings.AudioSettings.TryGetValue(audioSource?.Name, out var audioSettings))
            {
                ApplySettings(captureView.SourceSettings, audioSettings);
            }
        }

        private void SourceMenuOpening(object sender, EventArgs e)
        {
            UpdateSourceList();
        }

        void UpdateSourceList()
        {
            sourceToolStripMenuItem.DropDownItems.Clear();

            var sourceList = Program.State.SourceList;
            // TODO : updating this courses a issue when selection an new source in removing the old filter
            // INFO : this my be coused by destroying the Filters while in use by the renderer
            //sourceList.UpdateList();

            foreach(var entry in sourceList.List)
            {
                var menuItem = new ToolStripMenuItem(entry.DisplayName);
                menuItem.Tag = entry;
                menuItem.Click += SourceMenuItemClick;
                sourceToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        void SourceMenuItemClick(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                var source = menuItem.Tag as DeviceEntry;
                if (source != null)
                {
                    SetSourceWithSettings(Program.State.Settings, source, activeAudio);
                }
            }
        }

        private void AudioMenuOpening(object sender, EventArgs e)
        {
            UpdateAudioList();
        }

        void UpdateAudioList()
        {
            audioToolStripMenuItem.DropDownItems.Clear();

            var audioList = Program.State.AudioList;
            // TODO : updating this courses a issue when selection an new source in removing the old filter
            // INFO : this my be coused by destroying the Filters while in use by the renderer
            //sourceList.UpdateList();

            foreach (var entry in audioList.List)
            {
                var menuItem = new ToolStripMenuItem(entry.DisplayName);
                menuItem.Tag = entry;
                menuItem.Click += AudioMenuItemClick;
                audioToolStripMenuItem.DropDownItems.Add(menuItem);
            }
        }

        void AudioMenuItemClick(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                var source = menuItem.Tag as DeviceEntry;
                if (source != null)
                {
                    SetSourceWithSettings(Program.State.Settings, activeSource, source);
                }
            }
        }

        void VideoMouseEnter(object sender, EventArgs e)
        {
            BorderVisible = true;
        }

        void VideoMouseLeave(object sender, EventArgs e)
        {
            hideTimer.Stop();
            hideTimer.Start();
        }

        void VideoMouseDown(object sennder, MouseEventArgs e)
        {
            mouseDrag = true;

            mouseDragX = e.X;
            mouseDragY = e.Y;
        }

        void VideoMouseMove(object sennder, MouseEventArgs e)
        {
            if (mouseDrag)
            {
                int deltaX = e.X - mouseDragX;
                int deltaY = e.Y - mouseDragY;

                var loc = Location;
                loc.X += deltaX;
                loc.Y += deltaY;
                Location = loc;

                mouseDragX = e.X;
                mouseDragY = e.Y;
            }
        }

        void VideoMouseUp(object sennder, MouseEventArgs e)
        {
            mouseDrag = false;
        }

        void InitHideMechanic()
        {
            hideTimer.Interval += 1000;
            hideTimer.Tick += HideTimerTick;

            captureView.MouseEnter += VideoMouseEnter;
            captureView.MouseLeave += VideoMouseLeave;

            // TODO : while moving the windows is flickering and messing up the move
            /*captureView.MouseDown += VideoMouseDown;
            captureView.MouseMove += VideoMouseMove;
            captureView.MouseUp += VideoMouseUp;*/
        }

        void HideTimerTick(object sender, EventArgs e)
        {
            BorderVisible = false;
        }

        void InitSourceSettingsWindow()
        {
            sourceSettingsWindow = new SourceSettingsWindow();
            sourceSettingsWindow.Hide();

            sourceSettingsWindow.OnApply += SettingsApply;
        }

        private void SettingsToolStripMenuItemClick(object sender, EventArgs e)
        {
            sourceSettingsWindow.SourceSettings = captureView.SourceSettings;
            sourceSettingsWindow.AudioSettings = captureView.AudioSettings;

            sourceSettingsWindow.SourceEntry = activeSource;
            sourceSettingsWindow.AudioEntry = activeAudio;

            if (!sourceSettingsWindow.Visible)
                sourceSettingsWindow.ShowDialog();
        }

        void SettingsApply(SourceSettings settings, AudioSettings audioSettings, bool store)
        {
            ApplySettings(settings, audioSettings);

            if(store)
            {
                // TODO : Store Setting, need to know the name of the device
            }
        }

        void ApplySettings(SourceSettings settings, AudioSettings audioSettings)
        {
            SuspendLayout();
            var size = ClientSize;
            if (settings.FixedResolution)
            {
                size.Width = settings.ResolutionWidth;
                size.Height = settings.ResolutionHeight;
            }
            else if(captureView.SourceWidth != 0 && captureView.SourceHeight != 0)
            {
                size.Width = settings.MultiplyerWidth * captureView.SourceWidth;
                size.Height = settings.MultiplyerHeight * captureView.SourceHeight;
            }
            ClientSize = size;
            captureView.SourceSettings = settings;
            captureView.AudioSettings = audioSettings;
            ResumeLayout();
        }
    }
}
