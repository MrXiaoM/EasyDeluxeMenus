using EasyDeluxeMenus.DeluxeMenus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EasyDeluxeMenus.Minecraft
{
    public class ItemStack : Grid
    {
        int x, y;
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                this.Margin = new System.Windows.Thickness(value * 36, y * 36, 0, 0);
                x = value;
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                this.Margin = new System.Windows.Thickness(x * 36, value * 36, 0, 0);
                y = value;
            }
        }
        Rectangle cover;
        Image materialImage;
        TextBlock amountTextBox;
        MemoryItem item;
        public ItemStack(MemoryItem item)
        {
            this.item = item;
            this.Background = new ImageBrush((ImageSource) new ImageSourceConverter().ConvertFrom(Properties.Resources.slot));
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.Width = this.Height = 36;
            this.Children.Add(this.materialImage = new Image()
            {
                Width = 32,
                Height = 32,
                Source = item != null ? item.TrueMaterial.Image : null
            });
            this.Children.Add(this.amountTextBox = new TextBlock()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                FontFamily = App.UniFont,
                Text = item != null && item.Amount.HasValue ? item.Amount.ToString() : ""
            });
            this.Children.Add(this.cover = new Rectangle()
            {
                Fill = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255)),
                Width = 34,
                Height = 34,
                Opacity = 0
            });
            this.MouseEnter += delegate { this.cover.Opacity = 1; if(item == null) MainWindow.INSTANCE.Tips = ""; else MainWindow.INSTANCE.TipsList = item.Tooltips; };
            this.MouseLeave += delegate { this.cover.Opacity = 0; MainWindow.INSTANCE.Tips = ""; };
        }
    }
    public abstract class Inventory : Grid
    {
        protected Dictionary<int, ItemStack> items = new Dictionary<int, ItemStack>();
        protected Grid itemsGrid;
        protected TextBlock TextTitle;
        public string Title
        {
            set
            {
                TextTitle.Text = value;
            }
        }
        public Inventory()
        {

        }

        public abstract void Update(MemoryMenu menu);

        public virtual ItemStack GetItem(int slot)
        {
            if (!items.ContainsKey(slot)) return null;
            return items[slot];
        }

        public virtual void SetItem(int slot, ItemStack item)
        {
            if (items.ContainsKey(slot)) items[slot] = item;
            else items.Add(slot, item);
        }
        
        public virtual void RefreshItems()
        {
            itemsGrid.Children.Clear();
            foreach (ItemStack item in items.Values)
                itemsGrid.Children.Add(item);
        }
    }

    public class InventoryChest : Inventory
    {
        int row;
        public InventoryChest(int row)
        {
            this.row = row;
            this.Width = 352;
            this.Height = 264;
            this.Background = new ImageBrush((ImageSource)new ImageSourceConverter().ConvertFrom(Properties.Resources.chest));
            this.Children.Add(this.TextTitle = new TextBlock()
            {
                Margin = new System.Windows.Thickness(14, 14, 0, 0),
                FontFamily = App.UniFont
            });

            this.Children.Add(this.itemsGrid = new Grid() 
            {
                Margin = new System.Windows.Thickness(14, 34, 14, 0)
            });
            RefreshItems();
        }

        public override void Update(MemoryMenu menu)
        {
            this.row = menu.InventorySize.GetValueOrDefault(9) / 9;
            FormatCode.GenTextBlock(this.TextTitle, menu.Title);
            this.items.Clear();
            foreach (MemoryItem item in menu.PreviewItems)
            {
                foreach (int i in item.SlotsInt)
                {
                    Console.WriteLine("更新 " + i + " " + item.DisplayName);
                    SetItem(i, new ItemStack(item));
                }
            }
            RefreshItems();
        }

        public override void SetItem(int slot, ItemStack item)
        {
            int y = (int) Math.Floor(slot / 9.0D);
            int x = slot % 9;
            item.X = x;
            item.Y = y;
            base.SetItem(slot, item);
        }

        public override void RefreshItems()
        {
            for (int i = 0; i < row * 9; i++) {
                if (!items.ContainsKey(i) || items[i] == null)
                {
                    SetItem(i, new ItemStack(null));
                }
            }
            foreach (int i in items.Keys)
            {
                if (i >= row * 9 || i < 0)
                {
                    items.Remove(i);
                }
            }
            base.RefreshItems();
        }
    }
}
