using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Pharos.POS.Retailing.Styles.Base
{
    public partial class ChildPageFrame : ResourceDictionary
    {

        /// <summary>
        /// Handles the MouseLeftButtonDown event. This event handler is used here to facilitate
        /// dragging of the Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            window.DragMove();
        }

        /// <summary>
        /// Fires when the user clicks the Close button on the window's custom title bar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;
            window.Close();
        }


        /// <summary>
        /// Called when the user drags the title bar when maximized.
        /// </summary>
        private void OnBorderMouseMove(object sender, MouseEventArgs e)
        {
            var window = (Window)((FrameworkElement)sender).TemplatedParent;

            if (window != null)
            {
                if (e.LeftButton == MouseButtonState.Pressed && window.WindowState == WindowState.Maximized)
                {
                    Size maxSize = new Size(window.ActualWidth, window.ActualHeight);
                    Size resSize = window.RestoreBounds.Size;

                    double curX = e.GetPosition(window).X;
                    double curY = e.GetPosition(window).Y;

                    double newX = curX / maxSize.Width * resSize.Width;
                    double newY = curY;

                    window.WindowState = WindowState.Normal;

                    window.Left = curX - newX;
                    window.Top = curY - newY;
                    window.DragMove();
                }
            }
        }
    }
}