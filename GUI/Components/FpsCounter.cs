using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Floppy_Plane_WPF.GUI.Components
{
    public class FpsCounter
    {
        private DispatcherTimer TickTimer { get; } = new() { Interval = TimeSpan.FromMilliseconds(1) };
        private DispatcherTimer UpdateTimer { get; } = new() { Interval = TimeSpan.FromSeconds(1) };

        private int Updates { get; set; }

        public int Fps { get; private set; }

        public FpsCounter()
        {
            TickTimer.Tick += delegate { Updates++; };
            UpdateTimer.Tick += delegate { UpdateFps(); };

            TickTimer.Start();
            UpdateTimer.Start();
        }

        public FpsCounter(ContentControl fpsLabel) : this()
        {
            fpsLabel.Content = "FPS: 0";
            UpdateTimer.Tick += delegate { fpsLabel.Content = $"FPS: {Fps}"; };
        }

        private void UpdateFps()
        {
            Fps = Updates;
            Updates = 0;
        }
    }
}
