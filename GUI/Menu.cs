using System.Windows;
using System.Windows.Input;

namespace Floppy_Plane_WPF.GUI
{
    internal class Menu
    {
        private MainWindow MainWindow { get; }

        public Menu(MainWindow mainWindow)
        {
            MainWindow = mainWindow;

            MainWindow.SettingsButton.MouseDown += SettingsButton_Clicked;
            MainWindow.SettingsReturnButton.MouseDown += SettingsReturnButton_Clicked;
            MainWindow.SkinSelectionButton.MouseDown += SkinSelectionButton_Click;
            MainWindow.SkinReturnButton.MouseDown += SkinReturnButton_Click;
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
            MainWindow.SettingsWindow.Visibility = Visibility.Visible;
        }

        private void SettingsReturnButton_Clicked(object sender, MouseEventArgs e)
        {
            SetMenuVisibility(Visibility.Visible);
            MainWindow.SettingsWindow.Visibility = Visibility.Collapsed;
            MainWindow.Frame.Focus();
        }

        private void GameOverMenuButton_Click(object sender, RoutedEventArgs e)
        {
            SetMenuVisibility(Visibility.Visible);
            MainWindow.GameOverScreen.Visibility = Visibility.Collapsed;
            MainWindow.Frame.Focus();
            MainWindow.Player.SetToStartPosition();
        }

        private void SkinSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            SetMenuVisibility(Visibility.Collapsed);
            MainWindow.SkinSelectionPage.Show();
        }

        private void SkinReturnButton_Click(object sender, RoutedEventArgs e)
        {
            SetMenuVisibility(Visibility.Visible);
            MainWindow.SkinSelectionPage.Hide();
            MainWindow.Frame.Focus();
        }
    }
}
