using System;
using System.ComponentModel;
using System.Reflection;

namespace FeaturesDemo
{
    public static class EnumCN
    {
        public static string GetDescription(Enum obj)
        {
            FieldInfo fi = obj.GetType().GetField(obj.ToString());
            DescriptionAttribute[] arrDesc = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (arrDesc != null && arrDesc.Length > 0)
                return arrDesc[0].Description;
            else
                return null;
        }
    }

    public enum PersonSex
    {
        [Description("男人")]
        Man = 0,
        [Description("女人")]
        Woman = 1,
        [Description("人妖")]
        OtherSex = 2
    }
}
