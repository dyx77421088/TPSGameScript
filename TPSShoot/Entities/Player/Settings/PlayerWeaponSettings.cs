using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// ��ɫ��������
    /// </summary>
    [Serializable]
    public class PlayerWeaponSettings
    {
        [Tooltip("����Ĳ㼶")]public LayerMask shootMask;
        [Tooltip("��ǰ������")]public PlayerWeapon currentWeapon;
        [Tooltip("��������")]public PlayerWeapon[] allWeapon;
    }
}
