using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipSlot : Slot
{
    public Equipment.EquipmentType equipmentType;
    public Weapon.WeaponType weaponType;

    private Text text;

    public override void Start()
    {
        base.Start();
        text = transform.GetChild(0).GetComponent<Text>();
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        // �������������Ҽ�
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // ж��װ��
            if (!InventoryManage.Instance.isDrag)
            {
                if (transform.childCount > 1)
                {
                    ItemUi iui = transform.GetChild(1).GetComponent<ItemUi>();
                    // �ŵ���������
                    if (Knapsack.Instance.StoryItem(iui.item.Id))
                    {
                        Destroy(iui.gameObject);
                        TextActive(true);
                    }
                }
                
            }
        }else if (InventoryManage.Instance.isDrag)
        {
            ItemUi dragUi = InventoryManage.Instance.dragItem;
            // �����жϴ����Ƿ���ȷ,����ǲ���֮��ľ�ֱ�ӷ�����
            if (!(dragUi.item.Type == Item.ItemType.Weapon || dragUi.item.Type == Item.ItemType.Equipment))
            {
                // ���Ͳ�ƥ�䣬���ܴ��ڽ�ɫ��
                // ��ʾ��ʾ TODD
                Debug.Log("���Ͳ�ƥ��!!");
                return;
            }

            // �����װ��
            if (dragUi.item.Type == Item.ItemType.Equipment)
            {
                Equipment eq = dragUi.item as Equipment;
                if (eq.EquipType != equipmentType)
                {
                    Debug.Log("���Ͳ�ƥ��!!");
                    return;
                }
            } 
            else if (dragUi.item.Type == Item.ItemType.Weapon) // ���������
            {
                Weapon eq = dragUi.item as Weapon;
                if (eq.WType != weaponType)
                {
                    Debug.Log("���Ͳ�ƥ��!!");
                    return;
                }
            }
            if (transform.childCount > 1 ) // �������һ������������Ʒ��һ������ʾ�����֣�һ����ͼƬ
            {
                // �滻
                ItemUi itemUi = transform.GetChild(1).GetComponent<ItemUi>();

                Item tempUi = itemUi.item;
                int amount = itemUi.amount;
                itemUi.SetItem(dragUi.item, dragUi.amount);
                InventoryManage.Instance.ShowDrag(tempUi, amount);
            }
            else
            {
                // ֱ�ӷ���ȥ
                StorItem(dragUi.item, dragUi.amount);
                InventoryManage.Instance.HideDrag();
                // ��������
                TextActive(false);
            }
            
        }
        else // �������������ק
        {
            if (transform.childCount > 1)
            {
                ItemUi itemUi = transform.GetChild(1).GetComponent<ItemUi>();
                InventoryManage.Instance.ShowDrag(itemUi.item, 1);
                Destroy(itemUi.gameObject);
                // ��ʾ����
                TextActive(true);
            }
            
        }
        CharacterAttribute.Instance.showText();
    }

    public void TextActive(bool active = false)
    {
        text.gameObject.SetActive(active);
    }
}
