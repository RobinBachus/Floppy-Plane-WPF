using Floppy_Plane_WPF.Controllers;
using System.Windows;
using System.Windows.Shapes;

namespace Floppy_Plane_WPF.GUI
{
    internal class SkinSelection
    {
        private readonly MainWindow _mainWindow;
        private readonly PlayerGraphicsController _graphicsController;

        private Rectangle Frame { get; }

        public SkinSelection(MainWindow mainWindow, PlayerGraphicsController graphicsController)
        {
            _mainWindow = mainWindow;
            _graphicsController = graphicsController;

            Frame = _mainWindow.SkinPreview;
        }

        public void Show()
        {
            Frame.Fill = _graphicsController.Sprite.Fill;

            _mainWindow.SkinNextButton.MouseUp += SkinNextButton_Click;
            _mainWindow.SkinPrevButton.MouseUp += SkinPrevButton_Click;
        }

        public void Hide()
        {
            //TODO
        }

        private void SkinNextButton_Click(object sender, RoutedEventArgs e)
        {
            _graphicsController.NextSprite();
            Frame.Fill = _graphicsController.Sprite.Fill;
        }

        private void SkinPrevButton_Click(object sender, RoutedEventArgs e)
        {
            _graphicsController.PrevSprite();
            Frame.Fill = _graphicsController.Sprite.Fill;
        }
    }
}
