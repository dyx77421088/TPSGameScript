using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Inventory
{
    private static Chest instance;
    public static Chest Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("Chest Slot").GetComponent<Chest>();
            }
            return instance;
        }
    }

    public void OpenOrHide()
    {
        transform.parent.gameObject.SetActive(!transform.parent.gameObject.activeSelf);
    }
}
