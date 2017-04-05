using Pharos.POS.Retailing.Models;
using Pharos.POS.Retailing.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pharos.POS.Retailing.Printers
{
    public static class PrintLayoutHelper
    {
        static PrintLayoutHelper()
        {
            RegistreFunc();
        }
        private static Dictionary<Type, Dictionary<string, Func<object, string, string>>> funcs = new Dictionary<Type, Dictionary<string, Func<object, string, string>>>();
        private static Dictionary<Type, Dictionary<string, Func<IEnumerable<dynamic>>>> arrayfuncs = new Dictionary<Type, Dictionary<string, Func<IEnumerable<dynamic>>>>();
        private static Dictionary<string, Func<object, string, string>> gfuncs = new Dictionary<string, Func<object, string, string>>();
        public static FlowDocument GetDocument()
        {
            var doc = new FlowDocument();
            doc.FontFamily = new FontFamily("Microsoft YaHei UI");
            doc.IsOptimalParagraphEnabled = false;
            return doc;
        }

        public static void RegistreFunc()
        {
            #region New SaleOrder
            RegistreFunc<PosViewModel>("ORDERDATE", (p1, p2) =>
            {
                var model = p1 as PosViewModel;
                if (string.IsNullOrEmpty(p2))
                {
                    return model.Date.ToString("yyyyMMdd HH:mm:ss");

                }
                else
                {
                    return model.Date.ToString(p2);
                }
            });
            RegistreFunc<PosViewModel>("STORE", (p1, p2) =>
            {
                return Global.MachineSettings.MachineInformations.StoreName;
            });
            RegistreFunc<PosViewModel>("STOREID", (p1, p2) =>
            {
                return Global.MachineSettings.MachineInformations.StoreId;
            });
            RegistreFunc<PosViewModel>("MACHINE", (p1, p2) =>
            {
                return Global.MachineSettings.MachineInformations.MachineSn;
            });
            RegistreFunc<PosViewModel>("CASHER", (p1, p2) =>
            {
                return Global.CurrentSaleMen.FullName;
            });
            RegistreFunc<PosViewModel>("CASHERID", (p1, p2) =>
            {
                return Global.CurrentSaleMen.UserCode;
            });
            RegistreFunc<PosViewModel>("SALEMAN", (p1, p2) =>
            {
                var model = p1 as PosViewModel;
                return model.SaleMan;
            });
            RegistreFunc<PosViewModel>("PAYSN", (p1, p2) =>
            {
                var model = p1 as PosViewModel;
                return model.OrderSn;
            });
            RegistreFunc<PosViewModel>("NUM", (p1, p2) =>
            {
                var model = p1 as PosViewModel;
                return model.Num.ToString("N");
            });
            RegistreFunc<PosViewModel>("PREFERENTIAL", (p1, p2) =>
            {
                var model = p1 as PosViewModel;
                return model.Preferential.ToString("N");
            });
            RegistreFunc<PosViewModel>("PREFERENTIAL", (p1, p2) =>
            {
                var model = p1 as PosViewModel;
                return model.Receivable.ToString("N");
            });
            RegistreArray<PosViewModel>("ORDERS", () => { return PosViewModel.Current.OrderList; });
            RegistreFunc<Product>("BARCODE", (p1, p2) =>
            {
                var model = p1 as Product;
                return model.Barcode;
            });
            RegistreFunc<Product>("PRODUCT", (p1, p2) =>
            {
                var model = p1 as Product;
                return model.Title;
            });
            RegistreFunc<Product>("PRICE", (p1, p2) =>
            {
                var model = p1 as Product;
                return model.ActualPrice.ToString("N");
            });
            RegistreFunc<Product>("TOTAL", (p1, p2) =>
            {
                var model = p1 as Product;
                return model.Total.ToString("N");
            });
            #endregion New SaleOrder


        }
        public static void RegistreArray<T>(string mark, Func<IEnumerable<dynamic>> getItemsFunc)
        {
            var type = typeof(T);
            Dictionary<string, Func<IEnumerable<dynamic>>> func;
            if (arrayfuncs.ContainsKey(type))
            {
                func = arrayfuncs[type];
                if (func.ContainsKey(mark))
                {
                    func[mark] = getItemsFunc;
                }
                else
                {
                    func.Add(mark, getItemsFunc);
                }
            }
            else
            {
                var dict = new Dictionary<string, Func<IEnumerable<dynamic>>>();
                dict.Add(mark, getItemsFunc);
                arrayfuncs.Add(type, dict);
            }
        }

        public static void RegistreFunc<T>(string mark, Func<object, string, string> formatFunc)
        {
            var type = typeof(T);
            Dictionary<string, Func<object, string, string>> func;
            if (funcs.ContainsKey(type))
            {
                func = funcs[type];
                if (func.ContainsKey(mark))
                {
                    func[mark] = formatFunc;
                }
                else
                {
                    func.Add(mark, formatFunc);
                }
            }
            else
            {
                var dict = new Dictionary<string, Func<object, string, string>>();
                dict.Add(mark, formatFunc);
                funcs.Add(type, dict);
            }
        }

        public static string GetModelDatas(object model, string mark, string format)
        {
            Type _type = model.GetType();
            if (funcs.ContainsKey(_type))
            {
                var modelFuncs = funcs[_type];
                if (modelFuncs.ContainsKey(mark))
                {
                    return modelFuncs[mark](model, format);
                }
            }
            else if (gfuncs.ContainsKey(mark))
            {
                return gfuncs[mark](model, format);
            }
            return "";
        }
        public static IEnumerable<dynamic> GetCollections<T>(T model, string mark)
        {
            var type = model.GetType();
            if (arrayfuncs.ContainsKey(type) && arrayfuncs[type].ContainsKey(mark))
            {
                return arrayfuncs[type][mark]();
            }
            return new List<dynamic>();
        }
        public static void Format<T>(this FlowDocument doc, PrintTemplate tpl, T model)
        {
            var temptpl = tpl.GetCollections(model, GetModelDatas, GetCollections);
            var lines = tpl.GetNatureDatas(GetModelDatas, model, temptpl).Select(o => tpl.GetLineTagInfos(o));
            var ContentTags = new string[] { "BMP", "T" };
            foreach (var tags in lines)
            {
                Paragraph block = new Paragraph();
                block.LineHeight = 0.0034;
                block.Margin = new Thickness(0);
                foreach (var tag in tags)
                {
                    if (TagFormat(block, tag, tags))
                    {
                        break;
                    }
                }
                if (tags.Count() > 0)
                {
                    doc.Blocks.Add(block);
                }
            }
        }

        private static bool TagFormat(Paragraph block, Tag tag, IEnumerable<Tag> tags)
        {
            var result = false;
            switch (tag.TagName)
            {
                case "BR":
                    if (!string.IsNullOrEmpty(tag.TagValue))
                        block.Margin = new System.Windows.Thickness(0, Convert.ToDouble(tag.TagValue), 0, 0);
                    break;
                case "BMP":
                    if (!string.IsNullOrEmpty(tag.TagValue))
                    {
                        Figure figure = new Figure();
                        figure.Margin = new Thickness(0);
                        var bitmap = new BitmapImage(new Uri(tag.TagValue, UriKind.RelativeOrAbsolute));
                        var image = new Image { Source = bitmap, Stretch = Stretch.Fill };

                        BlockUIContainer imgBlock = new BlockUIContainer(image);
                        if (tag.SettingTags != null && tag.SettingTags.Count > 0)
                        {
                            foreach (var imgSetting in tag.SettingTags)
                            {
                                FormatImg(image, imgBlock, imgSetting);
                            }
                        }
                        figure.Blocks.Add(imgBlock);
                        block.Inlines.Add(figure);
                        result = true;
                    }
                    break;
                case "T":
                    if (!string.IsNullOrEmpty(tag.TagValue))
                    {
                        Figure figure = new Figure();
                        TextBlock tb = new TextBlock();
                        figure.Margin = new Thickness(0);

                        BlockUIContainer tBlock = new BlockUIContainer(tb);
                        tBlock.Margin = new Thickness(0);

                        foreach (var tempTag in tags)
                        {
                            var ctb = new TextBlock();
                            ctb.Text = tempTag.TagValue;
                            foreach (var imgSetting in tempTag.SettingTags)
                            {
                                FormatText(tb, ctb, tBlock, imgSetting);
                            }
                            tb.Inlines.Add(ctb);
                        }
                        figure.Blocks.Add(tBlock);
                        block.Inlines.Add(figure);
                        result = true;
                    }
                    break;

            }
            return result;
        }
        private static void FormatImg(Image img, BlockUIContainer buic, Tag tag)
        {
            switch (tag.TagName)
            {
                case "W":
                    try
                    {
                        if (!string.IsNullOrEmpty(tag.TagValue))
                            img.Width = Convert.ToDouble(tag.TagValue);
                    }
                    catch { }
                    break;
                case "H":
                    try
                    {
                        if (!string.IsNullOrEmpty(tag.TagValue))
                            img.Height = Convert.ToDouble(tag.TagValue);
                    }
                    catch { }
                    break;
                case "L":
                    try
                    {
                        if (!string.IsNullOrEmpty(tag.TagValue))
                            img.Margin = new System.Windows.Thickness(0)
                            {
                                Left = Convert.ToDouble(tag.TagValue)
                            };
                    }
                    catch { }
                    break;
                case "R":
                    try
                    {
                        if (!string.IsNullOrEmpty(tag.TagValue))
                            img.Margin = new System.Windows.Thickness(0)
                            {
                                Right = Convert.ToDouble(tag.TagValue)
                            };
                    }
                    catch { }
                    break;
            }
        }

        private static void FormatText(TextBlock ptb, TextBlock tb, BlockUIContainer buic, Tag tag)
        {
            switch (tag.TagName)
            {
                case "W":
                    try
                    {
                        if (!string.IsNullOrEmpty(tag.TagValue))
                            tb.Width = Convert.ToDouble(tag.TagValue);
                    }
                    catch { }
                    break;
                case "L":
                    try
                    {
                        if (!string.IsNullOrEmpty(tag.TagValue))
                            tb.Margin = new System.Windows.Thickness(0)
                            {
                                Left = Convert.ToDouble(tag.TagValue)
                            };
                    }
                    catch { }
                    break;
                case "R":
                    try
                    {
                        if (!string.IsNullOrEmpty(tag.TagValue))
                            tb.Margin = new System.Windows.Thickness(0)
                            {
                                Right = Convert.ToDouble(tag.TagValue)
                            };
                    }
                    catch { }
                    break;
                case "F":
                    try
                    {
                        if (!string.IsNullOrEmpty(tag.TagValue))
                            tb.FontSize = Convert.ToDouble(tag.TagValue);
                    }
                    catch { }
                    break;
                case "B":
                    try
                    {
                        tb.FontWeight = FontWeights.Bold;
                    }
                    catch { }
                    break;
            }
        }
    }
}
