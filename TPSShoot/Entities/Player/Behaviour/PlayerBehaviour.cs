using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using UnityEngine;

namespace TPSShoot
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent (typeof(CharacterController))]
    public partial class PlayerBehaviour : MonoBehaviour
    {
        [Header("��������")]
        public PlayerWeaponSettings weaponSettings;
        public PlayerMoveSettings moveSettings; // ��ɫ�˶���ص�����
        public PlayerIkSettings ikSettings; // ik����

        [Header("��������")]
        public PlayerSoundSettings soundSettings;
        [Header("��ɫ��������")]
        public PlayerAttributteSettings attributteSettings;
        // ����������
        private Animator _animator;
        // ����
        private static PlayerBehaviour instance; 
        // ��ɫ������
        private CharacterController _characterController;
        private bool isPause;
        private bool isAlive = true; // ���
        public static PlayerBehaviour Instance { get { return instance; } }
        public bool IsAlive { get { return isAlive; } }
        private void Awake()
        {
            instance = this;
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();

            StartCoroutine(BloodReturn());
            // ��ʼ������
            InitWeapon();
            // ��ʼ����ɫ����
            OnInitAttribute();
            // ��ʼ��cd
            InitCD();
            // ע��һЩ����
            SubScribe();
        }

        private void OnDestroy()
        {
            // ע������
            UnSubScribe();
        }



        private void Update()
        {
            if (isPause) return;
            // ����Ƿ��ŵ�
            UpdateGround();
            // ����
            UpdateWalk();
            // ��
            UpdateRun();
            // ���ݽ�ɫ��ǰ״̬�ı��˶��ٶ�
            UpdateMovementSpeed();
            // �ӵ�Ӧ�û��еĵ�
            UpdateFirePoint();
            // ����
            UpdateGravity();
        }

        // ��update֮��ִ�У�������update���º������
        private void LateUpdate()
        {
            if (isPause) return;
            // ���¼�׵����ת
            UpdateSpineRotate();
        }
        /// <summary>
        /// һЩ����
        /// </summary>
        private void SubScribe()
        {
            Events.PlayerSwapWeapon += OnSwapWeapon;
            Events.ReloadRequest += OnReloadRequest;
            Events.FireRequest += OnFireRequest;
            Events.JumpRequest += OnJumpRequested;
            Events.AimRequest += OnAimingRequest;

            Events.GamePause += OnPause;
            Events.PlayerOpenBag += OnPause;
            Events.GameResume += OnResume;
            Events.PlayerCloseBag += OnResume;

            Events.PlayerReturnHPAndMP += OnAddHPAndMP;
            Events.PlayerAddBulletAmount += OnAddBulletAmount;

            Events.SwordAttackRequest += OnSwrodAttackRequest;
            Events.PlayerShowSwordWeapon += OnSwordShow;
            Events.SwordSkillAttackRequest += OnSwordSkillAttackRequest;

            // ����
            Events.PlayerKillMonster += OnPlayerKillMonster;
            Events.PlayerAddExp += OnPlayerAddExp;
            Events.PlayerUpgradeRequest += OnUpgradeRequest;

        }
        /// <summary>
        /// ע��һЩ����
        /// </summary>
        private void UnSubScribe()
        {
            Events.PlayerSwapWeapon -= OnSwapWeapon;
            Events.ReloadRequest -= OnReloadRequest;
            Events.FireRequest -= OnFireRequest;
            Events.JumpRequest -= OnJumpRequested;
            Events.AimRequest -= OnAimingRequest;
            // ��ͣ��Ϸ֮���
            Events.GamePause -= OnPause;
            Events.PlayerOpenBag -= OnPause;
            Events.GameResume -= OnResume;
            Events.PlayerCloseBag -= OnResume;
            // ui��ص�
            Events.PlayerReturnHPAndMP -= OnAddHPAndMP;
            Events.PlayerAddBulletAmount -= OnAddBulletAmount;
            // ����ص�
            Events.SwordAttackRequest -= OnSwrodAttackRequest;
            Events.PlayerShowSwordWeapon -= OnSwordShow;
            Events.SwordSkillAttackRequest -= OnSwordSkillAttackRequest;
            // ����
            Events.PlayerKillMonster -= OnPlayerKillMonster;
            Events.PlayerAddExp -= OnPlayerAddExp;
            Events.PlayerUpgradeRequest -= OnUpgradeRequest;
        }

        

        private void OnAnimatorIK(int layerIndex)
        {
            // ���ֵ�ik
            UpdateLeftHandIk();
        }

        private void OnPause()
        {
            isPause = true;
            _animator.enabled = false;
        }
        private void OnResume()
        {
            isPause = false;
            _animator.enabled = true;
        }

        private void OnAddHPAndMP(int hp, int mp)
        {
            AddHP(hp);
            AddMP(mp);
        }

        private void OnAddBulletAmount(int count, Action complete)
        {
            if (!IsGunWeapon)
            {
                Events.PlayerInfoTipShow.Call("����װ��ǹ", UI.PlayerInfoTipUI.PlayerInfoTipPoint.Center);
                return;
            }
            // �ɹ����ӵ�
            complete?.Invoke();
            CurrentGun.bulletsAmount += count;
        }
    }

}