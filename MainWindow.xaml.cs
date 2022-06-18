using EasyDeluxeMenus.DeluxeMenus;
using EasyDeluxeMenus.Minecraft;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
                this.Items.Add(new ComboBoxItem()
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
                this.Items.Add(new ComboBoxItem()
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
    public class SimpleItem: Grid
    {
        public static readonly SolidColorBrush COLOR_DEFAULT = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
        public static readonly SolidColorBrush COLOR_SELECTED = new SolidColorBrush(Color.FromArgb(90, 0, 0, 0));
        Image image = new Image()
        {
            Margin = new Thickness(2),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Width = 16,
            Height = 16,
        };
        TextBlock text = new TextBlock()
        {
            Margin = new Thickness(20, 2, 2, 2),
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Foreground = new SolidColorBrush(Colors.White)
        };
        private string material;
        public SimpleItem(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.Background = COLOR_DEFAULT;
            this.Height = 24;
            this.Children.Add(image);
            this.Children.Add(text);
            this.MouseDown += delegate
            {
                mainWindow.ItemSelectedId = Id;
                foreach (SimpleItem i in mainWindow.ItemsList.Children)
                {
                    i.Background = COLOR_DEFAULT;
                }
                this.Background = COLOR_SELECTED;
            };
        }

        public MemoryItem MenuItem
        {
            get
            {
                if (MainWindow.INSTANCE.Menu.Items.ContainsKey(Id))
                    return MainWindow.INSTANCE.Menu.Items[Id];
                else return new MemoryItem() { Material = material };
            }
            set
            {
                if (MainWindow.INSTANCE.Menu.Items.ContainsKey(Id))
                    MainWindow.INSTANCE.Menu.Items[Id] = value;
                else MainWindow.INSTANCE.Menu.Items.Add(Id, value);
            }
        }
        public string Id;
        private MainWindow mainWindow;

        public string Text
        {
            get => text.Text;
            set => text.Text = value;
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
            if (Menu != null) TextBoxMenuSource.Text = Menu.ToString();
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
                if (cb.SelectedIndex >= 0) call(((ComboBoxItem)cb.Items[cb.SelectedIndex]).Tag);
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
                if (result == MessageBoxResult.Cancel) return;
            }
            this.Menu = MemoryMenu.Default;
            this.LoadMenu();
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
                this.LoadMenu();
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
            this.Close();
        }
        #endregion

        public static string MenusPath
        {
            get
            {
                string path = Environment.CurrentDirectory + "\\menus";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        public string CurrentMenuFile { get; private set; } = string.Empty;
        public MemoryMenu Menu { get; private set; } = MemoryMenu.Default;

        private readonly MaterialWindow materialWindow = new MaterialWindow();
        private readonly HovingBoxWindow tips = new HovingBoxWindow();
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
                if (this.Menu.Items.ContainsKey(ItemSelectedId))
                    return this.Menu.Items[itemSelectedId];
                return null;
            }
            set
            {
                if (this.Menu.Items.ContainsKey(ItemSelectedId))
                    this.Menu.Items[itemSelectedId] = value;
                else this.Menu.Items.Add(ItemSelectedId, value);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            instance = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region 绑定控件数值与配置文件同步
            GridUpper.SelectionChanged += delegate { UpdateSourceCode(); };
            materialWindow.MaterialClickEvent += delegate (MaterialItem i) { this.ItemMaterial.Text = i.Material.Name; };
            // 菜单
            BindTextValue(MenuTitle, delegate (string s) { this.Menu.Title = s; });
            BindComboValue(MenuType, delegate (object value) { string s = value.ToString(); this.Menu.InventoryType = s == string.Empty ? null : s; });
            BindComboValue(MenuSize, delegate (object value) { if (int.TryParse(value.ToString(), out int a)) { this.Menu.InventorySize = a > 0 ? a : (int?)null; } });
            BindSliderValue(UpdateInterval, delegate (int value) { this.Menu.UpdateInterval = value > 0 ? value : (int?)null; });
            BindCheckValue(IsRegisterCommand, delegate (bool isChecked) { this.Menu.IsRegisterOpenCommand = isChecked ? true : (bool?)null; });
            BindTextValue(Args, delegate (string s)
            {
                if (s == string.Empty) this.Menu.CommandArgs = null;
                else
                {
                    this.Menu.CommandArgs = new List<string>();
                    if (s.Contains(",")) { foreach (string a in s.Split(',')) this.Menu.CommandArgs.Add(a); }
                    else this.Menu.CommandArgs.Add(s);
                }
            });
            BindTextValue(ArgsUsage, delegate (string s) { this.Menu.ArgsUsageMessage = s == string.Empty ? null : s; });
            BindTextValue(OpenCommand, delegate (string s)
            {
                if (s == string.Empty) this.Menu.OpenCommand = null;
                else
                {
                    if (s.Contains(","))
                    {
                        List<string> temp = new List<string>();
                        foreach (string a in s.Split(',')) temp.Add(a);
                        this.Menu.OpenCommand = temp;
                    }
                    else this.Menu.OpenCommand = s;
                }
            });

            // TODO: 物品
            BindTextValue(ItemId, delegate (string s)
            {
                if (this.Menu.Items.ContainsKey(s) && s != itemSelectedId)
                {
                    MemoryItem item = this.Menu.Items[ItemSelectedId];
                    this.Menu.Items.Remove(ItemSelectedId);
                    this.Menu.Items.Add(s, item);
                    this.RefreshItemsList();
                }
            });
            BindTextValue(ItemDisplayName, delegate (string s) { this.ItemSelected.DisplayName = s; });
            BindTextValue(ItemSlots, delegate (string s) { this.ItemSelected.SlotString = s; });
            BindTextValue(ItemLore, delegate (string s) { this.ItemSelected.LoreString = s; });
            #endregion
        }

        private void OpenMaterialWindow(object sender, RoutedEventArgs e)
        {
            this.materialWindow.Show();
        }
        private void LoadMenu()
        {
            this.MenuTitle.Text = Menu.Title;
            foreach (ComboBoxItem i in this.MenuSize.Items)
            {
                if (int.TryParse(i.Tag.ToString(), out int a) && a == Menu.InventorySize.GetValueOrDefault(0))
                {
                    this.MenuSize.SelectedItem = i;
                    break;
                }
            }
            foreach (ComboBoxItem i in this.MenuType.Items)
            {
                if (i.Tag.ToString() == Menu.InventoryType)
                {
                    this.MenuType.SelectedItem = i;
                    break;
                }
            }
            this.UpdateInterval.Value = this.Menu.UpdateInterval.GetValueOrDefault(0);
            this.IsRegisterCommand.IsChecked = this.Menu.IsRegisterOpenCommand;
            var command = this.Menu.OpenCommand;
            if (command is List<string>)
            {
                List<string> cmds = (List<string>)command;
                for (int i = 0; i < cmds.Count; i++)
                {
                    this.OpenCommand.Text += cmds[i] + (i < cmds.Count - 1 ? "," : "");
                }
            }
            else if (command is string) this.OpenCommand.Text = (string)command;
            if (this.Menu.CommandArgs != null) {
                for (int i = 0; i < this.Menu.CommandArgs.Count; i++)
                {
                    this.Args.Text += this.Menu.CommandArgs[i] + (i < this.Menu.CommandArgs.Count - 1 ? "," : "");
                } 
            }
            this.ArgsUsage.Text = this.Menu.ArgsUsageMessage;

            this.RefreshItemsList();
        }

        public void RefreshItemsList()
        {
            this.ItemsList.Children.Clear();
            foreach (string id in this.Menu.Items.Keys)
            {
                MemoryItem item = this.Menu.Items[id];
                this.ItemsList.Children.Add(new SimpleItem(this) { Id = id, Text = item.DisplayName, Material = item.Material });
            }
        }
        
        private void LoadItem()
        {
            if (!this.Menu.Items.ContainsKey(ItemSelectedId)) return;
            MemoryItem item = this.Menu.Items[ItemSelectedId];
            this.ItemId.Text = ItemSelectedId;
            this.ItemMaterial.Text = item.Material;
            this.ItemDisplayName.Text = item.DisplayName;
            this.ItemLore.Text = item.LoreString;
            this.ItemSlots.Text = item.SlotString;
            this.ItemPriority.Text = item.Priority.ToString();

            this.ItemHideAttributes.IsChecked = item.HideAttributes;
            this.ItemHideEffects.IsChecked = item.HideEffects;
            this.ItemHideEnchantments.IsChecked = item.HideEnchantments;
            this.ItemHideUnbreakable.IsChecked = item.HideUnbreakable;
            this.ItemUnbreakable.IsChecked = item.Unbreakable;
        }

        #region 控件相关杂项
        private void IsRegisterCommand_Checked(object sender, RoutedEventArgs e)
        {
            OpenCommand.IsEnabled = Args.IsEnabled = true;
        }
        private void IsRegisterCommand_UnChecked(object sender, RoutedEventArgs e)
        {
            OpenCommand.IsEnabled = Args.IsEnabled = false;
        }
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
            this.ItemMaterialImage.Source = Materials.GetMaterial(this.ItemMaterial.Text).Image;
        }
        #endregion

        private void Items_Create(object sender, RoutedEventArgs e)
        {
            this.Menu.Items.Add(GenNewItemName(), new MemoryItem() { Material = "STONE", DisplayName = "新建物品" });
            this.RefreshItemsList();
            this.UpdateSourceCode();
        }
        public string GenNewItemName()
        {
            string name = this.Menu.Items.Count.ToString();
            while (this.Menu.Items.ContainsKey(name)) name += "-1";
            return name;
        }

        private void Source_Copy(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(TextBoxMenuSource.Text);
            MessageBox.Show("已复制源代码");
        }
    }
}
