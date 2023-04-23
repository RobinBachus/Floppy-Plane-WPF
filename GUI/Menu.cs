using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Floppy_Plane_WPF.GUI
{
    internal class Menu
    {
        private MainWindow MainWindow { get; set; }

        public Menu(MainWindow mainWindow)
        {
            MainWindow = mainWindow;

            MainWindow.SettingsButton.MouseDown += SettingsButton_Clicked;
            MainWindow.SettingsReturnButton.MouseDown += SettingsReturnButton_Clicked;
            MainWindow.GameOverMenuButton.Click += GameOverMenuButton_Click;
        }

        private void SetMenuVisibility(Visibility visibility)
        {
            if (visibility == Visibility.Visible)
            {
                MainWindow.GameUI.Visibility = Visibility.Collapsed;
            }
            MainWindow.Menu.Visibility = visibility;
            MainWindow.Frame.Visibility = visibility;
        }

        private void SettingsButton_Clicked(object sender, MouseEventArgs e)
        {
            SetMenuVisibility(Visibility.Collapsed);
            MainWindow.Settings.Visibility = Visibility.Visible;
        }

        private void SettingsReturnButton_Clicked(object sender, MouseEventArgs e)
        {
            SetMenuVisibility(Visibility.Visible);
            MainWindow.Settings.Visibility = Visibility.Collapsed;
            MainWindow.Frame.Focus();
        }

        private void GameOverMenuButton_Click(object sender, RoutedEventArgs e)
        {
            SetMenuVisibility(Visibility.Visible);
            MainWindow.GameOverScreen.Visibility = Visibility.Collapsed;
            MainWindow.Frame.Focus();
            MainWindow.Player.SetToStartingPosition();
        }
    }
}
