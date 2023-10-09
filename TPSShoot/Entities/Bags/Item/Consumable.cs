using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 消耗品
/// </summary>
namespace TPSShoot.Bags
{
    public class Consumable : Item
    {
        private int hp;
        private int mp;

        public int Hp { get => hp; set => hp = value; }
        public int Mp { get => mp; set => mp = value; }

        public Consumable(int id, string name, ItemType type, ItemQuality quality,
            string description, int capacity, int buyprice, int sellprice, string sprite, int hp, int mp)
            : base(id, name, type, quality, description, capacity, buyprice, sellprice, sprite)
        {
            this.Hp = hp;
            this.Mp = mp;
        }

        public override string TipShow()
        {
            string qs = "#7CFFF0"; // 青色
            string str = base.TipShow();
            StringBuilder sb = new StringBuilder();
            if (hp != 0) sb.Append("回复血量+").Append(hp).AppendLine();
            if (mp != 0) sb.Append("回复蓝量+").Append(mp).AppendLine();
            return str + string.Format("<color={0}>回复效果</color>\n{1}", qs, sb);
        }
    }
}
