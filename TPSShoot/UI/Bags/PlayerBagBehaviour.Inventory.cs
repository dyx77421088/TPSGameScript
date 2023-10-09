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
            /// �ŵ�slot�У��ɹ�����true
            /// </summary>
            public bool StoryItem(int id)
            {
                return StoryItem(PlayerBagBehaviour.Instance.GetItemById(id));
            }
            /// <summary>
            /// �жϱ�λ���Ƿ�����з���
            /// </summary>
            public bool StoryItem(Item item)
            {
                if (item == null)
                {
                    Debug.LogError("��ƷΪ��");
                    return false;
                }

                if (item.Capacity == 1) // �����������Ϊ1����ô��ֱ������Ʒ��
                {
                    Slot slot = FindEmptySlot();
                    if (slot == null)
                    {
                        Debug.Log("��Ʒ������!!!");
                        return false;
                    }
                    //Debug.Log("slot = " + slot);
                    //Debug.Log("item = " + item);
                    // �����Ʒ
                    slot.StorItem(item);
                }
                else
                {
                    // �Ƿ���Ե���
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
                            Debug.LogError("��Ʒ������");
                            return false;
                        }
                    }
                }
                return true;
            }

            /// <summary>
            /// �ҵ�һ���յ���Ʒ��
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
            /// ��slot������ͬ�ģ���������
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            private Slot FindSameTypeSlot(Item item)
            {
                foreach (Slot slot in slots)
                {
                    if (slot.transform.childCount >= 1 && slot.GetId() == item.Id
                        && slot.GetItem().Quality == item.Quality // Ʒ��ҲҪ��ͬ
                        && !slot.IsFilled())
                        return slot;
                }
                return null;
            }
        }
    }
}
