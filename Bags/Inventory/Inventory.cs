using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
public class Inventory : MonoBehaviour
{
    protected Slot[] slots;
    public void Start()
    {
        slots = GetComponentsInChildren<Slot>();
    }

    /// <summary>
    /// �ŵ�slot�У��ɹ�����true
    /// </summary>
    public bool StoryItem(int id)
    {
        return StoryItem(InventoryManage.Instance.GetItemById(id));
    }
    /// <summary>
    /// �жϱ�λ���Ƿ�����з���
    /// </summary>
    private bool StoryItem(Item item)
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
                Debug.LogError("��Ʒ������!!!");
                return false;
            }
            //Debug.Log("slot = " + slot);
            //Debug.Log("item = " + item);
            // �����Ʒ
            slot.StorItem(item);
        } else
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
                if ( slot != null )
                {
                    slot.StorItem(item);
                } else
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
        foreach(Slot slot in slots)
        {
            if (slot.transform.childCount >= 1 &&  slot.GetId() == item.Id && !slot.IsFilled()) 
                return slot;
        }
        return null;
    }
}
