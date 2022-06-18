using EasyDeluxeMenus.DeluxeMenus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

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
                Source = item.TrueMaterial.Image
            });
            this.Children.Add(this.amountTextBox = new TextBlock()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                FontFamily = App.UniFont,
                Text = item.Amount.HasValue ? item.Amount.ToString() : ""
            });
        }
    }
    public abstract class Inventory : Grid
    {
        Dictionary<int, ItemStack> items = new Dictionary<int, ItemStack>();
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

        public virtual ItemStack GetItem(int slot)
        {
            if (!items.ContainsKey(slot)) return null;
            return items[slot];
        }

        public virtual void SetItem(int slot, ItemStack item)
        {
            if (items.ContainsKey(slot)) items[slot] = item;
            items.Add(slot, item);
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
        public InventoryChest()
        {
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
        }

        public override void SetItem(int slot, ItemStack item)
        {
            int y = (int) Math.Floor(slot / 9.0D);
            int x = slot % 9;
            item.X = x;
            item.Y = y;
            base.SetItem(slot, item);
        }
    }
}
