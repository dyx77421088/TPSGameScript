using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 角色使用的武器的部分
    /// </summary>
    public partial class PlayerBehaviour
    {
        public bool IsAiming { get;  set; } // 是否在瞄准

        #region 一些订阅
        /// <summary>
        /// 瞄准
        /// </summary>
        private void OnAimingRequest()
        {
            if (PlayerBagBehaviour.Instance.IsOpenBag) return;
            // 一些状态下不能进行瞄准
            if (!CurrentWeapon) return;
            if (IsJump) return;
            if (IsReload) return;
            if (IsWeapingWeapon) return;
            if (IsSwordWeapon) return;
            if (IsAiming)
            {
                AimingOut();
            }
            else
            {
                AimingActive();
            }
        }
        #endregion

        #region 瞄准相关的
        /// <summary>
        /// 激活状态
        /// </summary>
        private void AimingActive()
        {
            IsAiming = true;
            soundSettings.Play(soundSettings.aimActiveSound);
            Events.PlayerAimActive.Call();
        }
        
        /// <summary>
        /// 退出瞄准状态
        /// </summary>
        private void AimingOut()
        {
            IsAiming = false;
            soundSettings.Play(soundSettings.aimOutSound);
            Events.PlayerAimOut.Call();
        }

        #endregion

    }
}
