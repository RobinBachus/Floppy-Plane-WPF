using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Input;
using Floppy_Plane_WPF.Controllers;
using Floppy_Plane_WPF.GameObjects;
using Floppy_Plane_WPF.GUI;
using Floppy_Plane_WPF.GUI.Components;

namespace Floppy_Plane_WPF
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Player Player { get;}
        public FpsCounter Fps { get; }
        private List<Enemy> Enemies { get; }
        private AnimationController AnimationController { get; }
        private Menu MenuController { get; }
        // private SoundPlayer SoundPlayer { get; }

        public MainWindow()
        {
            InitializeComponent();

            Player = new(Frame);
            Enemies = new List<Enemy>();
            AnimationController = new(Player, Frame, GameUI, GameOverScreen, Enemies);
            MenuController = new(this);
            Settings.AddSettingEventHandlers(AnimationController, Player, this);

            //SoundPlayer = new(Path.GetFullPath("Resources\\Sounds\\Engine.wav"));
            //SoundPlayer.LoadCompleted += (x, args) => { SoundPlayer.Play(); };
            //SoundPlayer.Load();

            Fps = new FpsCounter(FpsDisplay);

            Frame.Loaded += (s, e) => Player.Draw();
        }
        
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && (AnimationController.Started || AnimationController.CanRespawn))
            {
                if (!AnimationController.Started )
                {
                    AnimationController.StartPlayerAnimation();
                    Menu.Visibility = Visibility.Collapsed;
                    GameUI.Visibility = Visibility.Visible;
                }
                Player.Jump();
            }
        }
    }
}
