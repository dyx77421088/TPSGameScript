using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public class PlayerGun : PlayerWeapon
    {
        [Header("�ӵ���ص�")]
        [Tooltip("�ӵ���prefab")]public GameObject bulletPrefab;
        [Tooltip("�ӵ�ʵ������λ��")]public Transform bulletPosition;

        [Header("ǹ��ص�")]
        [Tooltip("��ǰ���ӵ�")]public int currentBullet = 30;
        [Tooltip("һ�����е��ӵ���")]public int bulletClip = 30;
        [Tooltip("�ӵ��Ŀ��")]public int bulletsAmount = 120;
        [Tooltip("����ļ��ʱ��")]public float fireInterval = 0.4f;

        [Header("��׼")]
        public WeaponScopeSettings weaponScopeSettings;


        

        [Header("��Ч")]
        public PlayerWeaponSoundSettings weaponSoundSettings;

        // ��ǰ״̬�Ƿ��ܻ�����
        public bool CanReload { get { return _canReload && currentBullet < bulletClip && bulletsAmount > 0; } private set { } }
        // ��ǰ״̬�Ƿ��ܿ���
        public bool CanFire {  get { return _canShoot && currentBullet > 0; } private set { } }
        public bool IsReloading {  get { return _isReloading; } private set { } }


        private bool _canShoot = true;
        private bool _canReload = true;
        private bool _isReloading = false;
        private Vector3 _emptyFirePoint = new Vector3(0, 90, 0); // �յ��ӵ�����

        /// <summary>
        /// ����
        /// </summary>
        public void Fire(Vector3 position)
        {
            if (currentBullet <= 0 || !_canShoot)
            {
                _canShoot = false;
                // ������
                Invoke("ShootInterval", 0.4f); // ���fireInterval���ִ��ShootInterval����
                weaponSoundSettings.Play(weaponSoundSettings.idleSound);
                return;
            }
            // ����
            weaponSoundSettings.Play(weaponSoundSettings.fireSound);
            // �ӵ�������
            currentBullet--; // ������PlayerBehaviour.weapon��
            _canShoot = false;
            Invoke("ShootInterval", fireInterval); // ���fireInterval���ִ��ShootInterval����
            // �ӵ�����Ŀ��
            if (position != Vector3.zero)
            {
                bulletPosition.LookAt(position);
            }
            else
            {
                bulletPosition.eulerAngles = _emptyFirePoint;
            }

            // ʵ�����ӵ�
            Instantiate(
                bulletPrefab,
                bulletPosition.position,
                bulletPosition.rotation
                );
        }

        /// <summary>
        /// ����ļ��
        /// </summary>
        private void ShootInterval()
        {
            _canShoot = true;
        }
        /// <summary>
        /// ������
        /// </summary>
        public void Reload()
        {
            _canReload = false;
            _canShoot = false;
            _isReloading = true;
            weaponSoundSettings.Play(currentBullet == 0 ? weaponSoundSettings.reloadExhaustSound : weaponSoundSettings.reloadSound);
        }
        /// <summary>
        /// �����н���
        /// </summary>
        public void Reloaded()
        {
            // ��Ҫ��ӵ�����
            int addCount = Mathf.Min(bulletClip - currentBullet, bulletsAmount);
            currentBullet += addCount;
            bulletsAmount -= addCount;

            _canReload = true;
            _canShoot = true;

            _isReloading = false;
        }

        /// <summary>
        /// �һ���׼��һЩ����
        /// </summary>
        [Serializable]
        public class WeaponScopeSettings
        {
            [Tooltip("��׼��λ��")]public Transform scopePosition;
        }

    }
}