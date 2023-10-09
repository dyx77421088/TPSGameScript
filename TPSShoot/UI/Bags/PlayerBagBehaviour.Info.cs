using System.Collections.Generic;
using System.Diagnostics;
using TPSShoot.Utils;
using UnityEngine;

namespace TPSShoot.Bags
{
    public partial class PlayerBagBehaviour
    {
        
        private List<int> consumableId = new List<int>();
        private List<int> equipmentId = new List<int>();
        private List<int> weaponId = new List<int>();
        private List<int> materialId = new List<int>();
        private List<int> bulletId = new List<int>();

        /// <summary>
        /// 初始化这些id
        /// </summary>
        private void OnInitItemTypeId(List<Item> items)
        {
            UnityEngine.Debug.Log("开始初始化了！！！");
            this._items = items;
            foreach (Item item in items)
            {
                switch (item.Type)
                {
                    case Item.ItemType.Consumable:
                        consumableId.Add(item.Id);
                        break;
                    case Item.ItemType.Equipment:
                        equipmentId.Add(item.Id);
                        break;
                    case Item.ItemType.Weapon:
                        weaponId.Add(item.Id);
                        break;
                    case Item.ItemType.Material:
                        materialId.Add(item.Id);
                        break;
                    case Item.ItemType.Bullet:
                        bulletId.Add(item.Id);
                        break;
                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// 随机返回这个类型的id
        /// </summary>
        public Item GetRandomItemByType(Item.ItemType itemType, int maxGrade)
        {
            UnityEngine.Debug.Log("===============");
            UnityEngine.Debug.Log(consumableId.Count);
            UnityEngine.Debug.Log(consumableId);
            switch (itemType)
            {
                case Item.ItemType.Consumable:
                    return RandomAttribute(consumableId[RandomUtils.RandomInt(0, consumableId.Count)], maxGrade);
                case Item.ItemType.Equipment:
                    return RandomAttribute(itemType, equipmentId[RandomUtils.RandomInt(0, equipmentId.Count)], maxGrade);
                case Item.ItemType.Weapon:
                    return RandomAttribute(itemType, weaponId[RandomUtils.RandomInt(0, weaponId.Count)], maxGrade);
                case Item.ItemType.Material:
                    return RandomAttribute(materialId[RandomUtils.RandomInt(0, materialId.Count)], maxGrade);
                case Item.ItemType.Bullet:
                    return RandomAttribute(bulletId[RandomUtils.RandomInt(0, bulletId.Count)], maxGrade);
                default:
                    return null;
            }
        }
        private int count = 0; // 随机的次数
        /// <summary>
        /// 这个是 如果随机的等级不在 min-max等级 之间 就要再次随机
        /// </summary>
        /// <returns></returns>
        private Item RandomAttribute(Item.ItemType itemType, int id, int maxGrade)
        {
            count++;
            if (count > 50) return RandomAttribute(id, maxGrade);

            Item newItem = GetItemById(id).Copy();
            // 如果击杀的monster等级低于 装备的最低等级，那么重新随机
            if (newItem.MinGrade > maxGrade) { return GetRandomItemByType(itemType, maxGrade); }

            // 级数也是随机的
            newItem.Grade = RandomUtils.RandomInt(Mathf.Min(newItem.MinGrade, maxGrade), Mathf.Min(newItem.MaxGrade, maxGrade));

            return RandomAttribute(newItem);
        }
        /// <summary>
        /// item随机属性
        /// </summary>
        /// <returns></returns>
        private Item RandomAttribute(int id, int maxGrade)
        {
            //Item item = GetItemById(id);
            Item newItem = GetItemById(id).Copy();
            // 级数也是随机的
            newItem.Grade = RandomUtils.RandomInt(Mathf.Min(newItem.MinGrade, maxGrade), Mathf.Min(newItem.MaxGrade, maxGrade));
            return RandomAttribute(newItem);
        }

        private Item RandomAttribute(Item newItem)
        {
            // 品质也是随机的
            if (newItem is Equipment || newItem is Bullet)
            {
                newItem.Quality = RandomUtils.RandomQuality();
            }
            if (newItem is Weapon)
            {
                UnityEngine.Debug.Log("是weapon");
                Weapon newWeapon = (Weapon)newItem;
                newWeapon.Strength = RandomUtils.RandomAttribute(newWeapon.Strength, _increase, _reduce);
                newWeapon.Intellect = RandomUtils.RandomAttribute(newWeapon.Intellect, _increase, _reduce);
                newWeapon.Agility = RandomUtils.RandomAttribute(newWeapon.Agility, _increase, _reduce);
                newWeapon.Stamina = RandomUtils.RandomAttribute(newWeapon.Stamina, _increase, _reduce);
                newWeapon.Damage = RandomUtils.RandomAttribute(newWeapon.Damage, _increase, _reduce);
            }
            else if (newItem is Equipment)
            {
                UnityEngine.Debug.Log("是Equipment");
                Equipment newEquipment = (Equipment)newItem;
                newEquipment.Strength = RandomUtils.RandomAttribute(newEquipment.Strength, _increase, _reduce);
                newEquipment.Intellect = RandomUtils.RandomAttribute(newEquipment.Intellect, _increase, _reduce);
                newEquipment.Agility = RandomUtils.RandomAttribute(newEquipment.Agility, _increase, _reduce);
                newEquipment.Stamina = RandomUtils.RandomAttribute(newEquipment.Stamina, _increase, _reduce);
            }
            return newItem;
        }


    }
}
