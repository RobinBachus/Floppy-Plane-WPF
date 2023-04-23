using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System;
using Floppy_Plane_WPF.GameObjects;

namespace Floppy_Plane_WPF.GUI
{
    internal class Settings
    {
        private AnimationController AnimationController { get; set; }
        private Player Player { get; set; }
        private MainWindow MainWindow { get; set; }

        public Settings(AnimationController animationController, Player player, MainWindow mainWindow)
        {
            AnimationController = animationController;
            Player = player;
            MainWindow = mainWindow;

            MainWindow.LevelTimeSlider.ValueChanged += LevelTimeSlider_ValueChanged;
            MainWindow.SpeedSlider.ValueChanged += SpeedSlider_ValueChanged;
            MainWindow.ShowHitbox.Click += ShowHitbox_Click;
        }

        private void LevelTimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.LevelTimeValue.Content = $"{Convert.ToInt32(e.NewValue)} sec";
            AnimationController.SetLevelUpTime(Convert.ToInt32(e.NewValue));
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MainWindow.SpeedValue.Content = $"{Convert.ToInt32(e.NewValue)}x";
            AnimationController.SpeedIncreaseValue = Convert.ToInt32(e.NewValue);
        }

        private void ShowHitbox_Click(object sender, RoutedEventArgs e)
        {
            AnimationController.ShowHitBoxes = MainWindow.ShowHitbox.IsChecked ?? false;
            Player.ShowHitboxes = MainWindow.ShowHitbox.IsChecked ?? false;
            Player.Draw();
        }
    }
}
