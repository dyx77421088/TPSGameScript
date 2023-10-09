using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public class PlayerAIGun : PlayerWeapon
    {
        [Header("外部好拿，子弹那里要拿")]
        public PlayerAIBehaviour playerAIBehaviour;
        [Header("子弹相关的")]
        [Tooltip("子弹的prefab")]public GameObject bulletPrefab;
        [Tooltip("子弹实例化的位置")]public Transform bulletPosition;

        [Header("枪相关的")]
        [Tooltip("开火的间隔时间")]public float fireInterval = 0.4f;
        

        [Header("音效")]
        public PlayerWeaponSoundSettings weaponSoundSettings;


        public bool CanFire { get { return _canShoot; } private set { } }
        private bool _canShoot = true;
        private Vector3 _emptyFirePoint = new Vector3(0, 90, 0); // 空的子弹朝向

        /// <summary>
        /// 开火
        /// </summary>
        public void Fire(Vector3 position)
        {
            if (!_canShoot)
            {
                _canShoot = false;
                // 开火间隔
                Invoke("ShootInterval", 0.4f); // 间隔fireInterval秒后执行ShootInterval方法
                weaponSoundSettings.Play(weaponSoundSettings.idleSound);
                return;
            }
            // 音乐
            weaponSoundSettings.Play(weaponSoundSettings.fireSound);
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
            var go = Instantiate(
                bulletPrefab,
                bulletPosition.position,
                bulletPosition.rotation
                );
            go.GetComponent<ProjectileMover>().PlayerB = playerAIBehaviour.aiAttribute;
        }

        /// <summary>
        /// 射击的间隔
        /// </summary>
        private void ShootInterval()
        {
            _canShoot = true;
        }

    }
}