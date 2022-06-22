using EasyDeluxeMenus.DeluxeMenus;
using EasyDeluxeMenus.Minecraft;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EasyDeluxeMenus
{
    #region 附魔和药水效果 ComboBox
    public class ComboEnchantmentBox : ComboBox
    {
        public static List<KeyValuePair<string, string>> Enchantments = GetEnchatments();
        public static List<KeyValuePair<string, string>> GetEnchatments()
        {
            List<KeyValuePair<string, string>> enchs = new List<KeyValuePair<string, string>>();
            enchs.Add(KeyValuePair.Create("DAMAGE_ALL", "锋利"));

            return enchs;
        }
        public ComboEnchantmentBox()
        {

            for (int i = 0; i < Enchantments.Count; i++)
            {
                KeyValuePair<string, string> e = Enchantments[i];
                Items.Add(new ComboBoxItem()
                {
                    Content = e.Value + "(" + e.Key + ")",
                    Tag = e.Key
                });
            }
        }
        public string GetSelectedEnchantment()
        {
            if (SelectedIndex > -1)
            {
                return Enchantments[SelectedIndex].Key;
            }
            return "";
        }
    }
    public class ListViewItemEnchantment
    {
        public int EnchantmentIndex
        {
            get; set;
        }
        public string Enchantment { get; set; }
        public int Level { get; set; }
    }
    public class ComboPotionEffectBox : ComboBox
    {
        public static List<KeyValuePair<string, string>> Effects = GetEffects();
        public static List<KeyValuePair<string, string>> GetEffects()
        {
            List<KeyValuePair<string, string>> enchs = new List<KeyValuePair<string, string>>();
            enchs.Add(KeyValuePair.Create("SPEED", "速度"));

            return enchs;
        }
        public ComboPotionEffectBox()
        {

            for (int i = 0; i < Effects.Count; i++)
            {
                KeyValuePair<string, string> e = Effects[i];
                Items.Add(new ComboBoxItem()
                {
                    Content = e.Value + "(" + e.Key + ")",
                    Tag = e.Key
                });
            }
        }
        public string GetSelectedEffect()
        {
            if (SelectedIndex > -1)
            {
                return Effects[SelectedIndex].Key;
            }
            return "";
        }
    }
    public class ListViewItemPotionEffect
    {
        public int EffectIndex
        {
            get; set;
        }
        public string Effect { get; set; }
        public int Level { get; set; }
    }
    #endregion

    #region 物品列表中的物品
    public class SimpleItem : Grid
    {
        public static readonly SolidColorBrush COLOR_DEFAULT = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
        public static readonly SolidColorBrush COLOR_MOUSEOVER = new SolidColorBrush(Color.FromArgb(90, 0, 0, 0));
        public static readonly SolidColorBrush COLOR_SELECTED = new SolidColorBrush(Color.FromArgb(180, 86, 157, 229));
        Image image = new Image()
        {
            Margin = new Thickness(6),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Width = 20,
            Height = 20,
        };
        TextBlock text = new TextBlock()
        {
            Margin = new Thickness(32, 2, 2, 2),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = new SolidColorBrush(Colors.White)
        };
        Border border;
        private string material;

        public SimpleItem(MainWindow mainWindow, string Id, string Text, string Material)
        {
            this.mainWindow = mainWindow;
            this.Id = Id;
            this.Text = Text;
            this.Material = Material;
            Margin = new Thickness(0, 2, 4, 2);
            Background = Id == mainWindow.ItemSelectedId ? COLOR_MOUSEOVER : COLOR_DEFAULT;
            Height = 36;
            
            Children.Add(border = new Border()
            {
                BorderBrush = COLOR_SELECTED,
                BorderThickness = new Thickness(Id != mainWindow.ItemSelectedId ? 0 : 1)
            });
            Children.Add(image);
            Children.Add(text);
            MouseDown += delegate
            {
                mainWindow.ItemSelectedId = Id;
                foreach (SimpleItem i in mainWindow.ItemsList.Children)
                {
                    i.border.BorderThickness = new Thickness(i.Id == this.Id ? 1 : 0);
                    i.Background = i.Id == this.Id ? COLOR_MOUSEOVER : COLOR_DEFAULT;
                }
            };
            MouseEnter += delegate
            {
                Background = COLOR_MOUSEOVER;
                MemoryItem item = mainWindow.Menu.Items[Id];
                mainWindow.TipsList = item.Tooltips;
            };
            MouseLeave += delegate
            {
                if (mainWindow.ItemSelectedId != Id)
                {
                    Background = COLOR_DEFAULT;
                }

                mainWindow.Tips = "";
            };
        }

        public MemoryItem MenuItem
        {
            get
            {
                if (MainWindow.INSTANCE.Menu.Items.ContainsKey(Id))
                {
                    return MainWindow.INSTANCE.Menu.Items[Id];
                }
                else
                {
                    return new MemoryItem() { Material = material };
                }
            }
            set
            {
                if (MainWindow.INSTANCE.Menu.Items.ContainsKey(Id))
                {
                    MainWindow.INSTANCE.Menu.Items[Id] = value;
                }
                else
                {
                    MainWindow.INSTANCE.Menu.Items.Add(Id, value);
                }
            }
        }
        public string Id;
        private readonly MainWindow mainWindow;

        public string Text
        {
            set => FormatCode.GenTextBlock(text, value);
        }
        public string Material
        {
            get => material;
            set
            {
                material = value;
                image.Source = Materials.GetMaterial(material).Image;
            }
        }
    }
    #endregion
    public partial class MainWindow : Window
    {
        private static MainWindow instance;
        public static MainWindow INSTANCE { get => instance; }

        #region 数值绑定到控件
        private void UpdateSourceCode()
        {
            if (Menu != null)
            {
                TextBoxMenuSource.Text = Menu.ToString();
                RefreshItemsList();
                // TODO 不推荐的强制转换
                ((Inventory)GridPreview.Children[0]).Update(Menu);
            }
        }
        delegate void CallBindTextValue(string s);
        private void BindTextValue(TextBox tb, CallBindTextValue call)
        {
            tb.TextChanged += delegate { call(tb.Text); UpdateSourceCode(); };
        }
        delegate void CallBindCheckValue(bool isChecked);
        private void BindCheckValue(CheckBox cb, CallBindCheckValue call)
        {
            cb.Checked += delegate { call(true); UpdateSourceCode(); };
            cb.Unchecked += delegate { call(false); UpdateSourceCode(); };
        }
        delegate void CallBindComboValue(object value);
        private void BindComboValue(ComboBox cb, CallBindComboValue call)
        {
            cb.SelectionChanged += delegate (object s, SelectionChangedEventArgs e)
            {
                if (cb.SelectedIndex >= 0)
                {
                    call(((ComboBoxItem)cb.Items[cb.SelectedIndex]).Tag);
                }

                UpdateSourceCode();
            };
        }
        delegate void CallBindSliderValue(int value);
        private void BindSliderValue(Slider s, CallBindSliderValue call)
        {
            s.ValueChanged += delegate (object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                call((int)e.NewValue);
                UpdateSourceCode();
            };
        }
        #endregion

        #region 顶部菜单
        private void NewFile(object sender, RoutedEventArgs e)
        {
            if (CurrentMenuFile != string.Empty)
            {
                MessageBoxResult result = MessageBox.Show("你正在新建一个菜单，是否保存当前菜单?", "EasyDeluxeMenus", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    SaveFile(sender, e);
                }
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }
            Menu = MemoryMenu.Default;
            LoadMenu();
        }
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "Yaml 配置文件|*.yml",
                InitialDirectory = MenusPath
            };
            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                Menu = MemoryMenu.LoadMenu(File.ReadAllText(CurrentMenuFile = dialog.FileName));
                LoadMenu();
            }
        }
        private void SaveFile(object sender, RoutedEventArgs e)
        {
            if (CurrentMenuFile == string.Empty)
            {
                SaveFileAs(sender, e);
                return;
            }
            if (Menu == null)
            {
                MessageBox.Show("当前没有打开任何菜单");
                return;
            }
            string text = Menu.ToString();
            File.WriteAllText(CurrentMenuFile, text);
            MessageBox.Show("文件已保存");
        }
        private void SaveFileAs(object sender, RoutedEventArgs e)
        {
            if (Menu == null)
            {
                MessageBox.Show("当前没有打开任何菜单");
                return;
            }
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Yaml 配置文件|*.yml",
                InitialDirectory = MenusPath
            };
            if (dialog.ShowDialog().GetValueOrDefault(false))
            {
                string text = Menu.ToString();
                File.WriteAllText(CurrentMenuFile, text);
                MessageBox.Show("文件已保存");
            }
        }
        private void OpenSettings(object sender, RoutedEventArgs e)
        {

        }
        private void ExitApp(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        public static string MenusPath
        {
            get
            {
                string path = Environment.CurrentDirectory + "\\menus";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
        public string CurrentMenuFile { get; private set; } = string.Empty;
        public MemoryMenu Menu { get; private set; } = MemoryMenu.Default;
        private readonly HovingBoxWindow tips = new HovingBoxWindow();
        private readonly MaterialWindow materialWindow = new MaterialWindow();
        public string Tips
        {
            set
            {
                tips.UpdateTips(value);
            }
        }
        public List<string> TipsList
        {
            set
            {
                tips.UpdateTips(value);
            }
        }
        private string itemSelectedId;
        public string ItemSelectedId
        {
            get => itemSelectedId;
            set
            {
                itemSelectedId = value;
                LoadItem();
            }
        }
        public MemoryItem ItemSelected
        {
            get
            {
                if (ItemSelectedId == null)
                {
                    return null;
                }

                if (Menu.Items.ContainsKey(ItemSelectedId))
                {
                    return Menu.Items[itemSelectedId];
                }

                return null;
            }
            set
            {
                if (ItemSelectedId == null)
                {
                    return;
                }

                if (Menu.Items.ContainsKey(ItemSelectedId))
                {
                    Menu.Items[itemSelectedId] = value;
                }
                else
                {
                    Menu.Items.Add(ItemSelectedId, value);
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            tips.SetPixelFont();
            GridTopmost.Children.Add(tips);
            instance = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            FormatCode.GenTextBlock(TextPreviewTipsLeft,
                new List<string>(new string[] {
                    "&7EasyDeluxeMenus 菜单预览",
                    "&7仅供参考，不代表最终效果"
                }));
            FormatCode.GenTextBlock(TextPreviewTipsRight,
                new List<string>(new string[] {
                    "&7添加选中物品 &2鼠标左键",
                    "&7移除物品 &2鼠标右键"
                }));
            #region 绑定控件数值与配置文件同步
            TabUpper.SelectionChanged += delegate { UpdateSourceCode(); };
            materialWindow.MaterialClickEvent += delegate (MaterialItem i) { ItemMaterial.Text = i.Material.Name; };
            // 菜单
            BindTextValue(MenuTitle, delegate (string s) { Menu.Title = s; });
            BindComboValue(MenuType, delegate (object value) { string s = value.ToString(); Menu.InventoryType = s == string.Empty ? null : s; });
            BindComboValue(MenuSize, delegate (object value) { if (int.TryParse(value.ToString(), out int a)) { Menu.InventorySize = a > 0 ? a : (int?)null; } });
            BindSliderValue(UpdateInterval, delegate (int value) { Menu.UpdateInterval = value > 0 ? value : (int?)null; });
            BindCheckValue(IsRegisterCommand, delegate (bool isChecked)
            {
                Menu.IsRegisterOpenCommand = isChecked ? true : (bool?)null;
                OpenCommand.IsEnabled = Args.IsEnabled = ArgsUsage.IsEnabled = isChecked;
            });
            BindTextValue(Args, delegate (string s)
            {
                if (s == string.Empty)
                {
                    Menu.CommandArgs = null;
                }
                else
                {
                    Menu.CommandArgs = new List<string>();
                    if (s.Contains(","))
                    {
                        foreach (string a in s.Split(','))
                        {
                            Menu.CommandArgs.Add(a);
                        }
                    }
                    else
                    {
                        Menu.CommandArgs.Add(s);
                    }
                }
            });
            BindTextValue(ArgsUsage, delegate (string s) { Menu.ArgsUsageMessage = s == string.Empty ? null : s; });
            BindTextValue(OpenCommand, delegate (string s)
            {
                if (s == string.Empty)
                {
                    Menu.OpenCommand = null;
                }
                else
                {
                    if (s.Contains(","))
                    {
                        List<string> temp = new List<string>();
                        foreach (string a in s.Split(','))
                        {
                            temp.Add(a);
                        }

                        Menu.OpenCommand = temp;
                    }
                    else
                    {
                        Menu.OpenCommand = s;
                    }
                }
            });

            // TODO: 物品
            BindTextValue(ItemId, delegate (string s)
            {
                if (Menu.Items.ContainsKey(s) && s != itemSelectedId)
                {
                    MemoryItem item = Menu.Items[ItemSelectedId];
                    Menu.Items.Remove(ItemSelectedId);
                    Menu.Items.Add(s, item);
                }
            });
            BindSliderValue(ItemAmount, delegate (int i) { TextItemAmount.Text = "物品数量: " + i; ItemSelected.Amount = i; });
            BindCheckValue(ItemIsUseDynamicAmount, delegate (bool b)
            {
                ItemDynamicAmount.IsEnabled = b;
                ItemAmount.IsEnabled = !b;
                TextItemAmount.Text = "物品数量: " + (b ? "(*)" : ((int)ItemAmount.Value).ToString());
            });
            BindTextValue(ItemDynamicAmount, delegate (string s)
            {
                ItemSelected.DynamicAmount = s;
            });
            BindTextValue(ItemMaterial, delegate (string s) { if (Materials.materials.ContainsKey(s)) { ItemSelected.Material = s; } });
            BindTextValue(ItemDisplayName, delegate (string s) { ItemSelected.DisplayName = s; });
            BindTextValue(ItemSlots, delegate (string s) { ItemSelected.SlotString = s; });
            BindTextValue(ItemLore, delegate (string s) { ItemSelected.LoreString = s; });
            #endregion

            Inventory inv = new InventoryChest(Menu.InventorySize.GetValueOrDefault(9) / 9);
            inv.Update(Menu);
            GridPreview.Children.Add(inv);
        }

        private void OpenMaterialWindow(object sender, RoutedEventArgs e)
        {
            materialWindow.Show();
        }
        private void LoadMenu()
        {
            MenuTitle.Text = Menu.Title;
            foreach (ComboBoxItem i in MenuSize.Items)
            {
                if (int.TryParse(i.Tag.ToString(), out int a) && a == Menu.InventorySize.GetValueOrDefault(0))
                {
                    MenuSize.SelectedItem = i;
                    break;
                }
            }
            foreach (ComboBoxItem i in MenuType.Items)
            {
                if (i.Tag.ToString() == Menu.InventoryType)
                {
                    MenuType.SelectedItem = i;
                    break;
                }
            }
            UpdateInterval.Value = Menu.UpdateInterval.GetValueOrDefault(0);
            IsRegisterCommand.IsChecked = Menu.IsRegisterOpenCommand;
            var command = Menu.OpenCommand;
            if (command is List<string>)
            {
                List<string> cmds = (List<string>)command;
                for (int i = 0; i < cmds.Count; i++)
                {
                    OpenCommand.Text += cmds[i] + (i < cmds.Count - 1 ? "," : "");
                }
            }
            else if (command is string)
            {
                OpenCommand.Text = (string)command;
            }

            if (Menu.CommandArgs != null)
            {
                for (int i = 0; i < Menu.CommandArgs.Count; i++)
                {
                    Args.Text += Menu.CommandArgs[i] + (i < Menu.CommandArgs.Count - 1 ? "," : "");
                }
            }
            ArgsUsage.Text = Menu.ArgsUsageMessage;

            UpdateSourceCode();
        }

        public void RefreshItemsList()
        {
            ItemsList.Children.Clear();
            foreach (string id in Menu.Items.Keys)
            {
                MemoryItem item = Menu.Items[id];
                ItemsList.Children.Add(new SimpleItem(this, id, item.DisplayName + "\n&7"+ id, item.Material));
            }

        }

        private void LoadItem()
        {
            if (!Menu.Items.ContainsKey(ItemSelectedId))
            {
                TabSelectedItem.IsEnabled = false;
                return;
            }

            TabSelectedItem.IsEnabled = true;

            MemoryItem item = Menu.Items[ItemSelectedId];
            ItemId.Text = ItemSelectedId;
            ItemMaterial.Text = item.Material;
            ItemDisplayName.Text = item.DisplayName;
            ItemLore.Text = item.LoreString;
            ItemSlots.Text = item.SlotString;
            ItemPriority.Text = item.Priority.ToString();

            ItemHideAttributes.IsChecked = item.HideAttributes;
            ItemHideEffects.IsChecked = item.HideEffects;
            ItemHideEnchantments.IsChecked = item.HideEnchantments;
            ItemHideUnbreakable.IsChecked = item.HideUnbreakable;
            ItemUnbreakable.IsChecked = item.Unbreakable;
        }

        #region 控件相关杂项
        private void UpdateInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int value = (int)e.NewValue;
            UpdateIntervalText.Text = value > 0 ? (value + "秒") : "无";
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            materialWindow.Close();
            tips.Close();
        }
        private void ItemMaterial_TextChanged(object sender, TextChangedEventArgs e)
        {
            ItemMaterialImage.Source = Materials.GetMaterial(ItemMaterial.Text).Image;
        }
        #endregion

        private void Items_Create(object sender, RoutedEventArgs e)
        {
            Menu.Items.Add(GenNewItemName(), new MemoryItem() { Material = "STONE", DisplayName = "新建物品" });
            UpdateSourceCode();
        }
        public string GenNewItemName()
        {
            string name = Menu.Items.Count.ToString();
            while (Menu.Items.ContainsKey(name))
            {
                name += "-1";
            }

            return name;
        }

        private void Source_Copy(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TextBoxMenuSource.Text);
            MessageBox.Show("已复制源代码");
        }

        private void ShowGithub(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://github.com/MrXiaoM/EasyDeluxeMenus");
        }
    }
}
