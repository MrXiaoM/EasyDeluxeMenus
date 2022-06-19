using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace EasyDeluxeMenus.Minecraft
{
    /// <summary>
    /// Minecraft 格式化代码 (又称: 颜色代码、样式代码)
    /// (https://minecraft.fandom.com/zh/wiki/%E6%A0%BC%E5%BC%8F%E5%8C%96%E4%BB%A3%E7%A0%81)
    /// 在 WPF 中的实现
    /// </summary>
    public class FormatCode
    {
        public static Color Hex(string s)
        {
            if (!s.StartsWith("#")) s = "#" + s;
            return (Color)ColorConverter.ConvertFromString(s);
        }

        public static SolidColorBrush FromMinecraft(char c)
        {
            Color color;
            switch (char.ToLower(c))
            {
                case '0':
                    color = Hex("000000");
                    break;
                case '1':
                    color = Hex("0000AA");
                    break;
                case '2':
                    color = Hex("00AA00");
                    break;
                case '3':
                    color = Hex("00AAAA");
                    break;
                case '4':
                    color = Hex("AA0000");
                    break;
                case '5':
                    color = Hex("AA00AA");
                    break;
                case '6':
                    color = Hex("FFAA00");
                    break;
                case '7':
                    color = Hex("AAAAAA");
                    break;
                case '8':
                    color = Hex("555555");
                    break;
                case '9':
                    color = Hex("5555FF");
                    break;
                case 'a':
                    color = Hex("55FF55");
                    break;
                case 'b':
                    color = Hex("55FFFF");
                    break;
                case 'c':
                    color = Hex("FF5555");
                    break;
                case 'd':
                    color = Hex("FF55FF");
                    break;
                case 'e':
                    color = Hex("FFFF55");
                    break;
                case 'r':
                case 'f':
                    color = Hex("FFFFFF");
                    break;
            }
            if (color == null) return null;
            return new SolidColorBrush(color);
        }

        public static TextBlock GenTextBlock(string text)
        {
            TextBlock tb = new TextBlock();
            GenTextBlock(tb, text);
            return tb;
        }
        public static TextBlock GenTextBlock(List<string> texts)
        {
            TextBlock tb = new TextBlock();
            GenTextBlock(tb, texts);
            return tb;
        }
        public static void GenTextBlock(TextBlock tb, string text)
        {
            GenTextBlock(tb, new List<string>(text.Contains("\n") ? text.Split('\n') : new string[] { text }));
        }
        public static void GenTextBlock(TextBlock tb, List<string> texts)
        {
            tb.Inlines.Clear();
            for (int i = 0; i < texts.Count; i++)
            {
                if (i > 0) tb.Inlines.Add(new LineBreak());
                string text = texts[i];
                int size = text.Length;
                Run cache = new Run();
                for (int index = 0; index < size; index++)
                {
                    if (text[index] == '§' || text[index] == '&')
                    {
                        if (index + 1 >= size) break;
                        char c = char.ToLower(text[index + 1]);
                        // 加粗
                        if (c == 'l')
                        {
                            if (cache.Text != string.Empty) tb.Inlines.Add(cache);
                            cache = Inherit(cache, null, FontWeights.Bold);
                        }
                        // 斜体
                        else if (c == 'o')
                        {
                            if (cache.Text != string.Empty) tb.Inlines.Add(cache);
                            cache = Inherit(cache, FontStyles.Italic);
                        }
                        // 下划线、删除线
                        else if (c == 'n' || c == 'm')
                        {
                            if (cache.Text != string.Empty) tb.Inlines.Add(cache);
                            TextDecorationCollection deco = c == 'n' ? TextDecorations.Underline : TextDecorations.Strikethrough;
                            cache = Inherit(cache, null, null, deco);
                        }
                        // TODO: 魔法文字
                        else if (c == 'k')
                        {
                            if (cache.Text != string.Empty) tb.Inlines.Add(cache);
                            cache = Inherit(cache);
                            cache.Tag = "magic";
                            // 临时解决方案
                            cache.Background = cache.Foreground;
                        }
                        // 颜色
                        else
                        {
                            Brush color = FromMinecraft(c);
                            if (color != null)
                            {
                                if (cache.Text != string.Empty) tb.Inlines.Add(cache);
                                cache = new Run() { Foreground = color };
                            }
                        }
                        index++;
                    }
                    else if (text[index] != '\n') cache.Text += text[index];
                }
                if (cache.Text != string.Empty) tb.Inlines.Add(cache);
            }
        }

        public static Run Inherit(Run old, FontStyle? newStyle = null, FontWeight? newWeight = null, TextDecorationCollection newDeco = null)
        {
            Run cache = new Run()
            {
                FontStyle = newStyle ?? old.FontStyle,
                Foreground = old.Foreground,
                FontWeight = newWeight ?? old.FontWeight,
                TextDecorations = old.TextDecorations,
                Tag = old.Tag
            };
            if (newDeco != null) cache.TextDecorations.Add(newDeco);
            return cache;
        }
    }
}
