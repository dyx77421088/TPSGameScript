using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MyMonoInstance<UIManager>
{
    [Tooltip("ǹ��ͼƬ")][SerializeField] private Image gunUI;
    [Tooltip("������")][SerializeField] private Text cartridgeClip;


    /// <summary>
    /// ��ǰ�ӵ���ʣ�൯��
    /// </summary>
    /// <param name="currentBullet">��ǰ���ӵ���</param>
    /// <param name="remainClip">ʣ�൯��</param>
    public void SetCartridgeText(int currentBullet, int remainClip)
    {
        cartridgeClip.text = currentBullet + "/" + remainClip;
    }


}
