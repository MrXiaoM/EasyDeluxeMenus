using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace EasyDeluxeMenus.DeluxeMenus
{
    public class RequirementsContainer
    {
        [YamlMember(Alias = "minimum_requirements", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public int MinimumRequirements;
        [YamlMember(Alias = "stop_at_success", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool StopAtSuccess;
        [YamlMember(Alias = "requirements", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public Dictionary<string, Requirement> Requirements;
        [YamlMember(Alias = "deny_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string[] DenyCommmands;
    }
    public class Requirement
    {
        [YamlMember(Alias = "type")]
        public string Type;
        [YamlMember(Alias = "perission", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Permission;
        [YamlMember(Alias = "amount", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public int? Amount;
        [YamlMember(Alias = "placeholder", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Placeholder;
        [YamlMember(Alias = "material", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Material;
        [YamlMember(Alias = "data", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public int? Data;
        [YamlMember(Alias = "name", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Name;
        [YamlMember(Alias = "lore", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> Lore;
        [YamlMember(Alias = "name_contains", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? NameContains;
        [YamlMember(Alias = "name_ignorecase", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? NameIgnoreCase;
        [YamlMember(Alias = "lore_contains", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? LoreContains;
        [YamlMember(Alias = "lore_ignorecase", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? LoreIgnoreCase;
        [YamlMember(Alias = "strict", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? Strict;
        [YamlMember(Alias = "armor", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? Armor;
        [YamlMember(Alias = "offhand", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? Offhand;

        [YamlMember(Alias = "key", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Key;
        [YamlMember(Alias = "meta_type", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string MetaType;
        [YamlMember(Alias = "value", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Value;

        [YamlMember(Alias = "level", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? Level;

        [YamlMember(Alias = "location", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Location;
        [YamlMember(Alias = "distance", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public int? Distance;

        [YamlMember(Alias = "expression", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Expression;
        [YamlMember(Alias = "input", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Input;
        [YamlMember(Alias = "output", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Output;

        [YamlMember(Alias = "regex", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public string Regex;

        [YamlMember(Alias = "success_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> SuccessCommmands;
        [YamlMember(Alias = "deny_commands", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public List<string> DenyCommmands;
        [YamlMember(Alias = "optional", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
        public bool? Optional;
    }
}
