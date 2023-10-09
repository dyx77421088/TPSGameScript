using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// ik��ص�
    /// </summary>
    public partial class PlayerBehaviour
    {
        /// <summary>
        /// ��׵����ת
        /// </summary>
        private void UpdateSpineRotate()
        {
            // һЩ״̬�²���Ҫ�޸�ik
            bool isAimIk = NeedSpineIk();

            if (CurrentWeapon is PlayerGun)
            {
                // ��׼״̬
                _animator.SetBool(PlayerAnimatorParameter.isAimBool, isAimIk);
            }
            else
            {
                _animator.SetBool(PlayerAnimatorParameter.isAimBool, false);
            }
            if (!isAimIk) return;

            ikSettings.sprine.LookAt(ikSettings.lookAt);

            ikSettings.sprine.Rotate(ikSettings.sprineRotate);
        }
        private bool NeedSpineIk()
        {
            if (IsNoWeapon) return false;
            if (IsRuning) return false;
            if (IsReload) return false;
            if (IsSwordWeapon) return false;
            return true;
        }

        /// <summary>
        /// ����ik���ŵ�ǹ�ϣ�
        /// </summary>
        private void UpdateLeftHandIk()
        {
            // ��һЩ״̬�����ֲ���Ҫik
            if (IsWeapingWeapon || IsNoWeapon || IsReload || !CurrentWeapon)
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                return;
            }

            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

            if (CurrentWeapon is PlayerGun)
            {
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, CurrentWeapon.leftHandIk.position);
                _animator.SetIKRotation(AvatarIKGoal.LeftHand, CurrentWeapon.leftHandIk.rotation);
            }

        }
    }
}
