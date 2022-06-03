using EasyDeluxeMenus.Minecraft;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using YamlDotNet.Serialization;

namespace EasyDeluxeMenus.DeluxeMenus
{
    public class MemoryMenu
    {
        [YamlMember(Alias = "menu_title", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Title;
        [YamlMember(Alias = "register_command", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? IsRegisterOpenCommand;
        [YamlMember(Alias = "open_command", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public object OpenCommand;
        [YamlMember(Alias = "args", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> CommandArgs;
        [YamlMember(Alias = "args_usage_message", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string ArgsUsageMessage;
        [YamlMember(Alias = "open_requirement", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public RequirementsContainer OpenRequirements;
        [YamlMember(Alias = "open_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> OpenExcuteCommands;
        [YamlMember(Alias = "close_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> CloseExcuteCommands;
        [YamlMember(Alias = "inventory_type", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string InventoryType;
        [YamlMember(Alias = "size", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public int? InventorySize;
        [YamlMember(Alias = "update_interval", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public int? UpdateInterval;
        [YamlMember(Alias = "items", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public Dictionary<string, MemoryItem> Items;

        [YamlIgnore]
        public static MemoryMenu Default => new MemoryMenu()
        {
            Title = "新建菜单",
            InventoryType = "CHEST",
            InventorySize = 54
        };

        public static MemoryMenu LoadMenu(string s)
        {
            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();
            return deserializer.Deserialize<MemoryMenu>(s);
        }

        public static string SaveMenu(MemoryMenu menu)
        {
            var se = new SerializerBuilder().Build();
            return se.Serialize(menu);
        }
        /// <summary>
        /// 生成菜单的YAML配置
        /// </summary>
        /// <returns>YAML配置</returns>
        public override string ToString()
        {
            return SaveMenu(this);
        }
    }
    public class MemoryItem
    {
        [YamlMember(Alias = "material", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Material;
        [YamlMember(Alias = "data", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public object Data;
        [YamlMember(Alias = "amount", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public int? Amount;
        [YamlMember(Alias = "dynmaic_amount", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string DynamicAmount;
        [YamlMember(Alias = "nbt_string", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string NbtString;
        [YamlMember(Alias = "nbt_strings", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> NbtStrings;
        [YamlMember(Alias = "nbt_int", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string NbtInt;
        [YamlMember(Alias = "nbt_ints", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> NbtInts;
        [YamlMember(Alias = "banner_meta", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> BannerMeta;
        [YamlMember(Alias = "item_flags", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> ItemFlags;
        [YamlMember(Alias = "potion_effects", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> PotionEffects;
        [YamlMember(Alias = "rgb", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string RGB;
        [YamlMember(Alias = "display_name", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string DisplayName;
        [YamlMember(Alias = "lore", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> Lore;
        [YamlMember(Alias = "slot", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public int? Slot;
        [YamlMember(Alias = "slots", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> Slots;
        [YamlMember(Alias = "priority", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public int? Priority;
        [YamlMember(Alias = "view_requestment", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public RequirementsContainer ViewRequestment;
        [YamlMember(Alias = "update", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? Update;
        [YamlMember(Alias = "enchantments", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> Enchantents;
        [YamlMember(Alias = "hide_enchantments", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? HideEnchantments;
        [YamlMember(Alias = "hide_attributes", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? HideAttributes;
        [YamlMember(Alias = "hide_effects", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? HideEffects;
        [YamlMember(Alias = "hide_unbreakable", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? HideUnbreakable;
        [YamlMember(Alias = "unbreakable", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? Unbreakable;

        [YamlMember(Alias = "click_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> ClickCommands;
        [YamlMember(Alias = "left_click_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> LeftClickCommands;
        [YamlMember(Alias = "right_click_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> RightClickCommands;
        [YamlMember(Alias = "shift_left_click_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> ShiftLeftClickCommands;
        [YamlMember(Alias = "shift_right_click_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> ShiftRightClickCommands;
        [YamlMember(Alias = "middle_click_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> MiddleClickCommands;

        [YamlMember(Alias = "click_requirement", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public RequirementsContainer ClickRequirement;
        [YamlMember(Alias = "left_click_requirement", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public RequirementsContainer LeftClickRequirement;
        [YamlMember(Alias = "right_click_requirement", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public RequirementsContainer RightClickRequirement;
        [YamlMember(Alias = "shift_left_click_requirement", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public RequirementsContainer ShiftLeftClickRequirement;
        [YamlMember(Alias = "shift_right_click_requirement", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public RequirementsContainer ShiftRightClickRequirement;
        [YamlMember(Alias = "middle_click_requirement", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public RequirementsContainer MiddleClickRequirement;

        public Image GenSlotItem()
        {
            return new Image()
            {
                Width = 32,
                Height = 32,
                Source = Materials.GetMaterial(Material).Image
            };
        }

        public TextBlock GenSlotAmount()
        {
            string textAmount = string.Empty;
            if (DynamicAmount != string.Empty) textAmount = "#";
            else if (Amount < 0 || Amount > 1) textAmount = Amount.ToString();
            else return null;
            return new TextBlock()
            {
                Margin = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                FontFamily = App.UniFont,
                Foreground = (Amount < 0 || Amount > 64) ? Brushes.Red : Brushes.White,
                Text = textAmount
            };
        }
    }
}
