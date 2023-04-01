using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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

        public MainWindow()
        {
            InitializeComponent();

            Enemies = new List<Enemy>();
            Frame.Loaded += Canvas_OnLoad;
            SettingsButton.MouseDown += SettingsButton_Clicked;
            SettingsReturnButton.MouseDown += SettingsReturnButton_Clicked;
            LevelTimeSlider.ValueChanged += LevelTimeSlider_ValueChanged;
            SpeedSlider.ValueChanged += SpeedSlider_ValueChanged;
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

        private void Canvas_OnLoad(object sender, RoutedEventArgs routedEvent)
        {
            Frame.Focus();
            Player = new(Frame);
            
            AnimationController = new(Player, Frame, GameUI, GameOverScreen, Enemies);
        }
        
        private void SettingsButton_Clicked(object sender, MouseEventArgs e) 
        {
            SetMenuVisibility(Visibility.Collapsed);
            Settings.Visibility = Visibility.Visible;
        }
        private void SettingsReturnButton_Clicked(object sender, MouseEventArgs e)
        {
            SetMenuVisibility(Visibility.Visible);
            Settings.Visibility = Visibility.Collapsed;
            Frame.Focus();
        }
        private void LevelTimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LevelTimeValue.Content = $"{Convert.ToInt32(e.NewValue)} sec";
            AnimationController.SetLevelUpTime(Convert.ToInt32(e.NewValue));
        }
        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SpeedValue.Content = $"{Convert.ToInt32(e.NewValue)}x";
            AnimationController.SpeedIncreaseValue = Convert.ToInt32(e.NewValue);
        }
        private void GameOverMenuButton_Click(object sender, RoutedEventArgs e)
        {
            SetMenuVisibility(Visibility.Visible);
            GameOverScreen.Visibility = Visibility.Collapsed;
            Frame.Focus();
            Player.SetToStartingPosition();
        }

        private void SetMenuVisibility(Visibility visibility)
        {
            if (visibility == Visibility.Visible)
            {
                GameUI.Visibility = Visibility.Collapsed;
            }
            Menu.Visibility = visibility;
            Frame.Visibility = visibility;
        }

        private void ShowHitbox_Click(object sender, RoutedEventArgs e)
        {
            AnimationController.ShowHitBoxes = ShowHitbox.IsChecked ?? false;
            Player.ShowHitboxes = ShowHitbox.IsChecked ?? false;
            Player.Draw();
        }
    }
}
