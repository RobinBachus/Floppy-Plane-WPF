using Floppy_Plane_WPF.Controllers;
using Floppy_Plane_WPF.GameObjects;
using Floppy_Plane_WPF.GUI;
using Floppy_Plane_WPF.GUI.Components;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace Floppy_Plane_WPF
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Player Player { get;}
        public FpsCounter Fps { get; }
        private List<Enemy> Enemies { get; }
        private AnimationController AnimationController { get; }
        public SkinSelection SkinSelectionPage { get; }
        // private Menu MenuController { get; }
        // private SoundPlayer SoundPlayer { get; }

        public MainWindow()
        {
            InitializeComponent();

            Player = new Player(Frame);
            Enemies = new List<Enemy>();
            AnimationController = new AnimationController(Player, Frame, GameUI, GameOverScreen, Enemies);
            SkinSelectionPage = new SkinSelection(this, Player.GraphicsController);
            Settings.AddSettingEventHandlers(AnimationController, Player, this);

            // Adds menu related event listeners  
            _ = new Menu(this);

            //SoundPlayer = new(Path.GetFullPath("Resources\\Sounds\\Engine.wav"));
            //SoundPlayer.LoadCompleted += (x, args) => { SoundPlayer.Play(); };
            //SoundPlayer.Load();

            Fps = new FpsCounter(FpsDisplay);

            Frame.Loaded += delegate { Player.Draw(); };
        }
        
        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space || AnimationController is { Started: false, CanReSpawn: false }) return;
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
