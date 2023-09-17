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
        [Header("武器设置")]
        public PlayerWeaponSettings weaponSettings;
        public PlayerMoveSettings moveSettings; // 角色运动相关的设置
        public PlayerIkSettings ikSettings; // ik设置

        [Header("声音设置")]
        public PlayerSoundSettings soundSettings;
        [Header("角色属性设置")]
        public PlayerAttributteSettings attributteSettings;
        // 动画控制器
        private Animator _animator;
        // 单例
        private static PlayerBehaviour instance; 
        // 角色控制器
        private CharacterController _characterController;
        
        public static PlayerBehaviour Instance { get { return instance; } }

        private void Awake()
        {
            instance = this;
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();

            // 初始化武器
            InitWeapon();

            // 注册一些订阅
            SubScribe();
        }

        private void OnDestroy()
        {
            // 注销订阅
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
            // 检测是否着地
            UpdateGround();
            // 行走
            UpdateWalk();
            // 跑
            UpdateRun();
            // 根据角色当前状态改变运动速度
            UpdateMovementSpeed();
            // 子弹应该击中的点
            UpdateFirePoint();
            // 重力
            UpdateGravity();
        }

        // 在update之后执行，依赖于update更新后的数据
        private void LateUpdate()
        {
            // 更新脊椎的旋转
            UpdateSpineRotate();
        }
        /// <summary>
        /// 一些订阅
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
        /// 注销一些订阅
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
            // 左手的ik
            UpdateLeftHandIk();
        }
    }

}