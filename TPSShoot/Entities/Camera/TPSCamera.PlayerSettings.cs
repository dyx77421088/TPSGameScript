using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// һЩ����������
    /// </summary>
    
    public partial class TPSCamera
    {
        [Serializable]
        public class PlayerCameraSettings
        {
            [Header("վ��ʱ�ĽǶ�����")]
            [Tooltip("վ��ʱ��С��y�᷽��ĽǶ�")] public float standMinAngle = -30;
            [Tooltip("վ��ʱ����y�᷽��ĽǶ�")] public float standMaxAngle = 70;
            [Header("�¶�ʱ�ĽǶ�����")]
            [Tooltip("�¶�ʱ��С��y�᷽��ĽǶ�")] public float crouchMinAngle = -30;
            [Tooltip("�¶�ʱ����y�᷽��ĽǶ�")] public float crouchMaxAngle = 55;
        }


    }
}
