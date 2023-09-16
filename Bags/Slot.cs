using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/// <summary>
/// ����
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
    /// ������Ʒ
    /// </summary>
    /// <param name="item"></param>
    public void StorItem(Item item, int amount = 1)
    {
        
        if (transform.childCount == GetCount())
        {
            // ����һ��������
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
        // ������1
        transform.GetChild(GetCount()).GetComponent<ItemUi>().AddItem();
        
    }

    /// <summary>
    /// �ж��Ƿ��Ѿ����˵��ӵ��������
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

    

    #region �����Ϻ��Ƴ�,���
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
        // �������������Ҽ�
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!InventoryManage.Instance.isDrag)
            {
                if (transform.childCount > 0)
                {
                    ItemUi itemUi = transform.GetChild(0).GetComponent<ItemUi>();
                    if (itemUi.item.Type == Item.ItemType.Consumable)
                    {
                        // ֱ��ʹ������Ʒ
                        itemUi.AddItem(-1);
                    } 
                    // ��װ��
                    Knapsack.Instance.PutOn(itemUi);
                }
            }
        } 
        else if (transform.childCount != 0)
        {
            ItemUi itemUi = transform.GetChild(0).GetComponent<ItemUi>();
            if (InventoryManage.Instance.isDrag) // ������Ʒ����
            {
                // ȡ����ק�Ķ���
                ItemUi dragItem = InventoryManage.Instance.dragItem;
                // ��������������id��һ���ģ��ǾͿ��ܲ��ܵ���
                if (itemUi.item.Id == dragItem.item.Id)
                {
                    // 1.���������CTRL��һ��һ���ķ���
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
                    // �滻(������Ŀ��)
                    Item tempUi = itemUi.item;
                    int amount = itemUi.amount;
                    itemUi.SetItem(dragItem.item, dragItem.amount);
                    InventoryManage.Instance.dragItem.SetItem(tempUi, amount);
                }

                
            } 
            else
            {
                // ��������ƶ�
                // �Ƿ�ctrl��
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
            
        }else // �����壬������drag����Ʒ�ͷ��������������
        {
            if (InventoryManage.Instance.isDrag)
            {
                // �Ƿ�סctrl������ס�Ļ�����һ��һ����
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    // �հ׵ط���һ��
                    StorItem(InventoryManage.Instance.dragItem.item);
                    InventoryManage.Instance.dragItem.AddItem(-1, true);
                    if (InventoryManage.Instance.dragItem.amount == 0) InventoryManage.Instance.HideDrag();
                }
                else
                {
                    // ��ק������Ʒ,ֱ�ӷ���
                    StorItem(InventoryManage.Instance.dragItem.item, InventoryManage.Instance.dragItem.amount);
                    InventoryManage.Instance.HideDrag();
                }
            }
        }

        CharacterAttribute.Instance.showText();
    }
    #endregion
}
