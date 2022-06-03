using EasyDeluxeMenus.Minecraft;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EasyDeluxeMenus.DeluxeMenus
{
    public class MenuSlot : Grid
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public List<MemoryItem> Items = new List<MemoryItem>();
        public MenuSlot(int x, int y, ImageSource bg)
        {
            this.X = x;
            this.Y = y;
            this.Margin = new Thickness(x * 36, y * 36, 0, 0);
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Width = this.Height = 36;
            this.Background = new ImageBrush(bg);
        }

        public MemoryItem DecideItemToDisplay()
        {
            MemoryItem menu = null;
            int priority = 2147483647;
            foreach (MemoryItem m in Items)
            {
                if (m.Priority.HasValue)
                {
                    if (m.Priority.Value <= priority) menu = m;
                }
                else if (menu == null) menu = m;
            }
            return menu;
        }

        public void UpdateContent()
        {
            this.Children.Clear();
            MemoryItem menu = DecideItemToDisplay();
            this.Tag = menu;
            if (menu == null) return;
            this.Children.Add(menu.GenSlotItem());
            TextBlock amount = menu.GenSlotAmount();
            if (amount != null) this.Children.Add(amount);
        }
    }
    public abstract class MenuSlotsAbstract : Grid
    {
        public Dictionary<int, MenuSlot> Slots = new Dictionary<int, MenuSlot>();
        public abstract void UpdateSlotLayer();
        /// <summary>
        /// 根据位置获取索引
        /// </summary>
        /// <param name="x">横坐标，从0开始</param>
        /// <param name="y">纵坐标，从0开始</param>
        /// <returns></returns>
        public abstract int GetSlotIndexByLocation(int x, int y);
        public virtual void UpdateItems(MemoryMenu menu)
        {

        }
        public MenuSlot GetSlotByLocation(int x, int y)
        {
            return Slots[GetSlotIndexByLocation(x, y)];
        }
    }
    public class MenuSlotsChest : MenuSlotsAbstract
    {
        TextBlock TextTitle { get; } = new TextBlock()
        {
            Margin = new Thickness(14, 10, 0, 0),
            Foreground = Brushes.Black,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            FontFamily = App.UniFont
        };
        public int Row;
        public MenuSlotsChest(int row)
        {
            this.Row = row;
        }
        public override void UpdateItems(MemoryMenu menu)
        {
            base.UpdateItems(menu);
            FormatCode.GenTextBlock(TextTitle, menu.Title);
        }
        public override void UpdateSlotLayer()
        {
            ImageSource bg = (ImageSource)new ImageSourceConverter().ConvertFrom(Properties.Resources.slot);
            int i = 0;
            for (int y = 0; y < Row; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    Slots.Add(i, new MenuSlot(x, y, bg));
                    i++;
                }
            }
            this.Height = Row * 36;
        }
        public override int GetSlotIndexByLocation(int x, int y)
        {
            return (y * 9) + x;
        }
    }
}
