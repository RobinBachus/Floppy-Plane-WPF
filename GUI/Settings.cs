using System;
using System.Windows;
using Floppy_Plane_WPF.Controllers;
using Floppy_Plane_WPF.GameObjects;

namespace Floppy_Plane_WPF.GUI
{
    internal class Settings
    {
        public static bool ShowHitBoxes { get; set; }

        public static void AddSettingEventHandlers(MainWindow mainWindow, Player player)
        {
            Sliders sliders = new(mainWindow);
            CheckBoxes checkboxes = new(player, mainWindow);

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
            private MainWindow Window { get; }

            public Sliders(MainWindow mainWindow)
            {
                Window = mainWindow;
            }

            public void LevelTimeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                Window.LevelTimeValue.Content = $"{Convert.ToInt32(e.NewValue)} sec";
                AnimationController.LevelUpTime = Convert.ToInt32(e.NewValue);
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
            private Player Player { get; }
            private MainWindow Window { get; }

            public CheckBoxes(Player player, MainWindow mainWindow)
            {
                Player = player;
                Window = mainWindow;
            }

            public void AdvancedOptionsCheckBox_Click(object sender, RoutedEventArgs e)
            {
                ToggleVisibility(Window.AdvancedSettings, Window.AdvancedOptionsCheckbox.IsChecked);
            }

            public void ShowHitBoxCheckBox_Click(object sender, RoutedEventArgs e)
            {
                ShowHitBoxes = Window.ShowHitboxCheckbox.IsChecked ?? false;
            }

            public void DisplayFpsCheckBox_Click(object sender, RoutedEventArgs e)
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
