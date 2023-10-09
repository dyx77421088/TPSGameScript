using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// ��ɫʹ�õ������Ĳ���
    /// </summary>
    public partial class PlayerBehaviour
    {
        public bool IsAiming { get;  set; } // �Ƿ�����׼

        #region һЩ����
        /// <summary>
        /// ��׼
        /// </summary>
        private void OnAimingRequest()
        {
            if (PlayerBagBehaviour.Instance.IsOpenBag) return;
            // һЩ״̬�²��ܽ�����׼
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

        #region ��׼��ص�
        /// <summary>
        /// ����״̬
        /// </summary>
        private void AimingActive()
        {
            IsAiming = true;
            soundSettings.Play(soundSettings.aimActiveSound);
            Events.PlayerAimActive.Call();
        }
        
        /// <summary>
        /// �˳���׼״̬
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
