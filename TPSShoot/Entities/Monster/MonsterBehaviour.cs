using System.Collections;
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

        // ״̬
        private MonsterBehaviourIdleStatus _idleStatus;
        private MonsterBehaviourAttackStatus _attackStatus;
        private MonsterBehaviourChaseStatus _chaseStatus;
        private MonsterBehaviourToBirthStatus _toBirthStatus;
        private MonsterBehaviourDiedStatus _diedStatus;
        private MonsterBehaviourStatus _status;

        // ί�ɣ�����������涨�壬��Ϊÿ��monster���Ƕ����ģ�
        public event System.Action onMonsterHPChange;
        public event System.Action onMonsterDied;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _playerBehaviour = PlayerBehaviour.Instance;

            _birthPoint = transform.position;

            _idleStatus = new MonsterBehaviourIdleStatus(this);
            _attackStatus = new MonsterBehaviourAttackStatus(this);
            _chaseStatus = new MonsterBehaviourChaseStatus(this);
            _toBirthStatus = new MonsterBehaviourToBirthStatus(this);
            _diedStatus = new MonsterBehaviourDiedStatus(this);

            _status = _idleStatus;
        }
        private void Update()
        {
            _status?.OnUpdate();
        }

        private void Awake()
        {
            onMonsterDied += OnDied;
        }
        private void OnDestroy()
        {
            onMonsterDied -= OnDied;
        }


        private void OnDied()
        {
            ChangeStatus(_diedStatus);
        }

        #region Ѫ��֮���
        private bool isHit;
        private Coroutine hitIe;
        /// <summary>
        /// ����
        /// </summary>
        public void OnChangeHP(int attack = 0, int magicAttack = 0)
        {
            // �ܵ��˺�
            if (hitIe != null) { StopCoroutine(hitIe); }
            hitIe = StartCoroutine(hitIE());

            OnHit(attack, magicAttack);
            
        }

        private IEnumerator hitIE()
        {
            isHit = true;
            yield return new WaitForSeconds(hitTime);
            isHit = false;
        }
        /// <summary>
        /// ��Ѫ
        /// </summary>
        private void OnAddHP(int add)
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

            return true;
        }
        /// <summary>
        /// �л�Ϊ׷Ѱ��ɫ״̬
        /// </summary>
        private bool CanChangeChase()
        {
            //Debug.Log(!IsAngleOfView() + " " + (GetSqrDistance() > distance) + " " + (GetSqrDistance() < attackOutSqrDistance));
            if (isHit) return true;
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
