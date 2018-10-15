using System;
using System.ComponentModel;

namespace FeaturesDemo
{
    public static class CheckPower
    {
        /// <summary>
        /// 检查是否有权限，
        /// |操作会合并成一个值，表示满足之一就返回true
        /// 如果有多个值，表示需要满足全部值才返回true
        /// </summary>
        /// <param name="ower">用户的权限值</param>
        /// <param name="powers">要检查的权限值</param>
        /// <returns></returns>
        public static bool CheckHasPower(EnumPower owner, params EnumPower[] powers)
        {
            //没有传权限过来，直接返回false
            if (powers == null || powers.Length < 1)
                return false;
            //只传一个权限(单权限，或者几个权限的|操作)
            if (powers.Length <= 1)
            {
                //如果是EnumPower.AllNull，直接比较是否和owner一样
                if (powers[0].Equals(EnumPower.AllNull))
                    return owner.Equals(powers[0]);
                else
                    return (owner & powers[0]) != 0x00;
            }

            //合并要检查的权限值
            foreach (var item in powers)
            {
                //核对每一项，如果有一项不成立，返回false
                if ((owner & item) == 0x00)
                    return false;
            }

            return true;
        }
    }

    [Flags]
    public enum EnumPower
    {
        [Description("游客")]
        AllNull = 0,             //0x00 表示全部没有    0000
        [Description("创建")]
        Create = 1 << 0,         //0x01或者1  2的0次方  0001
        [Description("读取")]
        Read = 1 << 1,           //0x02或者2  2的1次方  0010
        [Description("更新")]
        Update = 1 << 2,         //0x04或者4  2的2次方  0100
        [Description("删除")]
        Delete = 1 << 3          //0x08或者8  2的3次方  1000
    }
}
