using System.Windows;
using System.Windows.Input;
using Floppy_Plane_WPF.AudioUtils;

namespace Floppy_Plane_WPF.GUI
{
    internal class Menu
    {
        private readonly MainWindow _mainWindow;
        private readonly MusicController _musicController;

        private Menu(MainWindow mainWindow, MusicController musicController)
        {
            _mainWindow = mainWindow;
            _musicController = musicController;

            _mainWindow.SettingsButton.MouseDown += SettingsButton_Clicked;
            _mainWindow.SettingsReturnButton.MouseDown += SettingsReturnButton_Clicked;
            _mainWindow.SkinSelectionButton.MouseDown += SkinSelectionButton_Click;
            _mainWindow.SkinReturnButton.MouseDown += SkinReturnButton_Click;
            _mainWindow.GameOverMenuButton.Click += GameOverMenuButton_Click;
        }

        /// <summary>
        /// Creates a discarded Menu object that adds the event listeners to the menu and Game Over buttons
        /// </summary>
        /// <param name="mainWindow">The start window of the game</param>
        /// <param name="musicController">Controller for background music</param>
        public static void AddMenuEventListeners(MainWindow mainWindow, MusicController musicController) => _ = new Menu(mainWindow, musicController);

        private void SetMenuVisibility(Visibility visibility)
        {
            if (visibility == Visibility.Visible)
            {
                _mainWindow.GameUi.Visibility = Visibility.Collapsed;
            }
            _mainWindow.Menu.Visibility = visibility;
            _mainWindow.Frame.Visibility = visibility;
        }

        private void SettingsButton_Clicked(object sender, MouseEventArgs e)
        {
            SetMenuVisibility(Visibility.Collapsed);
            _mainWindow.SettingsWindow.Visibility = Visibility.Visible;
        }

        private void SettingsReturnButton_Clicked(object sender, MouseEventArgs e)
        {
            SetMenuVisibility(Visibility.Visible);
            _mainWindow.SettingsWindow.Visibility = Visibility.Collapsed;
            _mainWindow.Frame.Focus();
        }

        private void GameOverMenuButton_Click(object sender, RoutedEventArgs e)
        {
            SetMenuVisibility(Visibility.Visible);
            _mainWindow.GameOverScreen.Visibility = Visibility.Collapsed;
            _mainWindow.Frame.Focus();
            _mainWindow.Player.SetToStartPosition();
            _musicController.Play();
        }

        private void SkinSelectionButton_Click(object sender, RoutedEventArgs e)
        {
            SetMenuVisibility(Visibility.Collapsed);
            _mainWindow.SkinSelectionPage.Show();
        }

        private void SkinReturnButton_Click(object sender, RoutedEventArgs e)
        {
            SetMenuVisibility(Visibility.Visible);
            _mainWindow.SkinSelectionPage.Hide();
            _mainWindow.Frame.Focus();
        }
    }
}
