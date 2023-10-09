using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// ik相关的
    /// </summary>
    public partial class PlayerBehaviour
    {
        /// <summary>
        /// 脊椎的旋转
        /// </summary>
        private void UpdateSpineRotate()
        {
            // 一些状态下不需要修改ik
            bool isAimIk = NeedSpineIk();

            if (CurrentWeapon is PlayerGun)
            {
                // 瞄准状态
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
        /// 左手ik（放到枪上）
        /// </summary>
        private void UpdateLeftHandIk()
        {
            // 在一些状态下左手不需要ik
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
