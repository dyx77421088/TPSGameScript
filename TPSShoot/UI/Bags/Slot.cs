using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/// <summary>
/// ����
/// </summary>
namespace TPSShoot.Bags
{
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
            }
            else
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
        public Item GetItem()
        {
            ItemUi itemUi = transform.GetChild(GetCount()).GetComponent<ItemUi>();
            return itemUi.item;
        }

        public virtual string ShowContrast(Item item)
        {
            if (item is Equipment)
            {
                Equipment equip = (Equipment)item;
                return PlayerBagBehaviour.Instance.GetCharacter().GetItemTipByType(equip);
            }
            return null;
        }

        #region �����Ϻ��Ƴ�,���
        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log("OnPointerEnter");
            if (transform.childCount > GetCount())
            {
                Item item = transform.GetChild(GetCount()).GetComponent<ItemUi>().item;
                string info = item.TipShow();
                Events.BagsShowTip.Call(info, ShowContrast(item));
            }

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log("OnPointerExit");
            Events.BagsHideTip.Call();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("OnPointerDown");
            // �������������Ҽ�
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (!PlayerBagBehaviour.Instance.isDrag)
                {
                    if (transform.childCount > 0)
                    {
                        ItemUi itemUi = transform.GetChild(0).GetComponent<ItemUi>();
                        if (itemUi.item.Type == Item.ItemType.Consumable)
                        {
                            Consumable consumable = (Consumable)itemUi.item;
                            // ֱ��ʹ������Ʒ
                            itemUi.AddItem(-1);
                            Events.PlayerReturnHPAndMP.Call(consumable.Hp, consumable.Mp);
                        }
                        else if (itemUi.item.Type == Item.ItemType.Bullet)
                        {
                            // �����ӵ�
                            Events.PlayerAddBulletAmount.Call(((Bullet)itemUi.item).BulletCount, ()=> itemUi.AddItem(-1));
                        }
                        else
                        {
                            // ��װ��
                            Events.BagsKnapsackPutOn.Call(itemUi);
                        }
                    }
                }
            }
            else if (transform.childCount != 0)
            {
                ItemUi itemUi = transform.GetChild(0).GetComponent<ItemUi>();
                if (PlayerBagBehaviour.Instance.isDrag) // ������Ʒ����
                {
                    // ȡ����ק�Ķ���
                    ItemUi dragItem = PlayerBagBehaviour.Instance.dragItem;
                    // ��������������id��һ���� ��Ʒ����һ���ģ��ǾͿ��ܲ��ܵ���
                    if (itemUi.item.Id == dragItem.item.Id && itemUi.item.Quality == dragItem.item.Quality)
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
                                PlayerBagBehaviour.Instance.HideDrag();
                            }
                            else
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
                        PlayerBagBehaviour.Instance.dragItem.SetItem(tempUi, amount);
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
                        PlayerBagBehaviour.Instance.ShowDrag(itemUi.item, dragAmount);
                    }
                    else
                    {
                        PlayerBagBehaviour.Instance.ShowDrag(itemUi.item, itemUi.amount);
                        Destroy(itemUi.gameObject);
                    }
                }

            }
            else // �����壬������drag����Ʒ�ͷ��������������
            {
                if (PlayerBagBehaviour.Instance.isDrag)
                {
                    // �Ƿ�סctrl������ס�Ļ�����һ��һ����
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        // �հ׵ط���һ��
                        StorItem(PlayerBagBehaviour.Instance.dragItem.item);
                        PlayerBagBehaviour.Instance.dragItem.AddItem(-1, true);
                        if (PlayerBagBehaviour.Instance.dragItem.amount == 0) PlayerBagBehaviour.Instance.HideDrag();
                    }
                    else
                    {
                        // ��ק������Ʒ,ֱ�ӷ���
                        StorItem(PlayerBagBehaviour.Instance.dragItem.item, PlayerBagBehaviour.Instance.dragItem.amount);
                        PlayerBagBehaviour.Instance.HideDrag();
                    }
                }
            }
            Events.BagsComputeAttribute.Call();
        }
        #endregion
    }
}
