using Floppy_Plane_WPF.Controllers;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Floppy_Plane_WPF.GUI
{
    public class SkinSelection
    {
        private readonly MainWindow _mainWindow;
        private readonly PlayerGraphicsController _graphicsController;

        private ImageBrush SkinPreview { get; }

        public SkinSelection(MainWindow mainWindow, PlayerGraphicsController graphicsController)
        {
            _mainWindow = mainWindow;
            _graphicsController = graphicsController;

            SkinPreview = _mainWindow.SkinPreview;
        }

        public void Show()
        {
            UpdateVisual();

            _mainWindow.SkinNextButton.MouseUp += SkinNextButton_Click;
            _mainWindow.SkinPrevButton.MouseUp += SkinPrevButton_Click;

            _mainWindow.KeyUp += SkinMenu_KeyUp;

            _mainWindow.SkinMenu.Visibility = Visibility.Visible;
            _mainWindow.SkinMenu.Focus();
        }

        public void Hide()
        {
            _mainWindow.SkinNextButton.MouseUp -= SkinNextButton_Click;
            _mainWindow.SkinPrevButton.MouseUp -= SkinPrevButton_Click;
            _mainWindow.KeyUp -= SkinMenu_KeyUp;

            _mainWindow.SkinMenu.Visibility = Visibility.Collapsed;
            _mainWindow.Focus();
        }

        private void UpdateVisual()
        {
            VisualBrush? brush = (VisualBrush)_graphicsController.Sprite.Fill;
            SkinPreview.ImageSource = ((Image)brush.Visual).Source;
            _mainWindow.SkinNameLabel.Content = _graphicsController.CurrentSkinName;
        }

        private void SkinNextButton_Click(object sender, RoutedEventArgs e)
        {
            _graphicsController.NextSprite();
            UpdateVisual();
        }

        private void SkinPrevButton_Click(object sender, RoutedEventArgs e)
        {
            _graphicsController.PrevSprite();
            UpdateVisual();
        }

        private void SkinMenu_KeyUp(object sender, KeyEventArgs e)
        {
            List<Key> leftKeys = new() { Key.Left, Key.A, Key.Q};
            List<Key> rightKeys = new() { Key.Right, Key.D, Key.E };

            if (leftKeys.Contains(e.Key))
            {
                _graphicsController.PrevSprite();
                UpdateVisual();
            }
            else if (rightKeys.Contains(e.Key))
            {
                _graphicsController.NextSprite();
                UpdateVisual();
            }
            e.Handled = true;
        }
    }
}
