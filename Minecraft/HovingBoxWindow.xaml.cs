using System.Collections.Generic;
using System.Threading;
using System.Windows;
using static EasyDeluxeMenus.Win32;

namespace EasyDeluxeMenus.Minecraft
{
    /// <summary>
    /// Minecraft 风格的悬浮提示窗口
    /// </summary>
    public partial class HovingBoxWindow : Window
    {
        Thread thread;
        bool running = true;
        public HovingBoxWindow()
        {
            InitializeComponent();
            MainText.FontFamily = App.UniFont;
            Opacity = 0;
            SizeToContent = SizeToContent.WidthAndHeight;
            thread = new Thread(new ThreadStart(() =>
            {
                while (running)
                {
                    Dispatcher.Invoke(Tick);
                    Thread.Sleep(10);
                }
            }));
            thread.Start();
            Show();
        }

        private void Tick()
        {
            if (Opacity > 0)
            {
                SetTopMost(this);
                POINT point = MousePos;
                Dpi dpi = GetDpiByWin32();
                Thickness margin = MainText.Margin;
                Left = (point.X + 36) / (dpi.DpiX / 96);
                Top = point.Y / (dpi.DpiY / 96);
            }
        }

        public void UpdateTips(string text)
        {
            if (text == null || text.Length == 0)
            {
                Opacity = 0;
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
                Opacity = 0;
            }
            else
            {
                FormatCode.GenTextBlock(MainText, texts);
                Opacity = 1;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            running = false;
            try
            {
                thread.Abort();
            }
            catch { }
        }
    }
}
