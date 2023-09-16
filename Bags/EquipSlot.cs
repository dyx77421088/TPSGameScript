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
        // 如果鼠标点击的是右键
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 卸下装备
            if (!InventoryManage.Instance.isDrag)
            {
                if (transform.childCount > 1)
                {
                    ItemUi iui = transform.GetChild(1).GetComponent<ItemUi>();
                    // 放到背包里面
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
            // 首先判断大类是否正确,如果是材料之类的就直接返回了
            if (!(dragUi.item.Type == Item.ItemType.Weapon || dragUi.item.Type == Item.ItemType.Equipment))
            {
                // 类型不匹配，不能穿在角色上
                // 显示提示 TODD
                Debug.Log("类型不匹配!!");
                return;
            }

            // 如果是装备
            if (dragUi.item.Type == Item.ItemType.Equipment)
            {
                Equipment eq = dragUi.item as Equipment;
                if (eq.EquipType != equipmentType)
                {
                    Debug.Log("类型不匹配!!");
                    return;
                }
            } 
            else if (dragUi.item.Type == Item.ItemType.Weapon) // 如果是武器
            {
                Weapon eq = dragUi.item as Weapon;
                if (eq.WType != weaponType)
                {
                    Debug.Log("类型不匹配!!");
                    return;
                }
            }
            if (transform.childCount > 1 ) // 如果大于一就是有两个物品，一个是提示的名字，一个是图片
            {
                // 替换
                ItemUi itemUi = transform.GetChild(1).GetComponent<ItemUi>();

                Item tempUi = itemUi.item;
                int amount = itemUi.amount;
                itemUi.SetItem(dragUi.item, dragUi.amount);
                InventoryManage.Instance.ShowDrag(tempUi, amount);
            }
            else
            {
                // 直接放上去
                StorItem(dragUi.item, dragUi.amount);
                InventoryManage.Instance.HideDrag();
                // 文字隐藏
                TextActive(false);
            }
            
        }
        else // 把这个东西给拖拽
        {
            if (transform.childCount > 1)
            {
                ItemUi itemUi = transform.GetChild(1).GetComponent<ItemUi>();
                InventoryManage.Instance.ShowDrag(itemUi.item, 1);
                Destroy(itemUi.gameObject);
                // 显示文字
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
