using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

/// <summary>
/// ��Ʒ�࣬������Ʒ���е����ԣ�����
/// </summary>
namespace TPSShoot.Bags
{
    [System.Serializable]
    public class Item
    {
        private int id;
        private int minGrade; // ��͵ȼ�
        private int maxGrade; // ��ߵȼ�
        private int grade; // ��ǰ�ȼ�
        private float gradeAddition; // ÿ���ӳ�
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
        /// ��Ʒ���ͣ�����Ʒ��װ��������������
        /// </summary>
        public ItemType Type { get => type; set => type = value; }
        /// <summary>
        /// Ʒ�ʣ�
        /// </summary>
        public ItemQuality Quality { get => quality; set => quality = value; }
        /// <summary>
        /// ����
        /// </summary>
        public string Description { get => description; set => description = value; }
        /// <summary>
        /// ��������
        /// </summary>
        public int Capacity { get => capacity; set => capacity = value; }
        /// <summary>
        /// ����۸�
        /// </summary>
        public int Buyprice { get => buyprice; set => buyprice = value; }
        /// <summary>
        /// ���ۼ۸�
        /// </summary>
        public int Sellprice { get => sellprice; set => sellprice = value; }

        /// <summary>
        /// ͼƬ
        /// </summary>
        public string Sprite { get => sprite; set => sprite = value; }
        

        public Item()
        {

        }
        // ������
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

        #region Ʒ��ö����
        /// <summary>
        ///Common       һ��         white ��ɫ
        ///Uncommon ��һ��        lime �̻�ɫ
        ///Rare ϡ��         navy ����ɫ
        ///Epic ʷʫ        magenta Ʒ��
        ///Legendary ��˵        orange ��ɫ
        ///Artifact Զ��        red ��ɫ
        /// </summary>
        public enum ItemQuality
        {
            /// <summary>
            /// һ��� ��ɫ
            /// </summary>
            Common,
            /// <summary>
            /// ��һ��� ��ɫ
            /// </summary>
            Uncommon,
            /// <summary>
            /// ϡ�е� ��ɫ
            /// </summary>
            Rare,
            /// <summary>
            /// ʷʫ ��ɫ
            /// </summary>
            Epic,
            /// <summary>
            /// ��˵ ��ɫ
            /// </summary>
            Legendary,
            /// <summary>
            /// Զ�� ����ɫ
            /// </summary>
            Artifact
        }
        #endregion
        #region ����ö����
        /// <summary>
        /// ����Ʒ װ�� ���� ����
        /// Consumable Equipment Weapon Material
        /// </summary>
        public enum ItemType
        {
            /// <summary>
            /// ����Ʒ
            /// </summary>
            Consumable,
            /// <summary>
            /// װ��
            /// </summary>
            Equipment,
            /// <summary>
            /// ����
            /// </summary>
            Weapon,
            /// <summary>
            /// ����
            /// </summary>
            Material,
            /// <summary>
            /// �ӵ�
            /// </summary>
            Bullet
        }
        #endregion

        private string GetTypeStr()
        {
            switch (Type)
            {
                case ItemType.Consumable: return "����Ʒ";
                case ItemType.Equipment: return "װ��";
                case ItemType.Weapon: return "����";
                case ItemType.Material: return "����";
                case ItemType.Bullet: return "�ӵ�";
            }
            return "δ֪";
        }

        public virtual string TipShow()
        {
            string c = "#ff0000";
            //"<color={4}>��������</color>"
            string tip = string.Format("<color={0}><size=18>{1}</size></color>   <color={2}><size=18>{3}��</size></color>" +
                "\n<size=12>����:{4}</size>\n{5}\n" +
                "<color=yellow>����۸�:{6} ���ۼ۸�:{7}</color>\n"
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
                    return "#ffffff"; // ��ɫ
                case ItemQuality.Uncommon:
                    return "#DED59E"; // ��ɫ
                case ItemQuality.Rare:
                    return "#0000ff"; // ��ɫ
                case ItemQuality.Epic:
                    return "#ff0000"; // ��ɫ
                case ItemQuality.Legendary:
                    return "#E54FCE"; // ��ɫ
                case ItemQuality.Artifact:
                    return "#8A1898"; // ��ɫ
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
