using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using static EasyDeluxeMenus.Win32;

namespace EasyDeluxeMenus.Minecraft
{
    /// <summary>
    /// Minecraft 风格的悬浮提示窗口
    /// </summary>
    public partial class HovingBoxWindow : Grid
    {
        Thread thread;
        bool running = true;
        public HovingBoxWindow()
        {
            InitializeComponent();

            thread = new Thread(new ThreadStart(() =>
            {
                while (running)
                {
                    Dispatcher.Invoke(Tick);
                    Thread.Sleep(10);
                }
            }));
            
            thread.Start();
        }
        public void SetPixelFont()
        {
            MainText.FontFamily = App.UniFont;
        }

        private void Tick()
        {
            if (HoverBox.Visibility == Visibility.Visible)
            {
                UpdateLocation();
            }
        }

        public void UpdateLocation()
        {
            Point point = Mouse.GetPosition(this);
            double width = ActualWidth;
            double height = ActualHeight;
            double x = point.X + 16;
            double y = point.Y;
            double w = HoverBox.ActualWidth;
            double h = HoverBox.ActualHeight;
            if (x + w + 10 > width) x = width - w - 10;
            if (y + h + 10 > height) y = height - h - 10;

            //MainWindow.INSTANCE.Title = x + "," + y + ";" + w + "," + h + ";" + width + "," + height;
            
            HoverBox.Margin = new Thickness(x, y, 1, 1);
        }

        public void UpdateTips(string text)
        {
            if (text == null || text.Length == 0)
            {
                HoverBox.Visibility = Visibility.Hidden;
            }
            else
            {
                UpdateTips(new List<string>(text.Contains("\n") ? text.Split('\n') : new string[] { text }));
            }
        }

        public void UpdateTips(List<string> texts)
        {
            if (texts == null || texts.Count == 0)
            {
                HoverBox.Visibility = Visibility.Hidden;
            }
            else
            {
                FormatCode.GenTextBlock(MainText, texts);
                UpdateLocation();
                HoverBox.Visibility = Visibility.Visible;
            }
        }

        public void Close()
        {
            this.Visibility = Visibility.Hidden;
            running = false;
            try
            {
                thread.Abort();
            }
            catch { }
        }
    }
}
