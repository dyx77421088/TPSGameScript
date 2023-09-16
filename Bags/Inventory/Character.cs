using UnityEngine;
using UnityEngine.UI;

public class Character : Inventory
{
    //private EquipSlot[] eSlot;
    private static Character instance;
    public static Character Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("Character Slot").GetComponent<Character>();
            }
            return instance;
        }
    }

    //public new void Start()
    //{
    //    eSlot = GetComponentsInChildren<EquipSlot>();
    //}

    public void PutOnEquipt(ItemUi item, Equipment eq) 
    {
        foreach (EquipSlot slot in slots)
        {
            if (slot.equipmentType == eq.EquipType)
            {
                PutOn(item, slot);
                return;
            }
        }
    }

    public void PutOnWeapon(ItemUi item, Weapon w)
    {
        foreach (EquipSlot slot in slots)
        {
            if (slot.weaponType == w.WType)
            {
                PutOn(item, slot);
                return;
            }
        }
    }

    private void PutOn(ItemUi item, EquipSlot slot)
    {
        // ����Ѿ���װ�����ˣ���ֱ���滻
        if (slot.transform.childCount > 1)
        {
            ItemUi equipItemUi = slot.transform.GetChild(1).GetComponent<ItemUi>();
            Item tempItem = item.item;
            //int amount = equipItemUi.amount;

            item.SetItem(equipItemUi.item);
            equipItemUi.SetItem(tempItem);
        } 
        else// �����ֱ�Ӵ���
        {
            //int amount = equipItemUi.amount;
            slot.StorItem(item.item);
            Destroy(item.gameObject);
        }
        slot.TextActive(false);
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void SetAttribute(Text showText, int strength, int intellect, int agility, int stamina)
    {
        //int strength = player.BaseStrength;
        //int intellect = player.BaseIntellect;
        //int agility = player.BaseAgility;
        //int stamina = player.BaseStamina;
        int damage = 0;
        foreach(EquipSlot slot in slots)
        {
            if (slot.transform.childCount <= 1) continue;
            ItemUi itemUi = slot.transform.GetChild(1).GetComponent<ItemUi>();
            if (itemUi.item is Weapon)
            {
                damage += (itemUi.item as Weapon).Damage;
            }
            else
            {
                Equipment eq = itemUi.item as Equipment;
                strength += eq.Strength;
                intellect += eq.Intellect;
                agility += eq.Agility;
                stamina += eq.Stamina;
            }
        }
        showText.text = new Attribute(strength, intellect, agility, stamina, damage).GetAttributeString();
    }

    /// <summary>
    ///  ж��װ��
    /// </summary>
    public void Unwield()
    {

    }
}
