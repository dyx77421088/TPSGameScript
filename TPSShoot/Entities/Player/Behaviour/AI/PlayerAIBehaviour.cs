using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace TPSShoot
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent (typeof(CharacterController))]
    //[RequireComponent (typeof(NavMeshAgent))]
    public partial class PlayerAIBehaviour : MonoBehaviour
    {
        [Header("武器设置")]
        public PlayerWeaponSettings weaponSettings;
        public PlayerMoveSettings moveSettings; // 角色运动相关的设置
        public PlayerIkSettings ikSettings; // ik设置
        [Header("其它设置")]
        [Tooltip("战斗持续事件")]public float hitTime = 30;
        [Tooltip("向前发射的射线的位置")]public Transform forwardPosition;
        [Header("声音设置")]
        public PlayerSoundSettings soundSettings;
        [Header("AI属性设置")]
        public MonsterAttribute aiAttribute; // 角色的属性
        //public PlayerAttributteSettings attributteSettings;
        // 动画控制器
        private Animator _animator;
        // 角色控制器
        private CharacterController _characterController;
        private bool isPause;
        private bool isAlive = true; // 存活

        private bool isShowInfo; // 是否展示信息
        private bool isHit; // 是否受伤状态
        private Transform hitTransform; // 造成伤害的对象的transform
        public bool IsAlive { get { return isAlive; } }
        public bool IsShowInfo { get { return isShowInfo; } }
        public bool IsHit { get { return isHit; } }
        public Transform HitTransform { get { return hitTransform; } }
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            // 回血
            StartCoroutine(BloodReturn());
            // 初始化武器
            InitWeapon();
            // 初始化角色属性
            OnInitAttribute();
            // 初始化cd
            //InitCD();
            // 注册一些订阅
            SubScribe();

            _animator.SetFloat(PlayerAnimatorParameter.forwardFloat, 0);
            _animator.SetFloat(PlayerAnimatorParameter.rightFloat, 0);
        }

        private void OnDestroy()
        {
            // 注销订阅
            UnSubScribe();
        }



        private void Update()
        {
            if (isPause || !IsAlive) return;
            // 检测是否着地
            UpdateGround();
            //// 行走
            //UpdateWalk();
            //// 跑
            //UpdateRun();
            //// 根据角色当前状态改变运动速度
            //UpdateMovementSpeed();
            //// 子弹应该击中的点
            //UpdateFirePoint();
            //// 重力
            UpdateGravity();
            //Check();
        }

        // 在update之后执行，依赖于update更新后的数据
        private void LateUpdate()
        {
            if (isPause) return;
        }

        private void Check()
        {
            RaycastHit hit;
            if (Physics.Raycast(forwardPosition.position, forwardPosition.position, out hit, 2, LayerMask.GetMask(Layers.Terrain)))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
        /// <summary>
        /// 一些订阅
        /// </summary>
        private void SubScribe()
        {

            Events.GamePause += OnPause;
            Events.PlayerOpenBag += OnPause;
            Events.GameResume += OnResume;
            Events.PlayerCloseBag += OnResume;


        }
        /// <summary>
        /// 注销一些订阅
        /// </summary>
        private void UnSubScribe()
        {
            // 暂停游戏之类的
            Events.GamePause -= OnPause;
            Events.PlayerOpenBag -= OnPause;
            Events.GameResume -= OnResume;
            Events.PlayerCloseBag -= OnResume;
        }

        

        private void OnAnimatorIK(int layerIndex)
        {
            // 左手的ik
            UpdateLeftHandIk();
        }
        private Coroutine hitCor;
        public void OnHit(int grade, float attack = 0, float magicAttack = 0)
        {
            if (!IsAlive) return;
            if (hitCor != null) StopCoroutine(hitCor);
            hitCor = StartCoroutine(hitIE());
            Hit(grade, attack, magicAttack);
        }
        private IEnumerator hitIE()
        {
            isHit = true;
            hitTransform = PlayerBehaviour.Instance.transform;
            yield return new WaitForSeconds(hitTime);
            isHit = false;
            hitTransform = null;
            Events.TargetHide.Call(null, this);
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
    }

}