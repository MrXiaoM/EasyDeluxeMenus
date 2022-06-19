using EasyDeluxeMenus.Minecraft;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using YamlDotNet.Serialization;

namespace EasyDeluxeMenus.DeluxeMenus
{
    public class MemoryMenu
    {
        [YamlIgnore]
        private static readonly ISerializer SERIALIZER = new SerializerBuilder().Build();
        private static readonly IDeserializer DESERIALIZER = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();
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
        public Dictionary<string, MemoryItem> Items = new Dictionary<string, MemoryItem>();

        [YamlIgnore]
        public List<MemoryItem> PreviewItems
        {
            get
            {
                List<MemoryItem> items = new List<MemoryItem>(Items.Values);
                items.Sort(new PreviewComparer());
                return items;
            }
        }
        /// <summary>
        /// 按优先级排序
        /// </summary>
        class PreviewComparer : IComparer<MemoryItem>
        {
            public int Compare([AllowNull] MemoryItem x, [AllowNull] MemoryItem y)
            {
                if (x == null && x == null) return 0;
                if (x == null) return -1;
                if (y == null) return 1;
                int p0 = x.Priority.GetValueOrDefault(0);
                int p1 = y.Priority.GetValueOrDefault(0);
                return p0 == p1 ? 0 : (p0 < p1 ? 1 : -1);
            }
        }

        [YamlIgnore]
        public static MemoryMenu Default => new MemoryMenu()
        {
            Title = "新建菜单",
            InventoryType = "CHEST",
            InventorySize = 54
        };

        public static MemoryMenu LoadMenu(string s)
        {
            return DESERIALIZER.Deserialize<MemoryMenu>(s);
        }

        public static string SaveMenu(MemoryMenu menu)
        {
            return SERIALIZER.Serialize(menu);
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
        public List<string> Enchantments;
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
        
        [YamlIgnore]
        public List<string> Tooltips
        {
            get
            {
                List<string> tooltip = new List<string>();
                tooltip.Add(DisplayName);
                if (Enchantments != null && !HideEnchantments.GetValueOrDefault(false))
                {
                    foreach (string ench in Enchantments)
                    {
                        // TODO 汉化附魔
                        tooltip.Add("&7" + ench);
                    }
                }
                if (PotionEffects != null && !HideEffects.GetValueOrDefault(false))
                {
                    foreach (string eff in PotionEffects)
                    {
                        // TODO 汉化药水效果
                        tooltip.Add("&7" + eff);
                    }
                }
                if (Lore != null)
                {
                    foreach (string s in Lore)
                    {
                        tooltip.Add(s);
                    }
                }
                if (Unbreakable.GetValueOrDefault(false) && !HideUnbreakable.GetValueOrDefault(false))
                {
                    tooltip.Add("");
                    tooltip.Add("&9无法破坏");
                }
                return tooltip;
            }
        }

        [YamlIgnore]
        public Material TrueMaterial
        {
            get
            {
                return Materials.GetMaterial(this.Material);
            }
        }

        [YamlIgnore]
        public HashSet<int> SlotsInt
        {
            get
            {
                HashSet<int> set = new HashSet<int>();

                if (Slots != null && Slots.Count > 0)
                {
                    for (int i = 0; i < Slots.Count; i++)
                    {
                        if (int.TryParse(Slots[i], out int a))
                        {
                            set.Add(a);
                        }
                        else if (Slots[i].Contains('-'))
                        {
                            string[] values = Slots[i].Split('-');
                            if (values.Length == 2 && int.TryParse(values[0], out int start) && int.TryParse(values[1], out int end))
                            {
                                for(int j = start; j <= end; j++)
                                {
                                    set.Add(j);
                                }
                            }
                        }
                    }
                }
                else if (Slot.HasValue) set.Add(Slot.Value);
                return set;
            }
        }

        [YamlIgnore]
        public string SlotString
        {
            get
            {
                if (Slots != null && Slots.Count > 0)
                {
                    string result = "";
                    for (int i = 0; i < Slots.Count; i++)
                    {
                        result += Slots[i] + (i < Slots.Count - 1 ? "," : "");
                    }
                    return result;
                }
                else if (Slot.HasValue) return Slot.Value.ToString();
                return "";
            }
            set
            {
                if (!value.Contains(','))
                {
                    if (int.TryParse(value, out int a))
                    {
                        Slot = a;
                        Slots = null;
                        return;
                    }
                }
                Slot = null;
                Slots = null;
                string[] args = value.Split(',');
                foreach (string s in args)
                {
                    if (int.TryParse(s, out int a))
                    {
                        if (Slots == null) Slots = new List<string>();
                        Slots.Add(a.ToString());
                    }
                    else if (s.Contains('-'))
                    {
                        string[] values = s.Split('-');
                        if (values.Length == 2 && int.TryParse(values[0], out int start) && int.TryParse(values[1], out int end))
                        {
                            if (Slots == null) Slots = new List<string>();
                            Slots.Add(start + "-" + end);
                        }
                    }
                }
            }
        }

        [YamlIgnore]
        public string LoreString
        {
            get
            {
                if (Lore == null || Lore.Count < 1) return "";
                string result = "";
                for (int i = 0; i < Lore.Count; i++)
                {
                    result += Lore[i] + (i < Lore.Count - 1 ? "\n" : "");
                }
                return result;
            }
            set
            {
                Lore = new List<string>();
                value = value.Replace("\r", "");
                if (value.Contains('\n')) foreach (string s in value.Split('\n')) Lore.Add(s);
                else if (value.Length > 0) Lore.Add(value);
                else Lore = null;
            }
        }

        [YamlIgnore]
        public Image SlotItemImage
        {
            get
            {
                return new Image()
                {
                    Width = 32,
                    Height = 32,
                    Source = Materials.GetMaterial(Material).Image
                };
            }
        }

        [YamlIgnore]
        public TextBlock SlotAmountTextBox
        {
            get
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
}
