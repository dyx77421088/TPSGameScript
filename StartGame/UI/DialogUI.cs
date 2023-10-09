using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public Text info;
    public Action onOkClick;
    public Action onNoClick;
    public void SetActive(bool active = true)
    {
        this.gameObject.SetActive(active);
    }
    public void SetText(string text)
    {
        info.text = text;
    }
    public void OnOkButton()
    {
        if (onOkClick == null) return;
        onOkClick();
        
        Destroy(this.gameObject);
    }

    public void OnNoButton()
    {
        if (onNoClick == null) return;
        onNoClick();

        Destroy(this.gameObject);
    }
}
