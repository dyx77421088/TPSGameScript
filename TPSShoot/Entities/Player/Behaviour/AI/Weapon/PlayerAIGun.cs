using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public class PlayerAIGun : PlayerWeapon
    {
        [Header("�ⲿ���ã��ӵ�����Ҫ��")]
        public PlayerAIBehaviour playerAIBehaviour;
        [Header("�ӵ���ص�")]
        [Tooltip("�ӵ���prefab")]public GameObject bulletPrefab;
        [Tooltip("�ӵ�ʵ������λ��")]public Transform bulletPosition;

        [Header("ǹ��ص�")]
        [Tooltip("����ļ��ʱ��")]public float fireInterval = 0.4f;
        

        [Header("��Ч")]
        public PlayerWeaponSoundSettings weaponSoundSettings;


        public bool CanFire { get { return _canShoot; } private set { } }
        private bool _canShoot = true;
        private Vector3 _emptyFirePoint = new Vector3(0, 90, 0); // �յ��ӵ�����

        /// <summary>
        /// ����
        /// </summary>
        public void Fire(Vector3 position)
        {
            if (!_canShoot)
            {
                _canShoot = false;
                // ������
                Invoke("ShootInterval", 0.4f); // ���fireInterval���ִ��ShootInterval����
                weaponSoundSettings.Play(weaponSoundSettings.idleSound);
                return;
            }
            // ����
            weaponSoundSettings.Play(weaponSoundSettings.fireSound);
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
            var go = Instantiate(
                bulletPrefab,
                bulletPosition.position,
                bulletPosition.rotation
                );
            go.GetComponent<ProjectileMover>().PlayerB = playerAIBehaviour.aiAttribute;
        }

        /// <summary>
        /// ����ļ��
        /// </summary>
        private void ShootInterval()
        {
            _canShoot = true;
        }

    }
}