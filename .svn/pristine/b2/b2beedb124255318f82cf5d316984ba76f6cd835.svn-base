using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

namespace Pharos.POS.Retailing.Printers
{
    public class PrintTemplate
    {
        public string Template { get; set; }

        public string[] GetLines(string tpl)
        {
            return tpl.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        }
        public string GetCollections<T>(T model, Func<object, string, string, string> getItemDatas, Func<T, string, IEnumerable<dynamic>> getArrays)
        {
            var tpl = Template;
            return Regex.Replace(tpl, @"(?=\{TABLE@@)(?<mark>.*?)(?<=\})(?<tpl>.*?)(?=\{TABLE@@)(?<endmark>.*?)(?<=\})", ((match) =>
             {
                 var arr = getArrays(model, Regex.Match(match.Groups["mark"].Value.ToUpper(), @"(?<=\{TABLE@@)(?<mark>.*?)(?=\})").Value);
                 var itemTpl = string.Empty;
                 foreach (var item in arr)
                 {
                     itemTpl += GetItemNatureDatas(getItemDatas, item, match.Groups["tpl"].Value);
                     itemTpl += Environment.NewLine;
                 }
                 return itemTpl;
             }), RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }
        public string GetItemNatureDatas<T>(Func<object, string, string, string> getDatas, T model, string tpl)
        {
            var regexRule = new Regex(@"(?=\{)(?<tag>.*?)(?<=\})", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            tpl = regexRule.Replace(tpl, (match) =>
            {
                var tagRule = new Regex(@"(?<=\{)(?<tag>.*?)(?=\})");
                var arr = tagRule.Match(match.Value).Value.Split("@@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 0)
                {
                    return getDatas(model, arr[0], arr.Length == 2 ? arr[1] : string.Empty);
                }
                else
                {
                    return string.Empty;
                }
            });
            return tpl;
        }

        public string[] GetNatureDatas<T>(Func<object, string, string, string> getDatas, T model, string tpl)
        {
            var regexRule = new Regex(@"(?=\{)(?<tag>.*?)(?<=\})", RegexOptions.IgnoreCase | RegexOptions.Singleline);//(?=\{)(?<tag>.*?)(?<=\})
            tpl = regexRule.Replace(tpl, (match) =>
            {
                var tagRule = new Regex(@"(?<=\{)(?<tag>.*?)(?=\})");//(?=\{)(?<tag>.*?)(?<=\})
                var arr = tagRule.Match(match.Value).Value.Split("@@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 0)
                {
                    return getDatas(model, arr[0], arr.Length == 2 ? arr[1] : string.Empty);
                }
                else
                {
                    return string.Empty;
                }
            });
            return GetLines(tpl);
        }

        public IEnumerable<Tag> GetLineTagInfos(string line)
        {
            var regexRule = new Regex("(?<=<)(?<tag>.*?)(?=>)");
            var matchs = regexRule.Matches(line);
            List<Tag> tags = new List<Tag>();
            List<Tag> tempTags = new List<Tag>();
            var contentTags = new string[] { "T", "BMP" };
            foreach (Match item in matchs)
            {
                var arr = item.Value.Split("@@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 0)
                {
                    arr[0] = arr[0].ToUpper();
                    var tag = new Tag() { TagName = arr[0], TagValue = (arr.Length == 2 ? arr[1] : string.Empty) };
                    if (contentTags.Contains(arr[0]))
                    {
                        tag.SettingTags = tempTags;
                        tempTags = new List<Tag>();
                        tags.Add(tag);
                    }
                    else
                    {
                        tempTags.Add(tag);
                    }
                }
            }
            return tags;
        }
    }

    public class Tag
    {
        public string TagName { get; set; }

        public string TagValue { get; set; }

        public List<Tag> SettingTags { get; set; }
    }

}
