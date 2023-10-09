using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public class PlayerGun : PlayerWeapon
    {
        [Header("子弹相关的")]
        [Tooltip("子弹的prefab")]public GameObject bulletPrefab;
        [Tooltip("子弹实例化的位置")]public Transform bulletPosition;

        [Header("枪相关的")]
        [Tooltip("当前的子弹")]public int currentBullet = 30;
        [Tooltip("一个弹夹的子弹量")]public int bulletClip = 30;
        [Tooltip("子弹的库存")]public int bulletsAmount = 120;
        [Tooltip("开火的间隔时间")]public float fireInterval = 0.4f;

        [Header("瞄准")]
        public WeaponScopeSettings weaponScopeSettings;


        

        [Header("音效")]
        public PlayerWeaponSoundSettings weaponSoundSettings;

        // 当前状态是否能换弹夹
        public bool CanReload { get { return _canReload && currentBullet < bulletClip && bulletsAmount > 0; } private set { } }
        // 当前状态是否能开火
        public bool CanFire {  get { return _canShoot && currentBullet > 0; } private set { } }
        public bool IsReloading {  get { return _isReloading; } private set { } }


        private bool _canShoot = true;
        private bool _canReload = true;
        private bool _isReloading = false;
        private Vector3 _emptyFirePoint = new Vector3(0, 90, 0); // 空的子弹朝向

        /// <summary>
        /// 开火
        /// </summary>
        public void Fire(Vector3 position)
        {
            if (currentBullet <= 0 || !_canShoot)
            {
                _canShoot = false;
                // 开火间隔
                Invoke("ShootInterval", 0.4f); // 间隔fireInterval秒后执行ShootInterval方法
                weaponSoundSettings.Play(weaponSoundSettings.idleSound);
                return;
            }
            // 音乐
            weaponSoundSettings.Play(weaponSoundSettings.fireSound);
            // 子弹数减少
            currentBullet--; // 换弹在PlayerBehaviour.weapon中
            _canShoot = false;
            Invoke("ShootInterval", fireInterval); // 间隔fireInterval秒后执行ShootInterval方法
            // 子弹朝向目标
            if (position != Vector3.zero)
            {
                bulletPosition.LookAt(position);
            }
            else
            {
                bulletPosition.eulerAngles = _emptyFirePoint;
            }

            // 实例化子弹
            Instantiate(
                bulletPrefab,
                bulletPosition.position,
                bulletPosition.rotation
                );
        }

        /// <summary>
        /// 射击的间隔
        /// </summary>
        private void ShootInterval()
        {
            _canShoot = true;
        }
        /// <summary>
        /// 换弹夹
        /// </summary>
        public void Reload()
        {
            _canReload = false;
            _canShoot = false;
            _isReloading = true;
            weaponSoundSettings.Play(currentBullet == 0 ? weaponSoundSettings.reloadExhaustSound : weaponSoundSettings.reloadSound);
        }
        /// <summary>
        /// 换弹夹结束
        /// </summary>
        public void Reloaded()
        {
            // 需要添加弹夹数
            int addCount = Mathf.Min(bulletClip - currentBullet, bulletsAmount);
            currentBullet += addCount;
            bulletsAmount -= addCount;

            _canReload = true;
            _canShoot = true;

            _isReloading = false;
        }

        /// <summary>
        /// 右击瞄准的一些设置
        /// </summary>
        [Serializable]
        public class WeaponScopeSettings
        {
            [Tooltip("瞄准的位置")]public Transform scopePosition;
        }

    }
}