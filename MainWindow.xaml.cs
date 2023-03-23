using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Floppy_Plane_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Player Player { get; set; }
        private List<Enemy> Enemies { get; set; }
        private AnimationController AnimationController { get; set; }
        private bool Started { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Started = false; 
            Enemies = new List<Enemy>();
            Frame.Loaded += Canvas_OnLoad;
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (!Started)
                {
                    Started = true;
                    AnimationController.StartPlayerAnimation();
                }
                Player.Jump();
            }
        }

        private void Canvas_OnLoad(object sender, RoutedEventArgs routedEvent)
        {
            Frame.Focus();
            Frame.Background = Brushes.LightBlue;
            Player = new(Frame);
            AnimationController = new(Player, Frame, Enemies);
        }
    }
}
