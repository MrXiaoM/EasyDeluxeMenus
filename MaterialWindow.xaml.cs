﻿using EasyDeluxeMenus.Minecraft;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EasyDeluxeMenus
{
    public class MaterialItem : Border
    {
        MaterialWindow mw;
        Image image;
        int x;
        int y;
        public Material Material { get; private set; }
        public MaterialItem(string material, ImageSource bg, int x, int y, MaterialWindow mw)
        {
            this.x = x;
            this.y = y;
            Material = Materials.GetMaterial(material);
            this.mw = mw;
            image = new Image()
            {
                Source = Materials.materials[material],
                Width = 32,
                Height = 32
            };
            Width = Height = 36;
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Margin = new Thickness(x, y, 0, 0);
            Background = new ImageBrush(bg);
            Child = image;
            MouseDown += delegate { mw.OnMaterialClick(this); };
            MouseEnter += delegate { mw.OnMaterialMouseEnter(this); };
            MouseLeave += delegate { mw.OnMaterialMouseLeave(this); };
        }
        public void SetLocation(int x, int y)
        {
            Margin = new Thickness(x, y, 0, 0);
        }
        public void ResetLocation()
        {
            SetLocation(x, y);
        }
    }
    /// <summary>
    /// MaterialWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MaterialWindow : Window
    {
        public delegate void MaterialClickEventHandler(MaterialItem item);
        public event MaterialClickEventHandler MaterialClickEvent;
        private readonly HovingBoxWindow tips = new HovingBoxWindow();
        public MaterialWindow()
        {
            InitializeComponent();
            tips.SetPixelFont();
            GridTopmost.Children.Add(tips);
        }
        internal void OnMaterialMouseEnter(MaterialItem item)
        {
            tips.UpdateTips(item.Material.TranslateName);
        }
        internal void OnMaterialMouseLeave(MaterialItem item)
        {
            tips.UpdateTips("");
        }
        internal void OnMaterialClick(MaterialItem item)
        {
            MaterialClickEvent(item);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ImageSource bg = App.ImageSlot;
            int x = 0;
            int y = 0;
            MaterialBox.Children.Clear();
            foreach (string m in Materials.materials.Keys)
            {
                MaterialItem mi = new MaterialItem(m, bg, x * 36, y * 36, this);
                MaterialBox.Children.Add(mi);
                x++;
                if (x > 8)
                {
                    x = 0;
                    y++;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string content = SearchBox.Text.ToUpper();
            int x = 0;
            int y = 0;
            foreach (UIElement ui in MaterialBox.Children)
            {
                if (!(ui is MaterialItem))
                {
                    continue;
                }

                MaterialItem item = (MaterialItem)ui;
                if (content == string.Empty)
                {
                    item.ResetLocation();
                    item.Visibility = Visibility.Visible;
                }
                else if (item.Material.TranslateName.ToUpper().Contains(content) || item.Material.Name.Contains(content.Replace(" ", "_")))
                {
                    item.SetLocation(x * 36, y * 36);
                    item.Visibility = Visibility.Visible;
                    x++;
                    if (x > 8)
                    {
                        x = 0;
                        y++;
                    }
                }
                else
                {
                    item.Visibility = Visibility.Hidden;
                }
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tips.Close();
        }
    }
}
