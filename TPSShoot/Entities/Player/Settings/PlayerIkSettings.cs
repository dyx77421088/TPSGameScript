using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerIkSettings
    {
        [Tooltip("上半身的ik")]public Transform sprine;
        [Tooltip("朝向哪个")]public Transform lookAt;
        [Tooltip("偏移量，调整到合适的位置")]public Vector3 sprineRotate = new Vector3(6, 64, -6.4f);
    }
}
