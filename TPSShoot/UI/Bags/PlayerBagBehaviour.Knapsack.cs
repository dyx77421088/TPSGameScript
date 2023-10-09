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
        /// 背包里的
        /// </summary>
        public class Knapsack : Inventory
        {
            private PlayerBagBehaviour pbb;
            public Knapsack(PlayerBagBehaviour pbb, GameObject slots) : base(slots) 
            { 
                this.pbb = pbb;
            }

            /// <summary>
            /// 穿上装备
            /// </summary>
            public void PutOn(ItemUi item)
            {
                if (item.item.Grade > PlayerBehaviour.Instance.CurrentGrade)
                {
                    Events.PlayerInfoTipShow.Call("角色等级不够！", UI.PlayerInfoTipUI.PlayerInfoTipPoint.Center);
                    return;
                }
                if (item.item is Weapon)
                {
                    Weapon w = (Weapon)item.item;
                    
                    pbb._character.PutOnWeapon(item, w);
                }
                else if (item.item is Equipment)
                {
                    Equipment eq = (Equipment)item.item;
                    pbb._character.PutOnEquipt(item, eq);
                }
            }
            
        }
    }
}
