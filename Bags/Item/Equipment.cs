using Newtonsoft.Json.Utilities;
using System.Text;
/// <summary>
/// װ����
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
    /// ����
    /// </summary>
    public int Strength { get => strength; set => strength = value; }
    /// <summary>
    /// ����
    /// </summary>
    public int Intellect { get => intellect; set => intellect = value; }
    /// <summary>
    /// ����
    /// </summary>
    public int Agility { get => agility; set => agility = value; }
    /// <summary>
    /// ����
    /// </summary>
    public int Stamina { get => stamina; set => stamina = value; }
    public EquipmentType EquipType { get => equipType; set => equipType = value; }
    #endregion
   
    #region װ���Ĺ��캯��
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="name">����</param>
    /// <param name="type">����</param>
    /// <param name="quality">Ʒ��</param>
    /// <param name="description">����</param>
    /// <param name="capacity">��������</param>
    /// <param name="buyprice">����۸�</param>
    /// <param name="sellprice">���ۼ۸�</param>
    /// <param name="sprite">ͼƬ</param>
    /// <param name="strength">����</param>
    /// <param name="intellect">����</param>
    /// <param name="agility">����</param>
    /// <param name="stamina">����</param>
    /// <param name="equipType">װ������</param>
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

    #region װ������ö����
    /// <summary>
    /// Head  ͷ
    ///Neck ����
    ///Chest �ز�
    ///Ring ��ָ
    ///Leg ��
    ///Bracer ����
    ///Boots ѥ��
    ///Trinket ��Ʒ
    ///shoulder ���
    ///belt ����
    ///Glove    ���� (��)
    /// </summary>
    public enum EquipmentType
    {
        None,
        /// <summary>
        /// ͷ��
        /// </summary>
        Head,
        /// <summary>
        /// �·�
        /// </summary>
        Clothing,
        
        /// <summary>
        /// ����
        /// </summary>
        Bracer,
        
        /// <summary>
        /// ��Ʒ
        /// </summary>
        Trinket,
        /// <summary>
        /// ����
        /// </summary>
        Bracelet,
        /// <summary>
        /// ��ָ
        /// </summary>
        Ring,
        /// <summary>
        /// ѥ��
        /// </summary>
        Boots,
        /// <summary>
        /// ����
        /// </summary>
        Belt,
        /// <summary>
        /// ����
        /// </summary>
        Glove
    }
    #endregion

    #region �����������͵�ö������������
    public string GetTypeStr()
    {
        switch (EquipType)
        {
            case EquipmentType.None:
                return "��";
            case EquipmentType.Head:
                return "ͷ��";
            case EquipmentType.Clothing:
                return "�·�";
            case EquipmentType.Bracer:
                return "����";
            case EquipmentType.Trinket:
                return "��Ʒ";
            case EquipmentType.Bracelet:
                return "����";
            case EquipmentType.Ring:
                return "��ָ";
            case EquipmentType.Boots:
                return "Ь��";
            case EquipmentType.Belt:
                return "����";
            case EquipmentType.Glove:
                return "����";
            default:
                return "װ��";
        }
    }
    #endregion

    public override string TipShow()
    {
        string qs = "#7CFFF0"; // ��ɫ
        string str = base.TipShow();
        str = str.Replace("����:װ��", "��λ:" + GetTypeStr());
        StringBuilder sb = new StringBuilder();
        if (strength != 0) sb.Append("����+").Append(strength).AppendLine();
        if (intellect != 0) sb.Append("����+").Append(intellect).AppendLine();
        if (agility != 0) sb.Append("����+").Append(agility).AppendLine();
        if (stamina != 0) sb.Append("����+").Append(stamina).AppendLine();
        return str + string.Format("<color={0}>��������</color>\n<color={2}>{1}</color>", qs, sb, GetQualityColor());
    }
}
