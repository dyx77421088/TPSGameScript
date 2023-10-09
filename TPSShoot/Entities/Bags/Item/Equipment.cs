using Newtonsoft.Json.Utilities;
using System.Text;
using TPSShoot.Manger;
/// <summary>
/// 装备类
/// </summary>
namespace TPSShoot.Bags
{
    public class Equipment : Item
    {
        private int strength;
        private int intellect;
        private int agility;
        private int stamina;
        private EquipmentType equipType;

        #region get set
        /// <summary>
        /// 力量
        /// </summary>
        public int Strength { get => strength + (int)(Grade * GradeAddition * GetQualityMagnification()); set => strength = value; }
        /// <summary>
        /// 智力
        /// </summary>
        public int Intellect { get => intellect + (int)(Grade * GradeAddition * GetQualityMagnification()); set => intellect = value; }
        /// <summary>
        /// 敏捷
        /// </summary>
        public int Agility { get => agility + (int)(Grade * GradeAddition * GetQualityMagnification()); set => agility = value; }
        /// <summary>
        /// 体力
        /// </summary>
        public int Stamina { get => stamina + (int)(Grade * GradeAddition * GetQualityMagnification()); set => stamina = value; }
        public EquipmentType EquipType { get => equipType; set => equipType = value; }
        #endregion

        #region 装备的构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="name">名字</param>
        /// <param name="type">类型</param>
        /// <param name="quality">品质</param>
        /// <param name="description">描述</param>
        /// <param name="capacity">最大叠加数</param>
        /// <param name="buyprice">购买价格</param>
        /// <param name="sellprice">出售价格</param>
        /// <param name="sprite">图片</param>
        /// <param name="strength">力量</param>
        /// <param name="intellect">智力</param>
        /// <param name="agility">敏捷</param>
        /// <param name="stamina">体力</param>
        /// <param name="equipType">装备类型</param>
        public Equipment(int id, string name, ItemType type, ItemQuality quality,
            string description, int capacity, int buyprice, int sellprice, string sprite,
            int strength, int intellect, int agility, int stamina, EquipmentType equipType)
            : base(id, name, type, quality, description, capacity, buyprice, sellprice, sprite)
        {
            this.strength = strength;
            this.intellect = intellect;
            this.agility = agility;
            this.stamina = stamina;
            this.equipType = equipType;
        }
        public Equipment() : base() { }
        #endregion

        #region 装备类型枚举类
        /// <summary>
        /// Head  头
        ///Neck 脖子
        ///Chest 胸部
        ///Ring 戒指
        ///Leg 腿
        ///Bracer 护腕
        ///Boots 靴子
        ///Trinket 饰品
        ///shoulder 肩膀
        ///belt 腰带
        ///Glove    手套 (加)
        /// </summary>
        public enum EquipmentType
        {
            None,
            /// <summary>
            /// 头盔
            /// </summary>
            Head,
            /// <summary>
            /// 衣服
            /// </summary>
            Clothing,

            /// <summary>
            /// 护腕
            /// </summary>
            Bracer,

            /// <summary>
            /// 饰品
            /// </summary>
            Trinket,
            /// <summary>
            /// 手镯
            /// </summary>
            Bracelet,
            /// <summary>
            /// 戒指
            /// </summary>
            Ring,
            /// <summary>
            /// 靴子
            /// </summary>
            Boots,
            /// <summary>
            /// 腰带
            /// </summary>
            Belt,
            /// <summary>
            /// 手套
            /// </summary>
            Glove
        }
        #endregion

        #region 根据武器类型的枚举类获得中文名
        public string GetTypeStr()
        {
            switch (EquipType)
            {
                case EquipmentType.None:
                    return "空";
                case EquipmentType.Head:
                    return "头部";
                case EquipmentType.Clothing:
                    return "衣服";
                case EquipmentType.Bracer:
                    return "护腕";
                case EquipmentType.Trinket:
                    return "饰品";
                case EquipmentType.Bracelet:
                    return "手镯";
                case EquipmentType.Ring:
                    return "戒指";
                case EquipmentType.Boots:
                    return "鞋子";
                case EquipmentType.Belt:
                    return "腰带";
                case EquipmentType.Glove:
                    return "手套";
                default:
                    return "装备";
            }
        }
        #endregion

        /// <summary>
        /// 品质不同再乘以不同的倍率
        /// </summary>
        /// <returns></returns>
        private float GetQualityMagnification()
        {
            switch (Quality)
            {
                case ItemQuality.Common:
                    return 0.5f;
                case ItemQuality.Uncommon:
                    return 0.7f;
                case ItemQuality.Rare:
                    return 1f;
                case ItemQuality.Epic:
                    return 1.3f;
                case ItemQuality.Legendary:
                    return 1.7f;
                case ItemQuality.Artifact:
                    return 2.2f;
            }
            return 1;
        }
        public override string TipShow()
        {
            string qs = "#7CFFF0"; // 青色
            string str = base.TipShow();
            str = str.Replace("类型:装备", "部位:" + GetTypeStr());
            StringBuilder sb = new StringBuilder();
            if (Strength != 0) sb.Append("力量+").Append(Strength).Append("(").Append(minAttribute(1))
                    .Append("-").Append(maxAttribute(1)).Append(")").AppendLine();
            if (Intellect != 0) sb.Append("智力+").Append(Intellect).Append("(").Append(minAttribute(2))
                    .Append("-").Append(maxAttribute(2)).Append(")").AppendLine();
            if (Agility != 0) sb.Append("敏捷+").Append(Agility).Append("(").Append(minAttribute(3))
                    .Append("-").Append(maxAttribute(3)).Append(")").AppendLine();
            if (Stamina != 0) sb.Append("体力+").Append(Stamina).Append("(").Append(minAttribute(4))
                    .Append("-").Append(maxAttribute(4)).Append(")").AppendLine();
            return str + string.Format("<color={0}>基本属性</color>\n<color={2}>{1}</color>", qs, sb, GetQualityColor());
        }

        /// <summary>
        /// 最低属性，attribute(1-4)分别表示力量、智力、敏捷、体力
        /// </summary>
        /// <returns></returns>
        private int minAttribute(int attribute)
        {
            // 每级属性的加成 + 基础 - 波动
            return (int)(Grade * GradeAddition * GetQualityMagnification()) + getAttribute(attribute) - MonsterDroppedManager.Instance.reduce;
        }
        private int maxAttribute(int attribute)
        {
            // 每级属性的加成 + 基础 + 波动, 看到的是当前的等级的最大属性
            return (int)(Grade * GradeAddition * GetQualityMagnification()) + getAttribute(attribute) + MonsterDroppedManager.Instance.increase;
        }
        private int getAttribute(int type)
        {
            Equipment item = ((Equipment)PlayerBagBehaviour.Instance.GetItemById(Id));
            switch (type)
            {
                case 1: return item.strength;
                case 2: return item.intellect;
                case 3: return item.agility;
                case 4: return item.stamina;
            }
            return 0;
        }
    }
}
