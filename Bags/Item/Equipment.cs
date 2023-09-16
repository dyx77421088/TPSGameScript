using Newtonsoft.Json.Utilities;
using System.Text;
/// <summary>
/// 装备类
/// </summary>
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
    public int Strength { get => strength; set => strength = value; }
    /// <summary>
    /// 智力
    /// </summary>
    public int Intellect { get => intellect; set => intellect = value; }
    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agility { get => agility; set => agility = value; }
    /// <summary>
    /// 体力
    /// </summary>
    public int Stamina { get => stamina; set => stamina = value; }
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

    public override string TipShow()
    {
        string qs = "#7CFFF0"; // 青色
        string str = base.TipShow();
        str = str.Replace("类型:装备", "部位:" + GetTypeStr());
        StringBuilder sb = new StringBuilder();
        if (strength != 0) sb.Append("力量+").Append(strength).AppendLine();
        if (intellect != 0) sb.Append("智力+").Append(intellect).AppendLine();
        if (agility != 0) sb.Append("敏捷+").Append(agility).AppendLine();
        if (stamina != 0) sb.Append("体力+").Append(stamina).AppendLine();
        return str + string.Format("<color={0}>基本属性</color>\n<color={2}>{1}</color>", qs, sb, GetQualityColor());
    }
}
