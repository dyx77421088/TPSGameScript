using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.Bags
{
    public partial class PlayerBagBehaviour
    {
        public class Inventory
        {
            protected Slot[] slots;
            public Inventory(GameObject slots)
            {
                this.slots = slots.GetComponentsInChildren<Slot>();
            }

            /// <summary>
            /// 放到slot中，成功返回true
            /// </summary>
            public bool StoryItem(int id)
            {
                return StoryItem(PlayerBagBehaviour.Instance.GetItemById(id));
            }
            /// <summary>
            /// 判断本位置是否可以有放入
            /// </summary>
            public bool StoryItem(Item item)
            {
                if (item == null)
                {
                    Debug.LogError("物品为空");
                    return false;
                }

                if (item.Capacity == 1) // 如果叠加上限为1，那么就直接找物品槽
                {
                    Slot slot = FindEmptySlot();
                    if (slot == null)
                    {
                        Debug.Log("物品槽已满!!!");
                        return false;
                    }
                    //Debug.Log("slot = " + slot);
                    //Debug.Log("item = " + item);
                    // 添加物品
                    slot.StorItem(item);
                }
                else
                {
                    // 是否可以叠加
                    Slot slot = FindSameTypeSlot(item);
                    if (slot != null)
                    {
                        slot.AddStorItem();
                    }
                    else
                    {
                        slot = FindEmptySlot();
                        if (slot != null)
                        {
                            slot.StorItem(item);
                        }
                        else
                        {
                            Debug.LogError("物品槽已满");
                            return false;
                        }
                    }
                }
                return true;
            }

            /// <summary>
            /// 找到一个空的物品槽
            /// </summary>
            /// <returns></returns>
            private Slot FindEmptySlot()
            {
                foreach (Slot slot in slots)
                {
                    if (slot.transform.childCount == 0) return slot;
                }
                return null;
            }

            /// <summary>
            /// 在slot中找相同的，用来叠加
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            private Slot FindSameTypeSlot(Item item)
            {
                foreach (Slot slot in slots)
                {
                    if (slot.transform.childCount >= 1 && slot.GetId() == item.Id
                        && slot.GetItem().Quality == item.Quality // 品质也要相同
                        && !slot.IsFilled())
                        return slot;
                }
                return null;
            }
        }
    }
}
