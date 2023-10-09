using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityCharacterController;
using System.Collections;
using TPSShoot.Bags;
using TPSShoot.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace TPSShoot
{
    /// <summary>
    /// ���������������������һЩ������
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public partial class MonsterBehaviour : MonoBehaviour
    {
        [Header("�۲쵽player��һЩ����")]
        [Tooltip("�������")]public float minDistance = 3 * 3;
        [Tooltip("����")]public float distance = 10 * 10;
        [Tooltip("�ӽ�")] public float playerRadius = 75;
        [Tooltip("���ĸ�λ�ÿ�ʼ�۲�")]public Transform visionPoint;
        [Tooltip("�۲�Ĳ�")]public LayerMask visionLayer = 1 << 0;

        [Header("������һЩ����")]
        [Tooltip("��ɵ��˺�")] public float damage = 10;
        [Tooltip("��������")] public float attackSqrDistance = 1f;
        [Tooltip("������������")] public float attackOutSqrDistance = 2 * 2f;
        [Tooltip("������Χ")] public float attackSphereRadius = 0.3f;
        [Tooltip("����ƫ��")] public Vector3 attackOffset = new Vector3(0, 1, 1);


        [Header("�����һЩ����")]
        [Tooltip("����״̬����ʱ��")] public float hitTime = 10;
        [Tooltip("���˶�����cd")] public float hitAnimCD = 5;
        [Tooltip("Ѱ·�ٶ�")] public float runSpeed = 5;
        [Tooltip("�ص���������ٶ�")] public float toBirthSpeed = 8;
        [Tooltip("����ʱ��")] public float dieTime = 8;
        public MonsterAttribute monsterAttribute = new MonsterAttribute();
        

        private CharacterController _characterController;
        private Animator _animator;
        private NavMeshAgent _agent;
        private PlayerBehaviour _playerBehaviour;

        private Vector3 _birthPoint; // ������
        // һЩ����
        private static readonly int _speedHash = Animator.StringToHash("Speed");
        private static readonly int _attackHash = Animator.StringToHash("Attack");
        private static readonly int _dieHash = Animator.StringToHash("Die");
        private static readonly int _hitHash = Animator.StringToHash("Hit");

        private bool isPause;
        // ״̬
        private MonsterBehaviourIdleStatus _idleStatus;
        private MonsterBehaviourAttackStatus _attackStatus;
        private MonsterBehaviourChaseStatus _chaseStatus;
        private MonsterBehaviourToBirthStatus _toBirthStatus;
        private MonsterBehaviourDiedStatus _diedStatus;
        private MonsterBehaviourHitStatus _hitStatus;
        private MonsterBehaviourStatus _status;

        // ί�ɣ�����������涨�壬��Ϊÿ��monster���Ƕ����ģ�
        public event System.Action onMonsterHPChange;
        public event System.Action onMonsterDied;
        public event System.Action onMonsterHitAnim;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();

            if (_playerBehaviour == null) _playerBehaviour = PlayerBehaviour.Instance;
            _birthPoint = transform.position;

            _idleStatus = new MonsterBehaviourIdleStatus(this);
            _attackStatus = new MonsterBehaviourAttackStatus(this);
            _chaseStatus = new MonsterBehaviourChaseStatus(this);
            _toBirthStatus = new MonsterBehaviourToBirthStatus(this);
            _diedStatus = new MonsterBehaviourDiedStatus(this);
            _hitStatus = new MonsterBehaviourHitStatus(this);

            _status = _idleStatus;
            // ��ʼ���ȼ�
            monsterAttribute.grade = RandomUtils.RandomInt(monsterAttribute.minGrade, monsterAttribute.maxGrade);
            // ��ʼ������
            monsterAttribute.StartInit();

        }
        private void Update()
        {
            if (isPause) return;
            _status?.OnUpdate();
            //UpdateGravity();
        }

        private void Awake()
        {
            onMonsterDied += OnDied;
            onMonsterHitAnim += OnHitAnim;
            Events.PlayerOpenBag += OnPause;
            Events.PlayerCloseBag += OnResume;
            Events.GamePause += OnPause;
            Events.GameResume += OnResume;

            Events.PlayerLoaded += OnPlayerLoaded;
        }
        private void OnDestroy()
        {
            onMonsterDied -= OnDied;
            onMonsterHitAnim -= OnHitAnim;
            Events.PlayerOpenBag -= OnPause;
            Events.PlayerCloseBag -= OnResume;
            Events.GamePause -= OnPause;
            Events.GameResume -= OnResume;
            Events.PlayerLoaded -= OnPlayerLoaded;
        }

        private void OnPlayerLoaded()
        {
            Debug.Log("������ֵ�ˣ�����");
            _playerBehaviour = PlayerBehaviour.Instance;
        }
        private void OnDied()
        {
            ChangeStatus(_diedStatus);
        }

        private void OnPause()
        {
            _animator.enabled = false;
            isPause = true;

            _agent.isStopped = true;
        }
        private void OnResume()
        {
            _animator.enabled = true;
            isPause = false;

            _agent.isStopped = false;
        }
        // hit����״̬
        private void OnHitAnim()
        {
            if (canHitAnim && !isDied)
            {
                StartCoroutine(hitAnimIE());
                _animator.SetTrigger(_hitHash);
                ChangeStatus(_hitStatus);
            }

        }
        private float _gravity;
        private bool _resetGravity;
        private void UpdateGravity()
        {
            Debug.Log("111");
            
            if (!_characterController.enabled ) return;
            if (_characterController.isGrounded)
            {
                _gravity = 50f;
                _resetGravity = false;
            }
            else
            {
                if (!_resetGravity)
                {
                    _gravity = 1.2f;
                    _resetGravity = true;
                }
                _gravity += Time.deltaTime * 9.8f;
            }
            Debug.Log("�����޸�������");
            Vector3 gravityV3 = new Vector3();
            gravityV3.y -= _gravity;
            _characterController.Move(gravityV3 * Time.deltaTime);
        }
        #region Ѫ��֮���
        private bool isHit;
        private bool isHitAnim;
        private bool canHitAnim = true; // �ܲ��ܲ���hit����
        private Coroutine hitIe;
        /// <summary>
        /// ����
        /// </summary>
        public void OnChangeHP(int grade, int attack = 0, int magicAttack = 0)
        {
            Debug.Log("����");
            // �ܵ��˺�
            if (hitIe != null) { StopCoroutine(hitIe); }
            hitIe = StartCoroutine(hitIE());
            
            OnHit(grade, attack, magicAttack);
            
        }

        private IEnumerator hitIE()
        {
            isHit = true;
            yield return new WaitForSeconds(hitTime);
            isHit = false;
            Events.TargetHide.Call(monsterAttribute, null);
        }
        private IEnumerator hitAnimIE()
        {
            canHitAnim = false; 
            isHitAnim = true;
            yield return new WaitForSeconds(2);
            isHitAnim = false; 
            yield return new WaitForSeconds(hitAnimCD);
            canHitAnim = true;

        }
        /// <summary>
        /// ��Ѫ
        /// </summary>
        private void OnAddHP(float add)
        {
            AddHP(add);
        }
        #endregion
        #region һЩ����
        /// <summary>
        /// ��ת��lookat�У�ƽ����
        /// </summary>
        private void LookAtLerp(Vector3 lookAt, float speed = 5)
        {
            Quaternion q = transform.rotation;
            transform.LookAt(lookAt);
            transform.rotation = Quaternion.Lerp(q, transform.rotation, speed * Time.deltaTime);
        }
        /// <summary>
        /// ֹͣѰ·
        /// </summary>
        private void StopNavAgent()
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
        }

        private void StartNavAgent(float speed, Vector3 target)
        {
            _agent.isStopped = false;
            _agent.speed = speed;
            if (target != Vector3.zero) _agent.destination = target;
        }
        #endregion
        #region һЩ���
        private bool cacheIsAngleOfView;
        private int chacheIsAngleOfViewFrame = -1;
        /// <summary>
        /// �Ƿ����ӽ���
        /// </summary>
        /// <returns></returns>
        private bool IsAngleOfView()
        {
            if (_playerBehaviour == null) return false;
            // �����ǰ֡�Ѿ��жϹ���
            if (chacheIsAngleOfViewFrame == Time.frameCount)
            {
                return cacheIsAngleOfView;
            }
            chacheIsAngleOfViewFrame = Time.frameCount;
            // monster ����ɫ�ķ��������
            Vector3 v3 = _playerBehaviour.transform.position - transform.position;
            // �����������ͽ�ɫ��ǰ�����γɵļн�С��playerRadius
            return cacheIsAngleOfView = Mathf.Abs(Vector3.Angle(transform.forward, v3)) <= playerRadius;
        }
        private bool cacheIsCanLookPlayer;
        private int chacheIsCanLookPlayerFrame = -1;
        /// <summary>
        /// �Ƿ��ܿ�����ɫ������ɫ���ϰ�����סʱӦ����false
        /// </summary>
        /// <returns></returns>
        private bool IsCanLookPlayer()
        {
            // �����ǰ֡�Ѿ��жϹ���
            if (chacheIsCanLookPlayerFrame == Time.frameCount)
            {
                return cacheIsCanLookPlayer;
            }
            chacheIsCanLookPlayerFrame = Time.frameCount;
            cacheIsCanLookPlayer = false;

            // ��ǰ����һ������
            Vector3 start = visionPoint.position;
            Vector3 dir = _playerBehaviour.transform.position + Vector3.up;
            RaycastHit hit;
            Debug.DrawLine(start, dir, Color.red);
            // ���������֮���Ƿ�����ײ��
            if (Physics.Linecast(start, dir, out hit, visionLayer))
            {
                if (hit.collider.gameObject.GetComponentInParent<PlayerBehaviour>() != null)
                {
                    cacheIsCanLookPlayer = true;
                }
            }
            return cacheIsCanLookPlayer;
        }
        
        /// <summary>
        /// ��ý�ɫ��monster֮��ľ���
        /// </summary>
        private float GetSqrDistance()
        {
            if (_playerBehaviour == null) return Mathf.Infinity;
            // ����ƽ�����룬���⿪ƽ��
            return Vector3.SqrMagnitude(transform.position - _playerBehaviour.transform.position);
        }
        /// <summary>
        /// ��ó������monster֮��ľ���
        /// </summary>
        private float GetBirthSqrDistance()
        {
            // ����ƽ�����룬���⿪ƽ��
            return Vector3.SqrMagnitude(transform.position - _birthPoint);
        }
        #endregion
        #region �Ƿ�����ı�״̬
        /// <summary>
        /// �ܷ�ı�״̬��idle
        /// </summary>
        private bool CanChangeIdle()
        {
            // �ܿ�����ɫ���ڷ�Χ��
            if (IsCanLookPlayer() && IsAngleOfView() && GetSqrDistance() < distance) return false;
            if (GetBirthSqrDistance() > 1f) return false;
            return true;
        }
        /// <summary>
        /// �л�Ϊ����
        /// </summary>
        private bool CanChangeAttack()
        {
            // ����Ƿ�����Ұ��
            if (!IsAngleOfView()) return false;
            // ����Ƿ��ڹ�����Χ��
            if (GetSqrDistance() > attackSqrDistance) return false;
            return true;
        }
        /// <summary>
        /// �л�Ϊ�ص�������״̬
        /// </summary>
        private bool CanChangeToBirth()
        {
            // �ܿ�����ɫ���ڷ�Χ��
            if (IsCanLookPlayer() && IsAngleOfView() && GetSqrDistance() < distance) return false;
            if (GetSqrDistance() < minDistance) return false;
            return true;
        }
        /// <summary>
        /// �л�Ϊ׷Ѱ��ɫ״̬
        /// </summary>
        private bool CanChangeChase()
        {
            if (IsAttacking) return false;
            if (isHit) return true;
            if (GetSqrDistance() < minDistance) return true; // ��ɫ��̫���˻ᱻ���
            // �����ӽǷ�Χ��
            if (!IsAngleOfView()) return false;
            // player �� monster �ľ���
            if (GetSqrDistance() > distance) return false;
            // ������Χ��ֹ����
            if (GetSqrDistance() < attackOutSqrDistance) return false;

            return true;

        }
        /// <summary>
        /// �л�״̬
        /// </summary>
        private void ChangeStatus(MonsterBehaviourStatus status)
        {
            if (status == null) return;
            if (status == _status) return;
            _status?.OnExit();
            _status = status;
            _status.OnEnter();
        }
        #endregion

        #region ������event
        /// <summary>
        /// ����
        /// </summary>
        private void OnAttack()
        {
            // ����һ�����壬��������ײ��
            Collider[] coolider = Physics.OverlapSphere(
                transform.position + transform.rotation * attackOffset,
                attackSphereRadius,
                LayerMask.GetMask(Layers.Player)
                );

            foreach (Collider c in coolider)
            {
                c.GetComponent<PlayerBehaviour>().OnHit(monsterAttribute.aggressivity);
            }
        }
        private void OnAttackStart()
        {
            if (IsAttacking == false) StartCoroutine(AttackFinish());
        }
        private void OnAttackFinish()
        {
        }
        private IEnumerator AttackFinish()
        {
            IsAttacking = true;
            yield return new WaitForSeconds(2);
            IsAttacking = false;
        }
        #endregion
        public class MonsterBehaviourStatus
        {
            public MonsterBehaviour mb;
            public MonsterBehaviourStatus(MonsterBehaviour mb)
            {
                this.mb = mb;
            }
            public virtual void OnEnter() { }

            public virtual void OnExit() { }

            public virtual void OnUpdate() { }
        }
    }
}
