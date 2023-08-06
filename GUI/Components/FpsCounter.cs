using System.Windows.Threading;
using System;
using System.Windows.Controls;

namespace Floppy_Plane_WPF.GUI.Components
{
    public class FpsCounter
    {
        private DispatcherTimer TickTimer { get; } = new() { Interval = TimeSpan.FromMilliseconds(1) };
        private DispatcherTimer UpdateTimer { get; } = new() { Interval = TimeSpan.FromSeconds(1) };

        private int Updates { get; set; } = 0;

        public int FPS { get; private set; } = 0;

        public FpsCounter()
        {
            TickTimer.Tick += (s, e) => Updates++;
            UpdateTimer.Tick += (s, e) => UpdateFPS();

            TickTimer.Start();
            UpdateTimer.Start();
        }

        public FpsCounter(Label FPSLabel) : this()
        {
            FPSLabel.Content = "FPS: 0";
            UpdateTimer.Tick += (s, e) => FPSLabel.Content = $"FPS: {FPS}";
        }

        private void UpdateFPS()
        {
            FPS = Updates;
            Updates = 0;
        }
    }
}
