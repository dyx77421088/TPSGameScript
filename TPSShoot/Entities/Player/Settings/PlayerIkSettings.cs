using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerIkSettings
    {
        [Tooltip("�ϰ����ik")]public Transform sprine;
        [Tooltip("�����ĸ�")]public Transform lookAt;
        [Tooltip("ƫ���������������ʵ�λ��")]public Vector3 sprineRotate = new Vector3(6, 64, -6.4f);
    }
}
