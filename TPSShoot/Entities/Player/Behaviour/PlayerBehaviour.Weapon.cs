using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 角色使用的武器的部分
    /// </summary>
    public partial class PlayerBehaviour
    {
        public bool IsFire { get;  set; } // 开火
        public bool IsReload { get; private set;} // 换子弹
        public bool IsWeapingWeapon { get; private set;} // 换武器
        public bool IsNoWeapon { get; private set; } // 没有武器的状态
        public Vector3 FirePoint { get; private set; } // 从屏幕中点发射射线击中的位置，子弹实例化会lookat它
        public Transform FireObject { get; private set; } // 从屏幕中点发射射线击中的目标

        // 当前使用的武器
        public PlayerWeapon CurrentWeapon { get { return weaponSettings.currentWeapon; } set { weaponSettings.currentWeapon = value; } } // 当前角色使用的武器
        public int currentWeaponIndex { get; private set; } = -1;

        private Coroutine isFireCoroutine;
        #region 一些订阅,切换武器、开枪请求
        /// <summary>
        /// 换武器
        /// </summary>
        private void OnSwapWeapon(int index)
        {
            // 一些状态下不能换武器
            if (IsReload) return;
            if (IsJump) return;

            SetCurrentWeapon(index);
        }
        /// <summary>
        /// 开枪的请求
        /// </summary>
        private void OnFireRequest()
        {
            // 一些状态下不能开枪
            if (IsReload || IsWeapingWeapon || !CurrentWeapon || !CurrentWeapon.CanFire) return;

            if (isFireCoroutine != null) StopCoroutine(isFireCoroutine);
            // 正在开火
            isFireCoroutine = StartCoroutine(UpdateIsFire());

            Fire();
        }
        /// <summary>
        /// 换弹夹的请求
        /// </summary>
        private void OnReloadRequest()
        {
            if (!CurrentWeapon) return;
            if (!CurrentWeapon.CanReload) return;
            Reload();
        }
        #endregion

        #region 切换武器相关的
        /// <summary>
        /// 初始化武器
        /// </summary>
        private void InitWeapon()
        {
            int weaponIndex = -1;
            // 把所有武器都隐藏
            for (int i = 0; i < weaponSettings.allWeapon.Length; i++)
            {
                weaponSettings.allWeapon[i].gameObject.SetActive(false);
                if (CurrentWeapon != null)
                {
                    // 如果当前武器和武器库中的武器tag相同就把index设为它
                    if (CurrentWeapon.CompareTag(weaponSettings.allWeapon[i].tag))
                    {
                        weaponIndex = i;
                    }
                }
            }
            // 当前有武器
            if (weaponIndex != -1)
            {
                // 拿出武器
                SetCurrentWeapon(weaponIndex);
            }
            else
            {
                // 隐藏武器
                HideCurrentWeapon();
            }
        }
        
        /// <summary>
        /// 设置当前的武器
        /// </summary>
        private void SetCurrentWeapon(int index)
        {
            // 检测index是否合法
            if (index < 0 || index >= weaponSettings.allWeapon.Length || weaponSettings.allWeapon[index] == null) { return; }

            // 如果需要换的武器和当前武器的index相同就放回武器
            if (currentWeaponIndex == index)
            {
                // 收起武器
                HideCurrentWeapon();
            }
            else
            {
                // 拿出武器
                ChangeCurrentWeapon(index);
            }
        }

        /// <summary>
        /// 隐藏当前的武器
        /// </summary>
        private void HideCurrentWeapon()
        {
            SetChangeWeaponAnimator(true);
        }
        
        /// <summary>
        /// 改变当前的武器
        /// </summary>
        private void ChangeCurrentWeapon(int index)
        {
            SetChangeWeaponAnimator(false);
            currentWeaponIndex = index;
        }
        /// <summary>
        /// 设置改变武器的动画和音乐
        /// </summary>
        private void SetChangeWeaponAnimator(bool isHide)
        {
            // 设置动画
            _animator.SetTrigger(PlayerAnimatorParameter.changeWeaponModeTrigger);
            // 播放换武器的音乐 TODD

            // 设置当前状态，是否为无武器状态
            IsNoWeapon = isHide;
            // 设置武器状态
            _animator.SetInteger(PlayerAnimatorParameter.weaponModeInt, isHide ? -1 : 0);
        }

        #region 回收武器动画的event
        private void StartChangingWeapon()
        {
            IsWeapingWeapon = true;
        }
        /// <summary>
        /// 拿到武器的状态
        /// </summary>
        private void UnequipEvent()
        {
            // 先隐藏
            if (CurrentWeapon)
            {
                CurrentWeapon.gameObject.SetActive(false);
            }
            if (IsNoWeapon)
            {
                currentWeaponIndex = -1;
                CurrentWeapon = null;
            }
            else
            {
                CurrentWeapon = weaponSettings.allWeapon[currentWeaponIndex];
                CurrentWeapon.gameObject.SetActive(true);
            }
            
        }
        private void FinishChangingWeapon()
        {
            IsWeapingWeapon = false;
        }
        #endregion
        #endregion

        #region 开枪相关的
        private void Fire()
        {
            CurrentWeapon.Fire(FirePoint);
            // 角色开火了，一些订阅执行（例如修改子弹数的ui）
            Events.PlayerFire.Call();
            // 如果当前弹夹为0，则需要换弹
            if (CurrentWeapon.currentBullet == 0)
            {
                
                // 换弹夹相关的订阅执行
                Events.ReloadRequest.Call();
            }
        }
        /// <summary>
        /// 两帧都是正在开火的状态
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateIsFire()
        {
            IsFire = true;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            IsFire = false;
        }

        private void UpdateFirePoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, weaponSettings.shootMask))
            {
                FirePoint = hit.point;
                FireObject = hit.collider.transform;
            }
            else
            {
                FirePoint = ikSettings.lookAt.transform.position;
                FireObject = null;
            }
        }
        #endregion

        #region 换弹夹相关的
        private void Reload()
        {
            IsReload = true;
            // 换弹夹动画
            _animator.SetTrigger(PlayerAnimatorParameter.reloadTrigger);
            // 换弹
            CurrentWeapon.Reload();
        }
        /// <summary>
        /// 完成换弹动作后的event
        /// </summary>
        private void FinishedReloading()
        {
            IsReload = false;
            // 完成换弹夹的动作
            CurrentWeapon.Reloaded();
            // 换弹结束后的一些订阅执行，例如修改ui
            Events.PlayerReloaded.Call();
        }

        #endregion
    }
}
