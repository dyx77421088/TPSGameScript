using UnityEngine;

namespace TPSShoot.Bags
{
    public class Material : Item
    {
        public Material(int id, string name, ItemType type, ItemQuality quality,
            string description, int capacity, int buyprice, int sellprice, string sprite)
            : base(id, name, type, quality, description, capacity, buyprice, sellprice, sprite)
        {
        }
    }
}
