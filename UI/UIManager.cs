using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MyMonoInstance<UIManager>
{
    [Tooltip("枪的图片")][SerializeField] private Image gunUI;
    [Tooltip("弹夹数")][SerializeField] private Text cartridgeClip;


    /// <summary>
    /// 当前子弹和剩余弹夹
    /// </summary>
    /// <param name="currentBullet">当前的子弹数</param>
    /// <param name="remainClip">剩余弹夹</param>
    public void SetCartridgeText(int currentBullet, int remainClip)
    {
        cartridgeClip.text = currentBullet + "/" + remainClip;
    }


}
