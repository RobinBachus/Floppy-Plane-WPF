using System;
using System.Windows;
using System.Windows.Controls;
using Floppy_Plane_WPF.Controllers;

namespace Floppy_Plane_WPF.GUI
{
    internal class Settings
    {
        public static void SetSettingEventHandlers(AnimationController animationController, Player player, MainWindow mainWindow)
        {
            Sliders sliders = new(animationController, mainWindow);
            Checkboxes checkboxes = new(animationController, player, mainWindow);

            mainWindow.LevelTimeSlider.ValueChanged += sliders.LevelTimeSlider_ValueChanged;
            mainWindow.SpeedSlider.ValueChanged += sliders.SpeedSlider_ValueChanged;

            mainWindow.AdvancedOptionsCheckbox.Click += checkboxes.AdvancedOptionsCheckbox_Click;
            mainWindow.SafeDistanceSlider.ValueChanged += sliders.SafeDistanceSlider_ValueChanged;
            mainWindow.GravityStrenghtSlider.ValueChanged += sliders.GravityStrengthSlider_ValueChanged;

            mainWindow.ShowHitboxCheckbox.Click += checkboxes.ShowHitboxCheckbox_Click;
            mainWindow.DisplayFpsCheckbox.Click += checkboxes.DisplayFpsCheckbox_Click;
        }

        private class Sliders
        {
            private AnimationController AnimationController { get; set; }
            private MainWindow Window { get; set; }

            public Sliders(AnimationController animationController, MainWindow mainWindow)
            {
                AnimationController = animationController;
                Window = mainWindow;
            }

            public void LevelTimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                Window.LevelTimeValue.Content = $"{Convert.ToInt32(e.NewValue)} sec";
                AnimationController.SetLevelUpTime(Convert.ToInt32(e.NewValue));
            }

            public void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                Window.SpeedValue.Content = $"{Convert.ToInt32(e.NewValue)}x";
                AnimationController.SpeedIncreaseValue = Convert.ToInt32(e.NewValue);
            }

            public void SafeDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                Window.SafeDistanceValue.Content = $"{Convert.ToInt32(e.NewValue)}px";
                AnimationController.SafeDistance = Convert.ToInt32(e.NewValue);
            }

            // TODO: GravityStrengthSlider implementation
            public void GravityStrengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                Window.GravityStrenghtValue.Content = $"{Convert.ToInt32(e.NewValue)}px";
            }
        }

        private class Checkboxes
        {
            private AnimationController AnimationController { get; set; }
            private Player Player { get; set; }
            private MainWindow Window { get; set; }

            public Checkboxes(AnimationController animationController, Player player, MainWindow mainWindow)
            {
                AnimationController = animationController;
                Player = player;
                Window = mainWindow;
            }

            public void AdvancedOptionsCheckbox_Click(object sender, RoutedEventArgs e)
            {
                ToggleVisibility(Window.AdvancedSettings, Window.AdvancedOptionsCheckbox.IsChecked);
            }

            public void ShowHitboxCheckbox_Click(object sender, RoutedEventArgs e)
            {
                AnimationController.ShowHitBoxes = Window.ShowHitboxCheckbox.IsChecked ?? false;
                Player.ShowHitboxes = Window.ShowHitboxCheckbox.IsChecked ?? false;
                Player.Draw();
            }

            public void DisplayFpsCheckbox_Click(object sender, RoutedEventArgs e)
            {
                ToggleVisibility(Window.FpsDisplay, Window.DisplayFpsCheckbox.IsChecked);
            }
        }


        private static void ToggleVisibility(UIElement element, bool? isChecked = false)
        {
            Visibility visibility = isChecked ?? false ? Visibility.Visible : Visibility.Collapsed;
            element.Visibility = visibility;
        }
    }
}
