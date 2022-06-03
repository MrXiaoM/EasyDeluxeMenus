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
            this.MainText.FontFamily = App.UniFont;
            this.Opacity = 0;
            this.SizeToContent = SizeToContent.WidthAndHeight;
            thread = new Thread(new ThreadStart(() =>
            {
                while (running)
                {
                    this.Dispatcher.Invoke(Tick);
                    Thread.Sleep(10);
                }
            }));
            thread.Start();
            this.Show();
        }

        private void Tick()
        {
            if (this.Opacity > 0)
            {
                SetTopMost(this);
                POINT point = MousePos;
                Dpi dpi = GetDpiByWin32();
                Thickness margin = MainText.Margin;
                this.Left = (point.X + 36) / (dpi.DpiX / 96);
                this.Top = point.Y / (dpi.DpiY / 96);
            }
        }

        public void UpdateTips(string text)
        {
            UpdateTips(new List<string>(text.Contains("\n") ? text.Split('\n') : new string[] { text }));
        }

        public void UpdateTips(List<string> texts)
        {
            FormatCode.GenTextBlock(MainText, texts);
            this.Opacity = 1;
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
