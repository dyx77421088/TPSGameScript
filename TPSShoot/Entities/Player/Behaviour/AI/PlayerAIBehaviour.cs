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
        [Header("��������")]
        public PlayerWeaponSettings weaponSettings;
        public PlayerMoveSettings moveSettings; // ��ɫ�˶���ص�����
        public PlayerIkSettings ikSettings; // ik����
        [Header("��������")]
        [Tooltip("ս�������¼�")]public float hitTime = 30;
        [Tooltip("��ǰ��������ߵ�λ��")]public Transform forwardPosition;
        [Header("��������")]
        public PlayerSoundSettings soundSettings;
        [Header("AI��������")]
        public MonsterAttribute aiAttribute; // ��ɫ������
        //public PlayerAttributteSettings attributteSettings;
        // ����������
        private Animator _animator;
        // ��ɫ������
        private CharacterController _characterController;
        private bool isPause;
        private bool isAlive = true; // ���

        private bool isShowInfo; // �Ƿ�չʾ��Ϣ
        private bool isHit; // �Ƿ�����״̬
        private Transform hitTransform; // ����˺��Ķ����transform
        public bool IsAlive { get { return isAlive; } }
        public bool IsShowInfo { get { return isShowInfo; } }
        public bool IsHit { get { return isHit; } }
        public Transform HitTransform { get { return hitTransform; } }
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            // ��Ѫ
            StartCoroutine(BloodReturn());
            // ��ʼ������
            InitWeapon();
            // ��ʼ����ɫ����
            OnInitAttribute();
            // ��ʼ��cd
            //InitCD();
            // ע��һЩ����
            SubScribe();

            _animator.SetFloat(PlayerAnimatorParameter.forwardFloat, 0);
            _animator.SetFloat(PlayerAnimatorParameter.rightFloat, 0);
        }

        private void OnDestroy()
        {
            // ע������
            UnSubScribe();
        }



        private void Update()
        {
            if (isPause || !IsAlive) return;
            // ����Ƿ��ŵ�
            UpdateGround();
            //// ����
            //UpdateWalk();
            //// ��
            //UpdateRun();
            //// ���ݽ�ɫ��ǰ״̬�ı��˶��ٶ�
            //UpdateMovementSpeed();
            //// �ӵ�Ӧ�û��еĵ�
            //UpdateFirePoint();
            //// ����
            UpdateGravity();
            //Check();
        }

        // ��update֮��ִ�У�������update���º������
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
        /// һЩ����
        /// </summary>
        private void SubScribe()
        {

            Events.GamePause += OnPause;
            Events.PlayerOpenBag += OnPause;
            Events.GameResume += OnResume;
            Events.PlayerCloseBag += OnResume;


        }
        /// <summary>
        /// ע��һЩ����
        /// </summary>
        private void UnSubScribe()
        {
            // ��ͣ��Ϸ֮���
            Events.GamePause -= OnPause;
            Events.PlayerOpenBag -= OnPause;
            Events.GameResume -= OnResume;
            Events.PlayerCloseBag -= OnResume;
        }

        

        private void OnAnimatorIK(int layerIndex)
        {
            // ���ֵ�ik
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