using CsharpJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EasyDeluxeMenus.Minecraft
{
    public class Material
    {
        /// <summary>
        /// 物品ID
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 物品图标
        /// </summary>
        public ImageSource Image { get; private set; }
        /// <summary>
        /// 物品翻译名
        /// </summary>
        public string TranslateName { get; private set; }
        internal Material(string name, string translateName, ImageSource image)
        {
            Name = name;
            TranslateName = translateName;
            Image = image;
        }
    }
    /// <summary>
    /// Minecraft 物品材质及其翻译名管理器
    /// </summary>
    public class Materials
    {
        public static readonly Dictionary<string, ImageSource> materials = new Dictionary<string, ImageSource>();
        public static readonly Dictionary<string, string> translateMap = new Dictionary<string, string>();
        public static ImageSource NotFoundImage;
        public static Material GetMaterial(string name)
        {
            string s = name.ToUpper();
            string translate = translateMap.ContainsKey(s) ? translateMap[s] : s;
            ImageSource image = materials.ContainsKey(s) ? materials[s] : NotFoundImage;
            return new Material(s, translate, image);
        }
        public static void Load()
        {
            NotFoundImage = (ImageSource)new ImageSourceConverter().ConvertFrom(Properties.Resources.error);
            materials.Clear();
            DirectoryInfo di = new DirectoryInfo(Environment.CurrentDirectory + "\\items");
            foreach (FileInfo file in di.GetFiles("*.png"))
            {
                ImageSource image = new BitmapImage(new Uri(file.FullName, UriKind.Absolute));
                materials.Add(file.Name[0..^4], image);
            }
            translateMap.Clear();
            if (File.Exists(Environment.CurrentDirectory + "\\items\\_config.json"))
            {
                string text = File.ReadAllText(Environment.CurrentDirectory + "\\items\\_config.json");
                JsonDocument doc = JsonDocument.FromString(text);
                if (doc.IsObject())
                {
                    JsonObject obj = doc.Object;
                    foreach (string s in obj.Keys)
                    {
                        translateMap.Add(s.ToUpper(), obj[s].ToString());
                    }
                }
            }
        }
    }
}
