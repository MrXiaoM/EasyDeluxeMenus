using EasyDeluxeMenus.DeluxeMenus;
using EasyDeluxeMenus.Minecraft;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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

    public partial class MainWindow : Window
    {
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            for (int i = 0; i < this.Menu.CommandArgs.Count; i++)
            {
                this.Args.Text += this.Menu.CommandArgs[i] + (i < this.Menu.CommandArgs.Count - 1 ? "," : "");
            }
            this.ArgsUsage.Text = this.Menu.ArgsUsageMessage;
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

        #endregion

        private void ItemMaterial_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ItemMaterialImage.Source = Minecraft.Materials.GetMaterial(this.ItemMaterial.Text).Image;
        }
    }
}
