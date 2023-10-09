using Newtonsoft.Json.Utilities;
using System.Text;
using TPSShoot.Manger;
/// <summary>
/// װ����
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
        /// ����
        /// </summary>
        public int Strength { get => strength + (int)(Grade * GradeAddition * GetQualityMagnification()); set => strength = value; }
        /// <summary>
        /// ����
        /// </summary>
        public int Intellect { get => intellect + (int)(Grade * GradeAddition * GetQualityMagnification()); set => intellect = value; }
        /// <summary>
        /// ����
        /// </summary>
        public int Agility { get => agility + (int)(Grade * GradeAddition * GetQualityMagnification()); set => agility = value; }
        /// <summary>
        /// ����
        /// </summary>
        public int Stamina { get => stamina + (int)(Grade * GradeAddition * GetQualityMagnification()); set => stamina = value; }
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
        public Equipment() : base() { }
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

        /// <summary>
        /// Ʒ�ʲ�ͬ�ٳ��Բ�ͬ�ı���
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
            string qs = "#7CFFF0"; // ��ɫ
            string str = base.TipShow();
            str = str.Replace("����:װ��", "��λ:" + GetTypeStr());
            StringBuilder sb = new StringBuilder();
            if (Strength != 0) sb.Append("����+").Append(Strength).Append("(").Append(minAttribute(1))
                    .Append("-").Append(maxAttribute(1)).Append(")").AppendLine();
            if (Intellect != 0) sb.Append("����+").Append(Intellect).Append("(").Append(minAttribute(2))
                    .Append("-").Append(maxAttribute(2)).Append(")").AppendLine();
            if (Agility != 0) sb.Append("����+").Append(Agility).Append("(").Append(minAttribute(3))
                    .Append("-").Append(maxAttribute(3)).Append(")").AppendLine();
            if (Stamina != 0) sb.Append("����+").Append(Stamina).Append("(").Append(minAttribute(4))
                    .Append("-").Append(maxAttribute(4)).Append(")").AppendLine();
            return str + string.Format("<color={0}>��������</color>\n<color={2}>{1}</color>", qs, sb, GetQualityColor());
        }

        /// <summary>
        /// ������ԣ�attribute(1-4)�ֱ��ʾ���������������ݡ�����
        /// </summary>
        /// <returns></returns>
        private int minAttribute(int attribute)
        {
            // ÿ�����Եļӳ� + ���� - ����
            return (int)(Grade * GradeAddition * GetQualityMagnification()) + getAttribute(attribute) - MonsterDroppedManager.Instance.reduce;
        }
        private int maxAttribute(int attribute)
        {
            // ÿ�����Եļӳ� + ���� + ����, �������ǵ�ǰ�ĵȼ����������
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
