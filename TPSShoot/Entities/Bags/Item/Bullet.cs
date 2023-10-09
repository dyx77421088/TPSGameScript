
using System.Text;

namespace TPSShoot.Bags
{
    public class Bullet : Item
    {
        private int bulletCount;


        public Bullet(int id, string name, ItemType type, ItemQuality quality,
            string description, int capacity, int buyprice, int sellprice, string sprite)
            : base(id, name, type, quality, description, capacity, buyprice, sellprice, sprite)
        {
        }

        public int BulletCount { get => bulletCount; set => bulletCount = value; }

        public override string TipShow()
        {
            string qs = "#7CFFF0"; // ��ɫ
            string str = base.TipShow();
            StringBuilder sb = new StringBuilder();
            if (bulletCount != 0) sb.Append("�ӵ�+").Append(bulletCount).AppendLine();
            return str + string.Format("<color={0}>�ظ�Ч��</color>\n{1}", qs, sb);
        }
    }
}
