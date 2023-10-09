using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using TPSShoot.Manger;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 角色使用的武器的部分
    /// </summary>
    public partial class PlayerAIBehaviour
    {
        public bool IsFire { get;  set; } // 开火
        public bool IsReload { get; private set;} // 换子弹
        public bool IsWeapingWeapon { get; private set;} // 换武器
        public bool IsNoWeapon { get; private set; } // 没有武器的状态
        public bool IsGunWeapon { get; private set; } // 枪的状态
        public bool IsSwordWeapon { get; private set; } // 剑的状态
        public Vector3 FirePoint { get; private set; } // 从屏幕中点发射射线击中的位置，子弹实例化会lookat它
        public Transform FireObject { get; private set; } // 从屏幕中点发射射线击中的目标

        // 当前使用的武器
        public PlayerWeapon CurrentWeapon { get { return weaponSettings.currentWeapon; } set { weaponSettings.currentWeapon = value; } } // 当前角色使用的武器
        public PlayerAIGun CurrentGun { get => (PlayerAIGun)CurrentWeapon; }
        public int currentWeaponIndex { get; private set; } = -1;

        private Coroutine isFireCoroutine;
        #region 一些订阅,切换武器、开枪请求
        /// <summary>
        /// 换武器
        /// </summary>
        public void OnSwapWeapon(int index)
        {
            // 一些状态下不能换武器
            //if (IsReload) return;
            //if (IsJump) return;
            if (!IsAlive) return;
            SetCurrentWeapon(index);
        }
        /// <summary>
        /// 开枪的请求
        /// </summary>
        public void OnFireRequest(Vector3 point)
        {
            if (PlayerBagBehaviour.Instance.IsOpenBag) return;
            // 鼠标移出游戏点击后重新隐藏鼠标
            //if (IsPauseGame()) return;
            if (!IsAlive) return;
            if (!IsGunWeapon) return;
            // 一些状态下不能开枪
            if (IsReload || IsWeapingWeapon || !CurrentWeapon || !CurrentGun.CanFire) return;
            if (isFireCoroutine != null) StopCoroutine(isFireCoroutine);
            // 正在开火
            isFireCoroutine = StartCoroutine(UpdateIsFire());

            Fire(point);
        }
        /// <summary>
        /// 换弹夹的请求
        /// </summary>
        private void OnReloadRequest()
        {
            if (!CurrentWeapon) return;
            if (CurrentWeapon is not PlayerGun) return;
            //if (!CurrentGun.CanReload) return;
            Reload();
        }
        #endregion

        #region 切换武器相关的
        /// <summary>
        /// 初始化武器
        /// </summary>
        private void InitWeapon()
        {
            //int weaponIndex = -1;
            // 把所有武器都隐藏
            for (int i = 0; i < weaponSettings.allWeapon.Length; i++)
            {
                weaponSettings.allWeapon[i].gameObject.SetActive(false);
                //if (CurrentWeapon != null)
                //{
                //    // 如果当前武器和武器库中的武器tag相同就把index设为它
                //    if (CurrentWeapon.CompareTag(weaponSettings.allWeapon[i].tag))
                //    {
                //        weaponIndex = i;
                //    }
                //}
            }
            // 当前有武器
            //if (weaponIndex != -1)
            //{
            //    // 拿出武器
            //    SetCurrentWeapon(weaponIndex);
            //}
            //else
            //{
            //    // 隐藏武器
            //    HideCurrentWeapon();
            //}
        }
        
        /// <summary>
        /// 设置当前的武器
        /// </summary>
        public void SetCurrentWeapon(int index)
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
        public void ChangeCurrentWeapon(int index)
        {
            currentWeaponIndex = index;
            SetChangeWeaponAnimator(false);
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
            _animator.SetInteger(PlayerAnimatorParameter.weaponModeInt, isHide ? -1 : currentWeaponIndex);
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
            Debug.Log("进来换武器了");
            // 先隐藏
            if (CurrentWeapon)
            {
                CurrentWeapon.gameObject.SetActive(false);
                IsGunWeapon = IsSwordWeapon = false;
                //if (CurrentWeapon is PlayerGun)  Events.PlayerHideGunWeapon.Call();
                //else if(CurrentWeapon is PlayerSword) Events.PlayerHideSwordWeapon.Call();
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

                if (CurrentWeapon is PlayerAIGun)
                {
                    
                    IsGunWeapon = true;
                    //Events.PlayerShowGunWeapon.Call();    
                }
                else if (CurrentWeapon is PlayerAISword)
                {
                    Debug.Log("拿出了剑");
                    IsSwordWeapon = true;
                    // 如果武器是剑，则需要切换摄像头
                    //Events.PlayerShowSwordWeapon.Call();
                }
            }
            
        }
        private void FinishChangingWeapon()
        {
            IsWeapingWeapon = false;
        }
        #endregion
        #endregion

        #region 开枪相关的
        private bool IsPauseGame()
        {
            if (GameManager.Instance.IsGamePause)
            {
                Debug.Log("鼠标点击后继续游戏");
                //Events.GameResumeRequest.Call();
                return true;
            }
            else if (Cursor.visible)
            {
                Debug.Log("锁定");
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            return false;
        }
        private void Fire(Vector3 point)
        {
            CurrentGun.Fire(point);
            // 角色开火了，一些订阅执行（例如修改子弹数的ui）
            //Events.PlayerFire.Call();
            // 如果当前弹夹为0，则需要换弹
            //if (CurrentGun.currentBullet == 0)
            //{
                
            //    // 换弹夹相关的订阅执行
            //    //Events.ReloadRequest.Call();
            //}
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
            //CurrentGun.Reload();
        }
        /// <summary>
        /// 完成换弹动作后的event
        /// </summary>
        private void FinishedReloading()
        {
            IsReload = false;
            // 完成换弹夹的动作
            //CurrentGun.Reloaded();
            // 换弹结束后的一些订阅执行，例如修改ui
            //Events.PlayerReloaded.Call();
        }

        #endregion
    }
}
