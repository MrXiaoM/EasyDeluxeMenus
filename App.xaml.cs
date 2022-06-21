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
        public static ImageSource ImageSlot { get; private set; }
        public static ImageSource ImageChest { get; private set; }
        public static ImageSource ImageEnchantment { get; private set; }
        public static FontFamily UniFont { get; private set; }
        public App()
        {
            UniFont = new FontFamily(new Uri("pack://application:,,,;/Font/unifont-14.0.03.otf"), "Unifont");
            Materials.Load();
            ImageSourceConverter converter = new ImageSourceConverter();
            ImageSlot = (ImageSource)converter.ConvertFrom(EasyDeluxeMenus.Properties.Resources.slot);
            ImageChest = (ImageSource)converter.ConvertFrom(EasyDeluxeMenus.Properties.Resources.chest);
            ImageEnchantment = (ImageSource)converter.ConvertFrom(EasyDeluxeMenus.Properties.Resources.enchantment);
        }
    }
}
