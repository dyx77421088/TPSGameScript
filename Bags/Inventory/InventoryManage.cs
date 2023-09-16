using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Bags;

public class InventoryManage : MonoBehaviour 
{
    #region 单例
    private static InventoryManage instance;
    public static InventoryManage Instance { 
        get {
            if (instance == null) instance = GameObject.Find("Inventory Manage").GetComponent<InventoryManage>();
            return instance;
        }
    }
    #endregion
    [HideInInspector]
    public List<Item> items = new();
    /// <summary>
    /// 拖拽的对象
    /// </summary>
    [HideInInspector]
    public ItemUi dragItem;
    /// <summary>
    /// 是否有拖拽对象
    /// </summary>
    [HideInInspector]
    public bool isDrag;

    private TipTool tipTool;
    private bool isTipShow;
    //private Canvas canvas;
    private Vector3 transV = new Vector2(50, -30);
    void Start()
    {
        initStory();
        tipTool = GameObject.FindAnyObjectByType<TipTool>();
        dragItem = GameObject.Find("Drag").GetComponent<ItemUi>();
        HideDrag();
        //canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    
    void Update()
    {
        if (isTipShow)
        {
            //Vector2 v;
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out v);
            Vector3 v3 = Input.mousePosition;
            if (v3.y < 500) transV.y = 200;
            else transV.y = -30;
            tipTool.SetPosition(v3 + transV);
        }
        if (isDrag)
        {
            dragItem.SetPosition(Input.mousePosition);
        }
        if (isDrag && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            InventoryManage.instance.HideDrag();
        }
    }

    #region 初始化数据
    /// <summary>
    /// 初始化数据
    /// </summary>
    private void initStory()
    {
        JObject jo = JObject.Parse(Resources.Load<TextAsset>("Items").text);
        JArray ja =  JArray.Parse(jo["data"].ToString());
        foreach (JObject item in ja)
        {
            //Debug.Log(item["type"].ToString());
            Item.ItemType it = Enum.Parse<Item.ItemType>(item["type"].ToString());
            switch (it)
            {
                case Item.ItemType.Material:
                    Bags.Material mt = item.ToObject<Bags.Material>();
                    items.Add(mt);
                    break;
                case Item.ItemType.Consumable:
                    Consumable c = item.ToObject<Consumable>();
                    items.Add(c);
                    break;
                case Item.ItemType.Equipment:
                    Equipment eq = item.ToObject<Equipment>();
                    items.Add(eq);
                    break;
                case Item.ItemType.Weapon:
                    Weapon weapon = item.ToObject<Weapon>();
                    items.Add(weapon);
                    break;
            }
        }

        foreach (var item in items)
        {
            switch (item.Type)
            {
                case Item.ItemType.Material:
                    Bags.Material mt = item as Bags.Material;
                    break;
                case Item.ItemType.Consumable:
                    Consumable c = item as Consumable;
                    break;
                case Item.ItemType.Equipment:
                    Equipment eq = item as Equipment;
                    break;
                case Item.ItemType.Weapon:
                    Weapon weapon = item as Weapon;
                    break;
            }
        }
        //ja.ToList().ForEach(item => Debug.Log(item.ToObject<Item>()));
        //List<Item> it = jo["data"].ToObject<List<Item>>();
        //Debug.Log(items.Count);
    }
    #endregion


    public Item GetItemById(int id)
    {
        return items.Find(a=>a.Id == id);
    }

    public void ShowTip(string text)
    {
        isTipShow = true;
        tipTool.Show(text);
    }

    public void HideTip()
    {
        isTipShow = false;
        tipTool.Hide();
    }

    public void ShowDrag(Item item, int amount)
    {
        dragItem.Show(item, amount);
        isDrag = true;
    }

    public void HideDrag()
    {
        dragItem.Hide();
        isDrag = false;
    }
}
