using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/// <summary>
/// 格子
/// </summary>
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public GameObject itemPrefab;

    public virtual void Start()
    {
        
    }

    public int GetCount()
    {
        int count = 0;
        if (this is EquipSlot) count = 1;
        return count;
    }
    /// <summary>
    /// 储存物品
    /// </summary>
    /// <param name="item"></param>
    public void StorItem(Item item, int amount = 1)
    {
        
        if (transform.childCount == GetCount())
        {
            // 创建一个新物体
            GameObject go = Instantiate(itemPrefab);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.GetComponent<ItemUi>().SetItem(item, amount);
        } else
        {
            AddStorItem();
        }
    }
    public void AddStorItem()
    {
        // 数量加1
        transform.GetChild(GetCount()).GetComponent<ItemUi>().AddItem();
        
    }

    /// <summary>
    /// 判断是否已经到了叠加的最大数了
    /// </summary>
    /// <returns></returns>
    public bool IsFilled()
    {
        ItemUi itemUi = transform.GetChild(GetCount()).GetComponent<ItemUi>();
        return itemUi.item.Capacity <= itemUi.amount;
    }

    public int GetId()
    {
        ItemUi itemUi = transform.GetChild(GetCount()).GetComponent<ItemUi>();
        return itemUi.item.Id;
    }

    

    #region 鼠标放上和移出,点击
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
        if (transform.childCount > GetCount())
        {
            string info = transform.GetChild(GetCount()).GetComponent<ItemUi>().item.TipShow();
            InventoryManage.Instance.ShowTip(info);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
        InventoryManage.Instance.HideTip();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
        // 如果鼠标点击的是右键
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!InventoryManage.Instance.isDrag)
            {
                if (transform.childCount > 0)
                {
                    ItemUi itemUi = transform.GetChild(0).GetComponent<ItemUi>();
                    if (itemUi.item.Type == Item.ItemType.Consumable)
                    {
                        // 直接使用消耗品
                        itemUi.AddItem(-1);
                    } 
                    // 穿装备
                    Knapsack.Instance.PutOn(itemUi);
                }
            }
        } 
        else if (transform.childCount != 0)
        {
            ItemUi itemUi = transform.GetChild(0).GetComponent<ItemUi>();
            if (InventoryManage.Instance.isDrag) // 两个物品都有
            {
                // 取出拖拽的对象
                ItemUi dragItem = InventoryManage.Instance.dragItem;
                // 如果这两个对象的id是一样的，那就看能不能叠加
                if (itemUi.item.Id == dragItem.item.Id)
                {
                    // 1.如果按下了CTRL就一个一个的放入
                    int addAmount = dragItem.amount;
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        addAmount = 1;
                    }
                    if (addAmount + itemUi.amount > itemUi.item.Capacity) 
                        addAmount = itemUi.item.Capacity - itemUi.amount;
                    if (addAmount > 0)
                    {
                        itemUi.AddItem(addAmount);
                        if (dragItem.amount - addAmount <= 0)
                        {
                            InventoryManage.Instance.HideDrag();
                        } else
                        {
                            dragItem.AddItem(-addAmount);
                        }
                    }
                } 
                else
                {
                    // 替换(两都有目标)
                    Item tempUi = itemUi.item;
                    int amount = itemUi.amount;
                    itemUi.SetItem(dragItem.item, dragItem.amount);
                    InventoryManage.Instance.dragItem.SetItem(tempUi, amount);
                }

                
            } 
            else
            {
                // 跟随鼠标移动
                // 是否按ctrl键
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    int dragAmount = (itemUi.amount + 1) / 2;
                    if (dragAmount == itemUi.amount) Destroy(itemUi.gameObject);
                    else itemUi.AddItem(-dragAmount);
                    InventoryManage.Instance.ShowDrag(itemUi.item, dragAmount);
                }
                else
                {
                    InventoryManage.Instance.ShowDrag(itemUi.item, itemUi.amount);
                    Destroy(itemUi.gameObject);
                }
            }
            
        }else // 空物体，若本来drag有物品就放入这个空物体里
        {
            if (InventoryManage.Instance.isDrag)
            {
                // 是否按住ctrl键，按住的话就是一个一个方
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    // 空白地方放一个
                    StorItem(InventoryManage.Instance.dragItem.item);
                    InventoryManage.Instance.dragItem.AddItem(-1, true);
                    if (InventoryManage.Instance.dragItem.amount == 0) InventoryManage.Instance.HideDrag();
                }
                else
                {
                    // 拖拽中有物品,直接放入
                    StorItem(InventoryManage.Instance.dragItem.item, InventoryManage.Instance.dragItem.amount);
                    InventoryManage.Instance.HideDrag();
                }
            }
        }

        CharacterAttribute.Instance.showText();
    }
    #endregion
}
