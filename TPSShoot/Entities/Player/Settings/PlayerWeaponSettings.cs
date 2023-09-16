using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 角色武器设置
    /// </summary>
    [Serializable]
    public class PlayerWeaponSettings
    {
        [Tooltip("射击的层级")]public LayerMask shootMask;
        [Tooltip("当前的武器")]public PlayerWeapon currentWeapon;
        [Tooltip("所有武器")]public PlayerWeapon[] allWeapon;
    }
}
