using System;
using System.Windows;
using Floppy_Plane_WPF.Controllers;

namespace Floppy_Plane_WPF.GUI
{
    internal class Settings
    {
        public static void SetSettingEventHandlers(AnimationController animationController, Player player, MainWindow mainWindow)
        {
            Sliders sliders = new(animationController, player, mainWindow);
            Checkboxes checkboxes = new(animationController, player, mainWindow);

            mainWindow.LevelTimeSlider.ValueChanged += sliders.LevelTimeSlider_ValueChanged;
            mainWindow.SpeedSlider.ValueChanged += sliders.SpeedSlider_ValueChanged;
            mainWindow.SafeDistanceSlider.ValueChanged += sliders.SafeDistanceSlider_ValueChanged;
            mainWindow.GravityStrenghtSlider.ValueChanged += sliders.SafeDistanceSlider_ValueChanged;

            mainWindow.ShowHitbox.Click += checkboxes.ShowHitbox_Click;
            mainWindow.AdvancedOptionsHitbox.Click += checkboxes.AdvancedOptionsHitbox_Click;
        }

        private class Sliders
        {
            private AnimationController AnimationController { get; set; }
            Player Player { get; set; }
            private MainWindow MainWindow { get; set; }

            public Sliders(AnimationController animationController, Player player, MainWindow mainWindow)
            {
                AnimationController = animationController;
                Player = player;
                MainWindow = mainWindow;
            }

            public void LevelTimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                MainWindow.LevelTimeValue.Content = $"{Convert.ToInt32(e.NewValue)} sec";
                AnimationController.SetLevelUpTime(Convert.ToInt32(e.NewValue));
            }

            public void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                MainWindow.SpeedValue.Content = $"{Convert.ToInt32(e.NewValue)}x";
                AnimationController.SpeedIncreaseValue = Convert.ToInt32(e.NewValue);
            }

            public void SafeDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                MainWindow.SafeDistanceValue.Content = $"{Convert.ToInt32(e.NewValue)}px";
                AnimationController.SafeDistance = Convert.ToInt32(e.NewValue);
            }

            public void GravetyStrengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                MainWindow.GravityStrenghtValue.Content = $"{Convert.ToInt32(e.NewValue)}px";
                AnimationController.SafeDistance = Convert.ToInt32(e.NewValue);
            }
        }

        private class Checkboxes
        {
            private AnimationController AnimationController { get; set; }
            private Player Player { get; set; }
            private MainWindow MainWindow { get; set; }

            public Checkboxes(AnimationController animationController, Player player, MainWindow mainWindow)
            {
                AnimationController = animationController;
                Player = player;
                MainWindow = mainWindow;
            }

            public void ShowHitbox_Click(object sender, RoutedEventArgs e)
            {
                AnimationController.ShowHitBoxes = MainWindow.ShowHitbox.IsChecked ?? false;
                Player.ShowHitboxes = MainWindow.ShowHitbox.IsChecked ?? false;
                Player.Draw();
            }

            public void ShowFpsCounter_Click(object sender, RoutedEventArgs e)
            {
                bool _checked = MainWindow.AdvancedOptionsHitbox.IsChecked ?? false;
                Visibility visibility = _checked ? Visibility.Visible : Visibility.Collapsed;
                MainWindow.FpsDisplay.Visibility = visibility;
            }

            public void AdvancedOptionsHitbox_Click(object sender, RoutedEventArgs e)
            {
                bool _checked = MainWindow.AdvancedOptionsHitbox.IsChecked ?? false;
                Visibility visibility = _checked ? Visibility.Visible : Visibility.Collapsed;
                MainWindow.AdvancedSettings.Visibility = visibility;
            }
        }
    }
}
