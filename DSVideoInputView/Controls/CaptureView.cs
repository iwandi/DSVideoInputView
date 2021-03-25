using System;
using System.Windows.Forms;
using System.Collections.Generic;
using DirectShowLib;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.ComponentModel;

namespace DSVideoInputView
{
    public enum PlayState
    {
        Stopped,
        Paused,
        Running,
        Init
    }; 

    public class CaptureView : Control, IDisposable
    {
        // Application-defined message to notify app of filtergraph events
        public const int WM_GRAPHNOTIFY = 0x8000 + 1;

        PlayState currentState;

        IGraphBuilder graphBuilder = null;

        IVideoWindow videoWindow = null;
        IMediaControl mediaControl = null;
        IMediaEventEx mediaEventEx = null;
        DsROTEntry graphBuilderRotEntry;

        ICaptureGraphBuilder2 captureGraphBuilder = null;

        IBaseFilter outputRenderer = null;
        IVMRAspectRatioControl aspectRatioControl = null;
        IVMRAspectRatioControl9 aspectRatioControl9 = null;

        IBaseFilter source = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBaseFilter Source
        {
            get { return source; }
        }

        SourceSettings sourceSettings;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SourceSettings SourceSettings
        {
            get { return sourceSettings; }
            set
            {
                sourceSettings = value;
                ApplySettings(value, audioSettings);
            }
        }

        IBaseFilter audioSource = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IBaseFilter AudioSource
        {
            get { return audioSource; }
        }

        AudioSettings audioSettings;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public AudioSettings AudioSettings
        {
            get { return audioSettings; }
            set
            {
                audioSettings = value;
                ApplySettings(sourceSettings, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FullScreen
        {
            get
            {
                var hr = videoWindow.get_FullScreenMode(out var value);
                DsError.ThrowExceptionForHR(hr);
                return value != OABool.False;
            }
            set
            {
                videoWindow.put_FullScreenMode(value ? OABool.True : OABool.False);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ShowCapture
        {
            get
            {
                return currentState == PlayState.Running;
            }
            set
            {
                if (mediaControl == null)
                    return;

                if (value)
                {
                    if (currentState != PlayState.Running)
                    {
                        var hr = mediaControl.Run();
                        DsError.ThrowExceptionForHR(hr);
                        currentState = PlayState.Running;
                    }
                }
                else
                {
                    var hr = mediaControl.StopWhenReady();
                    DsError.ThrowExceptionForHR(hr);
                    currentState = PlayState.Stopped;
                }
            }
        }

        public int SourceWidth { get; protected set; }
        public int SourceHeight { get; protected set; }

        public void Init()
        {
            currentState = PlayState.Init;
            InitDirectShow(Handle);

            Resize += ControlResize;
        }

        void InitDirectShow(IntPtr handle)
        {
            graphBuilder = (IGraphBuilder)new FilterGraph();
            mediaControl = (IMediaControl)(IVideoWindow)graphBuilder;
            videoWindow = (IVideoWindow)(IMediaControl)graphBuilder;
            mediaEventEx = (IMediaEventEx)graphBuilder;

            captureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();

            outputRenderer = (IBaseFilter)new VideoMixingRenderer9();

            if(outputRenderer != null)
            {
                graphBuilder.AddFilter(outputRenderer, "Output");

                aspectRatioControl = outputRenderer as IVMRAspectRatioControl;
                aspectRatioControl9 = outputRenderer as IVMRAspectRatioControl9;
            }

            var hr = mediaEventEx.SetNotifyWindow(handle, WM_GRAPHNOTIFY, IntPtr.Zero);
            DsError.ThrowExceptionForHR(hr);

            hr = captureGraphBuilder.SetFiltergraph(graphBuilder);
            DsError.ThrowExceptionForHR(hr);

            graphBuilderRotEntry = new DsROTEntry(graphBuilder);
        }

        void InitVideoWidnow(IntPtr handle)
        {
            var hr = videoWindow.put_Owner(handle);
            DsError.ThrowExceptionForHR(hr);

            hr = videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren);
            DsError.ThrowExceptionForHR(hr);

            hr = videoWindow.put_MessageDrain(handle);
            DsError.ThrowExceptionForHR(hr);

            UpdateVideoSize();

            hr = videoWindow.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);
        }

        // TODO : add audio playback
        public void Play(IBaseFilter source, IBaseFilter audioSource)
        {
            int hr = 0;

            if (currentState == PlayState.Running)
            {
                ShowCapture = false;

                if (this.source != null)
                {
                    hr = graphBuilder.RemoveFilter(this.source);
                    DsError.ThrowExceptionForHR(hr);
                }
                if (this.audioSource != null)
                {
                    hr = graphBuilder.RemoveFilter(this.audioSource);
                    DsError.ThrowExceptionForHR(hr);
                }
            }

            this.source = source;
            this.audioSource = audioSource;

            if (source == null)
            {
                ShowCapture = false;
                SourceWidth = 0;
                SourceHeight = 0;
            }
            else
            {
                if (source != null)
                {
                    hr = graphBuilder.AddFilter(source, "Video Capture");
                    DsError.ThrowExceptionForHR(hr);

                    hr = captureGraphBuilder.RenderStream(PinCategory.Preview, MediaType.Video, source, null, outputRenderer);
                    DsError.ThrowExceptionForHR(hr);
                }

                if (audioSource != null)
                {
                    hr = graphBuilder.AddFilter(audioSource, "Audio Capture");
                    DsError.ThrowExceptionForHR(hr);

                    hr = captureGraphBuilder.RenderStream(PinCategory.Capture, MediaType.Audio, audioSource, null, null);
                    DsError.ThrowExceptionForHR(hr);
                }

                InitVideoWidnow(Handle);

                hr = mediaControl.Run();
                DsError.ThrowExceptionForHR(hr);

                currentState = PlayState.Running;
            }
        }

        public void TryShowSource(VideoInputDeviceList sourceList,
            AudioInputDeviceList audioList)
        {
            foreach(var entry in sourceList.List)
            {
                try
                {
                    if (entry != null && entry.Filter != null)
                        Play(entry.Filter, AudioSource);
                    break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
            }

            foreach (var entry in audioList.List)
            {
                try
                {
                    if (entry != null && entry.Filter != null)
                        Play(Source, entry.Filter);
                    break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
            }
        }

        void UpdateVideoSize()
        {
            if(videoWindow != null)
            {
                int width = ClientSize.Width;
                int height = ClientSize.Height;

                videoWindow.SetWindowPosition(0, 0, width, height);
            }
        }

        void ControlResize(object sender, System.EventArgs e)
        {
            UpdateVideoSize();
        }

        const int WM_KEYUP = 0x0101;
        const int WM_LBUTTONDBLCLK = 0x203;

        const int VK_ESCAPE = 0x1B;
        const int VK_SPACE = 0x20;

        // TODO : find out how to not kill windows forms reciving messages
        // TODO : move the control code into the MainWindow code
        protected override void WndProc(ref Message m)
        {
            switch(m.Msg)
            {
                case WM_GRAPHNOTIFY:
                    HandleGraphEvent();
                    break;
                case WM_KEYUP:
                    switch(m.WParam.ToInt32())
                    {
                        case VK_ESCAPE:
                            FullScreen = false;
                            break;
                        case VK_SPACE:
                            FullScreen = !FullScreen;
                            break;
                    }
                    break;
                case WM_LBUTTONDBLCLK:
                    FullScreen = !FullScreen;
                    break;
                default:
                    //System.Diagnostics.Debug.WriteLine(m.Msg.ToString("X"));
                    break;
            }

            if (videoWindow != null)
                videoWindow.NotifyOwnerMessage(m.HWnd, m.Msg, m.WParam, m.LParam);

            base.WndProc(ref m);
        }

        void HandleGraphEvent()
        {
            if (mediaEventEx == null)
                return;

            while(mediaEventEx.GetEvent(out var eventCode, out var param1, out var param2, 0) == 0)
            {
                var hr = mediaEventEx.FreeEventParams(eventCode, param1, param2);
                DsError.ThrowExceptionForHR(hr);
            }
        }

        // TODO : Implment Strech
        // TODO : Implement Mute
        void ApplySettings(SourceSettings sourceSettings, AudioSettings audioSettings)
        {
            VMRAspectRatioMode aspectRatioMode = sourceSettings.SourceAspectFit == SourceAspectFit.LetterBox ? 
                VMRAspectRatioMode.LetterBox : 
                VMRAspectRatioMode.None;

            if (aspectRatioControl != null)
                aspectRatioControl.SetAspectRatioMode(aspectRatioMode);
            if (aspectRatioControl9 != null)
                aspectRatioControl9.SetAspectRatioMode(aspectRatioMode);
        }

        bool QueryResolution(IPin sourcePin, out int width, out int height)
        {
            IAMStreamConfig streamConfig = sourcePin as IAMStreamConfig;
            if (streamConfig != null)
            {
                AMMediaType format;
                streamConfig.GetFormat(out format);
                var iidVideoInfoHeader = typeof(VideoInfoHeader).GUID;
                var iidVideoInfoHeader2 = typeof(VideoInfoHeader2).GUID;
                if (format.formatType == iidVideoInfoHeader2)
                {
                    var videoInfo = Marshal.PtrToStructure<VideoInfoHeader2>(format.formatPtr);

                    width = videoInfo.BmiHeader.Width;
                    height = videoInfo.BmiHeader.Height;
                    return true;
                }
                else if (format.formatType == iidVideoInfoHeader)
                {
                    var videoInfo = Marshal.PtrToStructure<VideoInfoHeader>(format.formatPtr);

                    width = videoInfo.BmiHeader.Width;
                    height = videoInfo.BmiHeader.Height;
                    return true;
                }
            }
            width = 0;
            height = 0;
            return false;
        }

        public new void Dispose()
        {
            base.Dispose();
            DeInitDirectShow();

            Resize -= ControlResize;
        }

        void DeInitDirectShow()
        {
            if (mediaControl != null)
            {
                mediaControl.StopWhenReady();
            }

            if(mediaEventEx != null)
            {
                mediaEventEx.SetNotifyWindow(IntPtr.Zero, WM_GRAPHNOTIFY, IntPtr.Zero);
            }

            if(videoWindow != null)
            {
                videoWindow.put_Visible(OABool.False);
                videoWindow.put_Owner(IntPtr.Zero);
            }

            Marshal.ReleaseComObject(mediaControl);
            mediaControl = null;
            Marshal.ReleaseComObject(mediaEventEx);
            mediaControl = null;
            Marshal.ReleaseComObject(videoWindow);
            videoWindow = null;
            Marshal.ReleaseComObject(graphBuilder);
            graphBuilder = null;
            Marshal.ReleaseComObject(captureGraphBuilder);
            captureGraphBuilder = null;
            Marshal.ReleaseComObject(outputRenderer);
            outputRenderer = null;
            Marshal.ReleaseComObject(aspectRatioControl);
            aspectRatioControl = null;
            Marshal.ReleaseComObject(aspectRatioControl9);
            aspectRatioControl9 = null;
        }
    }
}
