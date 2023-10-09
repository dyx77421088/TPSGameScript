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
        /// ��ʼ����Щid
        /// </summary>
        private void OnInitItemTypeId(List<Item> items)
        {
            UnityEngine.Debug.Log("��ʼ��ʼ���ˣ�����");
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
        /// �������������͵�id
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
        private int count = 0; // ����Ĵ���
        /// <summary>
        /// ����� �������ĵȼ����� min-max�ȼ� ֮�� ��Ҫ�ٴ����
        /// </summary>
        /// <returns></returns>
        private Item RandomAttribute(Item.ItemType itemType, int id, int maxGrade)
        {
            count++;
            if (count > 50) return RandomAttribute(id, maxGrade);

            Item newItem = GetItemById(id).Copy();
            // �����ɱ��monster�ȼ����� װ������͵ȼ�����ô�������
            if (newItem.MinGrade > maxGrade) { return GetRandomItemByType(itemType, maxGrade); }

            // ����Ҳ�������
            newItem.Grade = RandomUtils.RandomInt(Mathf.Min(newItem.MinGrade, maxGrade), Mathf.Min(newItem.MaxGrade, maxGrade));

            return RandomAttribute(newItem);
        }
        /// <summary>
        /// item�������
        /// </summary>
        /// <returns></returns>
        private Item RandomAttribute(int id, int maxGrade)
        {
            //Item item = GetItemById(id);
            Item newItem = GetItemById(id).Copy();
            // ����Ҳ�������
            newItem.Grade = RandomUtils.RandomInt(Mathf.Min(newItem.MinGrade, maxGrade), Mathf.Min(newItem.MaxGrade, maxGrade));
            return RandomAttribute(newItem);
        }

        private Item RandomAttribute(Item newItem)
        {
            // Ʒ��Ҳ�������
            if (newItem is Equipment || newItem is Bullet)
            {
                newItem.Quality = RandomUtils.RandomQuality();
            }
            if (newItem is Weapon)
            {
                UnityEngine.Debug.Log("��weapon");
                Weapon newWeapon = (Weapon)newItem;
                newWeapon.Strength = RandomUtils.RandomAttribute(newWeapon.Strength, _increase, _reduce);
                newWeapon.Intellect = RandomUtils.RandomAttribute(newWeapon.Intellect, _increase, _reduce);
                newWeapon.Agility = RandomUtils.RandomAttribute(newWeapon.Agility, _increase, _reduce);
                newWeapon.Stamina = RandomUtils.RandomAttribute(newWeapon.Stamina, _increase, _reduce);
                newWeapon.Damage = RandomUtils.RandomAttribute(newWeapon.Damage, _increase, _reduce);
            }
            else if (newItem is Equipment)
            {
                UnityEngine.Debug.Log("��Equipment");
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
