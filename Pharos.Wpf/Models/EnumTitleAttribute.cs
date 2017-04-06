using System;

namespace Pharos.Wpf.Models
{
    public class EnumTitleAttribute : Attribute
    {
        public EnumTitleAttribute(int value, string title)
        {
            Title = title;
            Value = value;
        }

        public string Title { get; set; }
        public int Value { get; set; }
    }
}
