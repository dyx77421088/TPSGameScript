using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// ����
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
        /// Mainhand    ����
        /// Offhand ����
        /// </summary>
        public enum WeaponType
        {
            None,
            /// <summary>
            /// ˫������
            /// </summary>
            Mainhand,
            /// <summary>
            /// ��������
            /// </summary>
            Offhand
        }

        private string GetWeqponTypeStr()
        {
            switch (type)
            {
                case WeaponType.Mainhand:
                    return "˫������";
                case WeaponType.Offhand:
                    return "��������";
                default:
                    return "����";
            }
        }

        public override string TipShow()
        {
            string qs = "#7CFFF0"; // ��ɫ
            string str = base.TipShow();
            str = str.Replace("����:����", "����:" + GetWeqponTypeStr());
            StringBuilder sb = new StringBuilder();
            if (Damage != 0) sb.Append("������+").Append(Damage).AppendLine();
            return str + sb;
        }
    }
}
