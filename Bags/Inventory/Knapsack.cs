using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 管理背包
/// </summary>
public class Knapsack : Inventory
{
    public GameObject character;
    public GameObject attribute;
    public Transform bagMoveTo;

    private bool showBag = false;

    private CanvasGroup _characterCanvas;
    private CanvasGroup _attributeCanvas;

    private void Awake()
    {
        _attributeCanvas = attribute.GetComponent<CanvasGroup>();
        _characterCanvas = character.GetComponent<CanvasGroup>();
    }
    private static Knapsack instance;
    public static Knapsack Instance 
    { 
        get
        {
            if (instance == null) 
            { 
                instance = GameObject.Find("Bag Slot").GetComponent<Knapsack>();
            }
            return instance;
        }
    }


    //public void OpenOrHideBag()
    //{
    //    if (showBag)
    //    {
    //        //bagAnimator.SetTrigger("NoBag");
    //        //transform.DOMove(new Vector3(100, -300), 2);
    //    }
    //    else
    //    {
    //        //bagAnimator.SetTrigger("Bag");
    //        //transform.DOMove(new Vector3(-100, 300), 2);
    //    }
    //    showBag = !showBag;
    //}

    //public bool IsOpenBag()
    //{
    //    return showBag;
    //}

    public void NoAttributeAndCharacter()
    {
    }
    public void OnClickAttribute()
    {
        _attributeCanvas.DOFade(1, 1);
        _characterCanvas.DOFade(0, 1);
        // 重新计算以下属性
        CharacterAttribute.Instance.showText();
    }

    public void OnClickCharacter()
    {
        _attributeCanvas.DOFade(0, 1);
        _characterCanvas.DOFade(1, 1);
    }

    /// <summary>
    /// 穿上装备
    /// </summary>
    public void PutOn(ItemUi item)
    {
        
        if (item.item is Weapon)
        {
            Weapon w = (Weapon)item.item;

            Character.Instance.PutOnWeapon(item, w);
        } 
        else if (item.item is Equipment)
        {
            Equipment eq = (Equipment)item.item;
            Character.Instance.PutOnEquipt(item, eq);
        }
    }
}
