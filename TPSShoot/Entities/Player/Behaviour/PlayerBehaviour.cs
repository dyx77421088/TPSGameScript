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
        
        public static PlayerBehaviour Instance { get { return instance; } }

        private void Awake()
        {
            instance = this;
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();

            // ��ʼ������
            InitWeapon();

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
            if (PlayerBagBehaviour.Instance.IsOpenBag)
            {
                _animator.enabled = false;
                return;
            }
            _animator.enabled = true;
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
        }

        

        private void OnAnimatorIK(int layerIndex)
        {
            // ���ֵ�ik
            UpdateLeftHandIk();
        }
    }

}