using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using TPSShoot.Manger;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// ��ɫʹ�õ������Ĳ���
    /// </summary>
    public partial class PlayerAIBehaviour
    {
        public bool IsFire { get;  set; } // ����
        public bool IsReload { get; private set;} // ���ӵ�
        public bool IsWeapingWeapon { get; private set;} // ������
        public bool IsNoWeapon { get; private set; } // û��������״̬
        public bool IsGunWeapon { get; private set; } // ǹ��״̬
        public bool IsSwordWeapon { get; private set; } // ����״̬
        public Vector3 FirePoint { get; private set; } // ����Ļ�е㷢�����߻��е�λ�ã��ӵ�ʵ������lookat��
        public Transform FireObject { get; private set; } // ����Ļ�е㷢�����߻��е�Ŀ��

        // ��ǰʹ�õ�����
        public PlayerWeapon CurrentWeapon { get { return weaponSettings.currentWeapon; } set { weaponSettings.currentWeapon = value; } } // ��ǰ��ɫʹ�õ�����
        public PlayerAIGun CurrentGun { get => (PlayerAIGun)CurrentWeapon; }
        public int currentWeaponIndex { get; private set; } = -1;

        private Coroutine isFireCoroutine;
        #region һЩ����,�л���������ǹ����
        /// <summary>
        /// ������
        /// </summary>
        public void OnSwapWeapon(int index)
        {
            // һЩ״̬�²��ܻ�����
            //if (IsReload) return;
            //if (IsJump) return;
            if (!IsAlive) return;
            SetCurrentWeapon(index);
        }
        /// <summary>
        /// ��ǹ������
        /// </summary>
        public void OnFireRequest(Vector3 point)
        {
            if (PlayerBagBehaviour.Instance.IsOpenBag) return;
            // ����Ƴ���Ϸ����������������
            //if (IsPauseGame()) return;
            if (!IsAlive) return;
            if (!IsGunWeapon) return;
            // һЩ״̬�²��ܿ�ǹ
            if (IsReload || IsWeapingWeapon || !CurrentWeapon || !CurrentGun.CanFire) return;
            if (isFireCoroutine != null) StopCoroutine(isFireCoroutine);
            // ���ڿ���
            isFireCoroutine = StartCoroutine(UpdateIsFire());

            Fire(point);
        }
        /// <summary>
        /// �����е�����
        /// </summary>
        private void OnReloadRequest()
        {
            if (!CurrentWeapon) return;
            if (CurrentWeapon is not PlayerGun) return;
            //if (!CurrentGun.CanReload) return;
            Reload();
        }
        #endregion

        #region �л�������ص�
        /// <summary>
        /// ��ʼ������
        /// </summary>
        private void InitWeapon()
        {
            //int weaponIndex = -1;
            // ����������������
            for (int i = 0; i < weaponSettings.allWeapon.Length; i++)
            {
                weaponSettings.allWeapon[i].gameObject.SetActive(false);
                //if (CurrentWeapon != null)
                //{
                //    // �����ǰ�������������е�����tag��ͬ�Ͱ�index��Ϊ��
                //    if (CurrentWeapon.CompareTag(weaponSettings.allWeapon[i].tag))
                //    {
                //        weaponIndex = i;
                //    }
                //}
            }
            // ��ǰ������
            //if (weaponIndex != -1)
            //{
            //    // �ó�����
            //    SetCurrentWeapon(weaponIndex);
            //}
            //else
            //{
            //    // ��������
            //    HideCurrentWeapon();
            //}
        }
        
        /// <summary>
        /// ���õ�ǰ������
        /// </summary>
        public void SetCurrentWeapon(int index)
        {
            // ���index�Ƿ�Ϸ�
            if (index < 0 || index >= weaponSettings.allWeapon.Length || weaponSettings.allWeapon[index] == null) { return; }

            // �����Ҫ���������͵�ǰ������index��ͬ�ͷŻ�����
            if (currentWeaponIndex == index)
            {
                // ��������
                HideCurrentWeapon();
            }
            else
            {
                // �ó�����
                ChangeCurrentWeapon(index);
            }
        }

        /// <summary>
        /// ���ص�ǰ������
        /// </summary>
        private void HideCurrentWeapon()
        {
            SetChangeWeaponAnimator(true);
        }
        
        /// <summary>
        /// �ı䵱ǰ������
        /// </summary>
        public void ChangeCurrentWeapon(int index)
        {
            currentWeaponIndex = index;
            SetChangeWeaponAnimator(false);
        }
        /// <summary>
        /// ���øı������Ķ���������
        /// </summary>
        private void SetChangeWeaponAnimator(bool isHide)
        {
            // ���ö���
            _animator.SetTrigger(PlayerAnimatorParameter.changeWeaponModeTrigger);
            // ���Ż����������� TODD

            // ���õ�ǰ״̬���Ƿ�Ϊ������״̬
            IsNoWeapon = isHide;
            // ��������״̬
            _animator.SetInteger(PlayerAnimatorParameter.weaponModeInt, isHide ? -1 : currentWeaponIndex);
        }

        #region ��������������event
        private void StartChangingWeapon()
        {
            IsWeapingWeapon = true;
        }
        /// <summary>
        /// �õ�������״̬
        /// </summary>
        private void UnequipEvent()
        {
            Debug.Log("������������");
            // ������
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
                    Debug.Log("�ó��˽�");
                    IsSwordWeapon = true;
                    // ��������ǽ�������Ҫ�л�����ͷ
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

        #region ��ǹ��ص�
        private bool IsPauseGame()
        {
            if (GameManager.Instance.IsGamePause)
            {
                Debug.Log("������������Ϸ");
                //Events.GameResumeRequest.Call();
                return true;
            }
            else if (Cursor.visible)
            {
                Debug.Log("����");
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            return false;
        }
        private void Fire(Vector3 point)
        {
            CurrentGun.Fire(point);
            // ��ɫ�����ˣ�һЩ����ִ�У������޸��ӵ�����ui��
            //Events.PlayerFire.Call();
            // �����ǰ����Ϊ0������Ҫ����
            //if (CurrentGun.currentBullet == 0)
            //{
                
            //    // ��������صĶ���ִ��
            //    //Events.ReloadRequest.Call();
            //}
        }
        /// <summary>
        /// ��֡�������ڿ����״̬
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

        #region ��������ص�
        private void Reload()
        {
            IsReload = true;
            // �����ж���
            _animator.SetTrigger(PlayerAnimatorParameter.reloadTrigger);
            // ����
            //CurrentGun.Reload();
        }
        /// <summary>
        /// ��ɻ����������event
        /// </summary>
        private void FinishedReloading()
        {
            IsReload = false;
            // ��ɻ����еĶ���
            //CurrentGun.Reloaded();
            // �����������һЩ����ִ�У������޸�ui
            //Events.PlayerReloaded.Call();
        }

        #endregion
    }
}
