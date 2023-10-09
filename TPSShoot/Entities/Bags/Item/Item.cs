using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

/// <summary>
/// 物品类，所有物品共有的属性，父类
/// </summary>
namespace TPSShoot.Bags
{
    [System.Serializable]
    public class Item
    {
        private int id;
        private int minGrade; // 最低等级
        private int maxGrade; // 最高等级
        private int grade; // 当前等级
        private float gradeAddition; // 每级加成
        private string name;
        private ItemType type;
        private ItemQuality quality;
        private string description;
        private int capacity;
        private int buyprice;
        private int sellprice;
        private string sprite;

        public int Id { get => id; set => id = value; }
        public int MinGrade { get => minGrade; set => minGrade = value; }
        public int MaxGrade { get => maxGrade; set => maxGrade = value; }
        public int Grade { get => grade; set => grade = value; }
        public float GradeAddition { get => gradeAddition; set => gradeAddition = value; }
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// 物品类型，消耗品，装备，武器，材料
        /// </summary>
        public ItemType Type { get => type; set => type = value; }
        /// <summary>
        /// 品质，
        /// </summary>
        public ItemQuality Quality { get => quality; set => quality = value; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get => description; set => description = value; }
        /// <summary>
        /// 最大叠加数
        /// </summary>
        public int Capacity { get => capacity; set => capacity = value; }
        /// <summary>
        /// 购买价格
        /// </summary>
        public int Buyprice { get => buyprice; set => buyprice = value; }
        /// <summary>
        /// 出售价格
        /// </summary>
        public int Sellprice { get => sellprice; set => sellprice = value; }

        /// <summary>
        /// 图片
        /// </summary>
        public string Sprite { get => sprite; set => sprite = value; }
        

        public Item()
        {

        }
        // 构造器
        public Item(int id, string name, ItemType type, ItemQuality quality,
            string description, int capacity, int buyprice, int sellprice, string sprite)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
            this.Quality = quality;
            this.Description = description;
            this.Sprite = sprite;
            this.Capacity = capacity;
            this.Buyprice = buyprice;
            this.Sellprice = sellprice;
        }

        #region 品质枚举类
        /// <summary>
        ///Common       一般         white 白色
        ///Uncommon 不一般        lime 绿黄色
        ///Rare 稀有         navy 深蓝色
        ///Epic 史诗        magenta 品红
        ///Legendary 传说        orange 橘色
        ///Artifact 远古        red 红色
        /// </summary>
        public enum ItemQuality
        {
            /// <summary>
            /// 一般的 白色
            /// </summary>
            Common,
            /// <summary>
            /// 不一般的 黄色
            /// </summary>
            Uncommon,
            /// <summary>
            /// 稀有的 蓝色
            /// </summary>
            Rare,
            /// <summary>
            /// 史诗 红色
            /// </summary>
            Epic,
            /// <summary>
            /// 传说 粉色
            /// </summary>
            Legendary,
            /// <summary>
            /// 远古 深紫色
            /// </summary>
            Artifact
        }
        #endregion
        #region 类型枚举类
        /// <summary>
        /// 消耗品 装备 武器 材料
        /// Consumable Equipment Weapon Material
        /// </summary>
        public enum ItemType
        {
            /// <summary>
            /// 消耗品
            /// </summary>
            Consumable,
            /// <summary>
            /// 装备
            /// </summary>
            Equipment,
            /// <summary>
            /// 武器
            /// </summary>
            Weapon,
            /// <summary>
            /// 材料
            /// </summary>
            Material,
            /// <summary>
            /// 子弹
            /// </summary>
            Bullet
        }
        #endregion

        private string GetTypeStr()
        {
            switch (Type)
            {
                case ItemType.Consumable: return "消耗品";
                case ItemType.Equipment: return "装备";
                case ItemType.Weapon: return "武器";
                case ItemType.Material: return "材料";
                case ItemType.Bullet: return "子弹";
            }
            return "未知";
        }

        public virtual string TipShow()
        {
            string c = "#ff0000";
            //"<color={4}>基本属性</color>"
            string tip = string.Format("<color={0}><size=18>{1}</size></color>   <color={2}><size=18>{3}级</size></color>" +
                "\n<size=12>类型:{4}</size>\n{5}\n" +
                "<color=yellow>购买价格:{6} 出售价格:{7}</color>\n"
                , GetQualityColor(quality), name, c, grade, GetTypeStr(), description, buyprice, sellprice);
            return tip;
        }
        public string GetQualityColor()
        {
            return GetQualityColor(quality);
        }
        public string GetQualityColor(ItemQuality quality)
        {
            switch (quality)
            {
                case ItemQuality.Common:
                    return "#ffffff"; // 白色
                case ItemQuality.Uncommon:
                    return "#DED59E"; // 黄色
                case ItemQuality.Rare:
                    return "#0000ff"; // 蓝色
                case ItemQuality.Epic:
                    return "#ff0000"; // 红色
                case ItemQuality.Legendary:
                    return "#E54FCE"; // 粉色
                case ItemQuality.Artifact:
                    return "#8A1898"; // 紫色
                default:
                    break;
            }
            return "white";
        }

        public string DropingTip()
        {
            return string.Format("<color={0}><size=18>{1}</size></color>"
                , GetQualityColor(quality), name);
        }

        public Item Copy()
        {
            return (Item)MemberwiseClone();
        }
    }
}
