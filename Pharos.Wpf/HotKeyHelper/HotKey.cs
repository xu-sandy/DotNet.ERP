﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Pharos.Wpf.Extensions;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Controls;

namespace Pharos.Wpf.HotKeyHelper
{
    public static class HotKey
    {
        private const string COFINGFILE = "PharosHotKeyRules.Config";
        static HotKey()
        {
            Rules = new List<HotKeyRule>();
            Mode = HotkeyMode.Open;
        }
        /// <summary>
        /// 过滤器
        /// </summary>
        public static Func<HotKeyRule, bool> Filter { get; set; }
        /// <summary>
        /// 快捷键全部设置
        /// </summary>
        public static List<HotKeyRule> Rules { get; private set; }

        public static HotkeyMode Mode { get; set; }

        public static void LoadConfig()
        {
            XDocument doc = XDocument.Load(COFINGFILE);
            HotKey.Rules = doc.Element("Settings").Elements("HotKeyRule")
                .Select(o =>
                    new HotKeyRule()
                    {
                        Name = o.Attribute("Name").Value,
                        Title = o.Attribute("Title").Value,
                        IsShowInHelp = o.Attribute("IsShowInHelp") != null ? Convert.ToBoolean(o.Attribute("IsShowInHelp").Value) : true,
                        IsShowInMainWindow = o.Attribute("IsShowInMainWindow") != null ? Convert.ToBoolean(o.Attribute("IsShowInMainWindow").Value) : false,
                        Keys = o.Attribute("Keys").Value.Replace("Ctrl", "Control"),
                        Effectivity = new List<Type>().InitListType(o.Elements("Effect")),
                        EnableSet = o.Attribute("EnableSet") != null ? Convert.ToBoolean(o.Attribute("EnableSet").Value) : false
                    }).ToList();
        }

        public static void SaveConfig()
        {
            XDocument doc = new XDocument();
            var element = new XElement("Settings");
            foreach (var item in Rules)
            {
                var cmdType = item.Command.GetType();
                XElement node = new XElement(
                    "HotKeyRule",
                    new XAttribute("Name", item.Name),
                    new XAttribute("Keys", item.Keys),
                    new XAttribute("Title", item.Title),
                    new XAttribute("IsShowInHelp", item.IsShowInHelp),
                    new XAttribute("IsShowInMainWindow", item.IsShowInMainWindow),
                    new XAttribute("EnableSet", item.EnableSet)
                    );

                foreach (var childItem in item.Effectivity)
                {
                    XElement effectList = new XElement("Effect", childItem);
                    node.Add(effectList);
                }
                element.Add(node);
            }
            doc.Add(element);
            doc.Save(COFINGFILE);
        }
        /// <summary>
        /// 从窗体绑定热键
        /// </summary>
        /// <param name="win"></param>
        public static void ApplyHotKeyBindings(this Window win)
        {
            win.PreviewKeyDown += win_PreviewKeyDown;
        }
        static object lockobj = new object();
        static List<KeyPointItem> keyPoints = new List<KeyPointItem>();
        static void win_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Mode == HotkeyMode.Close && e.Key != Key.F4) return;
            var keys = Keyboard.Modifiers.ToString();
            var modifierKeys = keys.Replace(",", "+");
            if (modifierKeys == "None")
            {
                modifierKeys = string.Empty;
            }

            string key = string.Empty;
            switch (e.Key)
            {
                case Key.LeftAlt:
                case Key.LeftCtrl:
                case Key.LeftShift:
                case Key.RightAlt:
                case Key.RightCtrl:
                case Key.RightShift:
                    break;
                case Key.System:
                    switch (e.SystemKey)
                    {
                        case Key.LeftAlt:
                        case Key.LeftCtrl:
                        case Key.LeftShift:
                        case Key.RightAlt:
                        case Key.RightCtrl:
                        case Key.RightShift:
                            break;
                        default:
                            key = e.SystemKey.ToString();
                            break;
                    }
                    break;
                case Key.OemPlus:
                case Key.Add:
                    key = "Plus";
                    break;
                case Key.OemMinus:
                case Key.Subtract:
                    key = "Minus";
                    break;
                default:
                    key = e.Key.ToString();
                    break;

            }
            if (!string.IsNullOrEmpty(modifierKeys) && !string.IsNullOrEmpty(key))
            {
                modifierKeys += "+";
            }
            var hotKey = string.Format("{0}{1}", modifierKeys, key);
            string[] systemKeys = new string[] { "None", "Alt", "Control", "Shift", "Windows" };
            if (systemKeys.Contains(hotKey))
            {
                return;
            }

            var item = new KeyPointItem() { HotKey = hotKey, PressTime = DateTime.Now };
            var temp = new List<KeyPointItem>();

            lock (lockobj)
            {
                if (keyPoints != null && keyPoints.Count > 0)
                {
                    temp.Add(keyPoints.LastOrDefault());
                }
                temp.Add(item);
                keyPoints = temp;
            }
            if (temp.Count == 2)
            {
                var oneItem = temp.FirstOrDefault();
                var twoItem = temp.LastOrDefault();
                if ((oneItem.PressTime - twoItem.PressTime).Duration() < new TimeSpan(0, 0, 0, 0, 200))
                {
                    return;
                }
            }

            // Thread.Sleep(200);
            // Regex regex = new Regex("");
            var activeRule = Rules.FirstOrDefault(o => o.Keys.VerfyKey(hotKey) && o.Effectivity.Contains(sender.GetType()));
            if (activeRule != null)
            {
                if ((Filter != null && Filter(activeRule)) || Filter == null)
                {
                    Task.Factory.StartNew((o) =>
                    {
                        Thread.Sleep(200);
                        var info = o as KeyPointItem;
                        //   var o = hotKey;
                        List<KeyPointItem> tempKeys = null;
                        lock (lockobj)
                        {
                            tempKeys = keyPoints;
                        }
                        var len = 1;
                        if (systemKeys.Any(j => info.HotKey.Contains(j)))
                        {
                            len = 0;
                        }
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            if (tempKeys.Count == 1)
                            {
                                activeRule.Command.Handler(sender as Window);
                                ReseText(len);
                            }
                            else if (tempKeys.Count == 2 && tempKeys.FindIndex(p => p.Id == info.Id) == 0 && tempKeys.First().PressTime - tempKeys.Last().PressTime > new TimeSpan(0, 0, 0, 0, 200))
                            {
                                activeRule.Command.Handler(sender as Window);
                                ReseText(len);
                            }
                            else if (tempKeys.Count == 2 && tempKeys.FindIndex(p => p.Id == info.Id) == 1)
                            {
                                activeRule.Command.Handler(sender as Window);
                                ReseText(len);
                            }
                        }));
                    }, item);
                }
            }
        }
        private static void ReseText(int len)
        {
            if (Keyboard.FocusedElement is TextBox)
            {
                var ctrl = Keyboard.FocusedElement as TextBox;
                if (ctrl.Text.Length > len && len != 0)
                {
                    ctrl.Text = ctrl.Text.Substring(0, ctrl.Text.Length - len);
                }
                else if (len != 0)
                {
                    ctrl.Text = string.Empty;
                }
                ctrl.SelectAll();
                keyPoints = new List<KeyPointItem>();
            }

        }
        public static bool VerfyKey(this string keys, string key)
        {
            var keyArr = keys.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            return keyArr.Contains(key);
        }
    }

    class KeyPointItem
    {
        public KeyPointItem()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string HotKey { get; set; }
        public DateTime PressTime { get; set; }
    }

}