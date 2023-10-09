using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.Bags
{
    public partial class PlayerBagBehaviour
    {
        /// <summary>
        /// 装备
        /// </summary>
        public class Character : Inventory
        {
            PlayerBagBehaviour pbb;
            public Character(PlayerBagBehaviour pbb, GameObject slots) : base(slots) 
            { 
                this.pbb = pbb;
            }

            public void PutOnEquipt(ItemUi item, Equipment eq)
            {
                foreach (EquipSlot slot in slots)
                {
                    if (slot.equipmentType == eq.EquipType)
                    {
                        PutOn(item, slot);
                        return;
                    }
                }
            }

            public void PutOnWeapon(ItemUi item, Weapon w)
            {
                foreach (EquipSlot slot in slots)
                {
                    if (slot.weaponType == w.WType)
                    {
                        PutOn(item, slot);
                        return;
                    }
                }
            }

            private void PutOn(ItemUi item, EquipSlot slot)
            {
                // 如果已经有装备上了，就直接替换
                if (slot.transform.childCount > 1)
                {
                    ItemUi equipItemUi = slot.transform.GetChild(1).GetComponent<ItemUi>();
                    Item tempItem = item.item;
                    //int amount = equipItemUi.amount;

                    item.SetItem(equipItemUi.item);
                    equipItemUi.SetItem(tempItem);
                }
                else// 否则就直接穿上
                {
                    //int amount = equipItemUi.amount;
                    slot.StorItem(item.item);
                    Destroy(item.gameObject);
                }
                slot.TextActive(false);
            }

            /// <summary>
            /// 设置属性
            /// </summary>
            public void ComputeAttribute()
            {
                PlayerBehaviour.Attribute player = PlayerBehaviour.Instance.playerAttribute; 
                int strength = player.BaseStrength;
                int intellect = player.BaseIntellect;
                int agility = player.BaseAgility;
                int stamina = player.BaseStamina;
                int damage = 0;

                foreach (EquipSlot slot in slots)
                {
                    if (slot.transform.childCount <= 1) continue;
                    ItemUi itemUi = slot.transform.GetChild(1).GetComponent<ItemUi>();
                    if (itemUi.item is Weapon)
                    {
                        Weapon we = (itemUi.item as Weapon);
                        damage += we.Damage;
                        strength += we.Strength;
                        intellect += we.Intellect;
                        agility += we.Agility;
                        stamina += we.Stamina;
                    }
                    else
                    {
                        Equipment eq = itemUi.item as Equipment;
                        strength += eq.Strength;
                        intellect += eq.Intellect;
                        agility += eq.Agility;
                        stamina += eq.Stamina;
                    }
                }
                player.CurrentAgility = agility;
                player.CurrentIntellect = intellect;
                player.CurrentStamina = stamina;
                player.CurrentStrength = strength;
                player.Damage = damage;
                pbb.attributeText.text = player.GetAttributeString();
            }

            /// <summary>
            ///  卸下装备
            /// </summary>
            public void Unwield()
            {

            }

            /// <summary>
            /// 获得这个部位的装备的string
            /// </summary>
            public string GetItemTipByType(Equipment w)
            {
                foreach (EquipSlot slot in slots)
                {
                    if (w is Weapon)
                    {
                        Weapon weapon = (Weapon)w;
                        if (slot.weaponType == weapon.WType)
                        {
                            if (slot.transform.childCount <= 1) continue;
                            ItemUi itemUi = slot.transform.GetChild(1).GetComponent<ItemUi>();
                            return itemUi.item.TipShow();
                        }
                    }
                    else
                    {
                        if (slot.equipmentType == w.EquipType)
                        {
                            if (slot.transform.childCount <= 1) continue;
                            ItemUi itemUi = slot.transform.GetChild(1).GetComponent<ItemUi>();
                            return itemUi.item.TipShow();
                        }
                    }
                }
                return null;
            }
        }
    }
}
