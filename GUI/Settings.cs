using System;
using System.Windows;
using Floppy_Plane_WPF.Controllers;
using Floppy_Plane_WPF.GameObjects;

namespace Floppy_Plane_WPF.GUI
{
    internal class Settings
    {
        public static void AddSettingEventHandlers(AnimationController animationController, Player player, MainWindow mainWindow)
        {
            Sliders sliders = new(animationController, mainWindow, player);
            CheckBoxes checkboxes = new(animationController, player, mainWindow);

            mainWindow.LevelTimeSlider.ValueChanged += sliders.LevelTimeSlider_ValueChanged;
            mainWindow.SpeedSlider.ValueChanged += sliders.SpeedSlider_ValueChanged;

            mainWindow.AdvancedOptionsCheckbox.Click += checkboxes.AdvancedOptionsCheckBox_Click;
            mainWindow.SafeDistanceSlider.ValueChanged += sliders.SafeDistanceSlider_ValueChanged;
            mainWindow.GravityStrenghtSlider.ValueChanged += sliders.GravityStrengthSlider_ValueChanged;

            mainWindow.ShowHitboxCheckbox.Click += checkboxes.ShowHitBoxCheckBox_Click;
            mainWindow.DisplayFpsCheckbox.Click += checkboxes.DisplayFpsCheckBox_Click;
        }

        private class Sliders
        {
            private AnimationController AnimationController { get; }
            private MainWindow Window { get; }
            private Player Player { get; }

            public Sliders(AnimationController animationController, MainWindow mainWindow, Player player)
            {
                AnimationController = animationController;
                Window = mainWindow;
                Player = player;
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
                double value = Math.Round(e.NewValue, 1);
                Window.GravityStrenghtValue.Content = $"{value}g";
                Player.GravityMultiplier = value;
            }
        }

        private class CheckBoxes
        {
            private AnimationController AnimationController { get; set; }
            private Player Player { get; set; }
            private MainWindow Window { get; set; }

            public CheckBoxes(AnimationController animationController, Player player, MainWindow mainWindow)
            {
                AnimationController = animationController;
                Player = player;
                Window = mainWindow;
            }

            public void AdvancedOptionsCheckBox_Click(object sender, RoutedEventArgs e)
            {
                ToggleVisibility(Window.AdvancedSettings, Window.AdvancedOptionsCheckbox.IsChecked);
            }

            public void ShowHitBoxCheckBox_Click(object sender, RoutedEventArgs e)
            {
                AnimationController.ShowHitBoxes = Window.ShowHitboxCheckbox.IsChecked ?? false;
                Player.ShowHitBoxes = Window.ShowHitboxCheckbox.IsChecked ?? false;
                Player.Draw();
            }

            public void DisplayFpsCheckBox_Click(object sender, RoutedEventArgs e)
            {
                ToggleVisibility(Window.FpsDisplay, Window.DisplayFpsCheckbox.IsChecked);
            }
        }


        private static void ToggleVisibility(UIElement element, bool? isChecked = false)
        {
            var visibility = isChecked ?? false ? Visibility.Visible : Visibility.Collapsed;
            element.Visibility = visibility;
        }
    }
}
