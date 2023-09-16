using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 一些参数的设置
    /// </summary>
    
    public partial class TPSCamera
    {
        [Serializable]
        public class PlayerCameraSettings
        {
            [Header("站立时的角度设置")]
            [Tooltip("站立时最小的y轴方向的角度")] public float standMinAngle = -30;
            [Tooltip("站立时最大的y轴方向的角度")] public float standMaxAngle = 70;
            [Header("下蹲时的角度设置")]
            [Tooltip("下蹲时最小的y轴方向的角度")] public float crouchMinAngle = -30;
            [Tooltip("下蹲时最大的y轴方向的角度")] public float crouchMaxAngle = 55;
        }


    }
}
