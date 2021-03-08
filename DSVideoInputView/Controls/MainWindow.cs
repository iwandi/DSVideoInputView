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
            
            captureView.Init(false);            

            InitSourceSettingsWindow();

            LoadSettings(new CaptureSettings
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
            });

            InitHideMechanic();
        }

        void LoadSettings(CaptureSettings settings)
        {
            var sourceName = settings;

            if(captureView.CaptureSourceList.TryGetByName(settings.SourceName, out var source))
            {
                SetSourceWithSettings(settings, source);
                return;
            }

            foreach(var entry in captureView.CaptureSourceList.List)
            {
                try
                {
                    SetSourceWithSettings(settings, entry);
                    return;
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
            }
        }

        void SetSourceWithSettings(CaptureSettings settings, CaptureSourceList.Entry source)
        {
            sourceSettingsWindow.SourceEntry = source;

            if (settings.SourceSettings != null && 
                settings.SourceSettings.TryGetValue(source.Name, out var sourceSettings))
            {
                ApplySettings(sourceSettings);
            }
            captureView.CaptureSource = source.Filter;
        }

        private void SourceMenuOpening(object sender, EventArgs e)
        {
            UpdateSourceList();
        }

        void UpdateSourceList()
        {
            sourceToolStripMenuItem.DropDownItems.Clear();

            var sourceList = captureView.CaptureSourceList;
            // TODO : updating this courses a issue when selection an new source in removing the old filter
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
            if(menuItem != null)
            {
                var source = menuItem.Tag as CaptureSourceList.Entry;
                if(source != null)
                {
                    captureView.CaptureSource = source.Filter;
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
            sourceSettingsWindow.SourceSettings = captureView.Settings;
            if(!sourceSettingsWindow.Visible)
                sourceSettingsWindow.ShowDialog();
        }

        void SettingsApply(SourceSettings settings, bool store)
        {
            ApplySettings(settings);

            if(store)
            {
                // TODO : Store Setting
            }
        }

        void ApplySettings(SourceSettings settings)
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
            captureView.Settings = settings;
            ResumeLayout();
        }

        private void AudioMenuOpening(object sender, EventArgs e)
        {

        }
    }
}
