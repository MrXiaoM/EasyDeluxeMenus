using EasyDeluxeMenus.Minecraft;
using System;
using System.Windows;
using System.Windows.Media;

namespace EasyDeluxeMenus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static FontFamily UniFont { get; private set; }
        public App()
        {
            UniFont = new FontFamily(new Uri("pack://application:,,,;/Font/unifont-14.0.03.otf"), "Unifont");
            Materials.Load();
        }
    }
}
