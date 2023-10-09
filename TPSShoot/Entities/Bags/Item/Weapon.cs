using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 武器
/// </summary>
namespace TPSShoot.Bags
{
    public class Weapon : Equipment
    {
        private int damage;
        private WeaponType type;

        public int Damage { get => damage + (int)(Grade * GradeAddition); set => damage = value; }
        public WeaponType WType { get => type; set => type = value; }

        public Weapon(int id, string name, ItemType type, ItemQuality quality,
            string description, int capacity, int buyprice, int sellprice, string sprite,
            int strength, int intellect, int agility, int stamina, EquipmentType equipType,
            int damage, WeaponType weaponType)
            : base(id, name, type, quality, description, capacity, buyprice, sellprice, sprite,
                strength, intellect, agility, stamina, equipType)
        {
            this.damage = damage;
            this.WType = weaponType;
        }

        /// <summary>
        /// Mainhand    主手
        /// Offhand 副手
        /// </summary>
        public enum WeaponType
        {
            None,
            /// <summary>
            /// 双手武器
            /// </summary>
            Mainhand,
            /// <summary>
            /// 单手武器
            /// </summary>
            Offhand
        }

        private string GetWeqponTypeStr()
        {
            switch (type)
            {
                case WeaponType.Mainhand:
                    return "双手武器";
                case WeaponType.Offhand:
                    return "单手武器";
                default:
                    return "武器";
            }
        }

        public override string TipShow()
        {
            string qs = "#7CFFF0"; // 青色
            string str = base.TipShow();
            str = str.Replace("类型:武器", "类型:" + GetWeqponTypeStr());
            StringBuilder sb = new StringBuilder();
            if (Damage != 0) sb.Append("攻击力+").Append(Damage).AppendLine();
            return str + sb;
        }
    }
}
