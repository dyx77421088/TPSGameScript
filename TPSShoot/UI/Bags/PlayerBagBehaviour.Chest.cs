using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.Bags
{
    public partial class PlayerBagBehaviour
    {
        public class Chest : Inventory
        {
            private GameObject chest;
            public Chest(GameObject slots) : base(slots) 
            {
                chest = slots;
            }

            private static Chest instance;
            public static Chest Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = GameObject.Find("Chest Slot").GetComponent<Chest>();
                    }
                    return instance;
                }
            }

            public void OpenOrHide()
            {
                chest.transform.parent.gameObject.SetActive(!chest.transform.parent.gameObject.activeSelf);
            }
        }
    }
}
