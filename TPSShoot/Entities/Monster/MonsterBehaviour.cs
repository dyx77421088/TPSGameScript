using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace TPSShoot
{
    /// <summary>
    /// 这个怪物的类就是用来接收一些参数的
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public partial class MonsterBehaviour : MonoBehaviour
    {
        [Header("观察到player的一些属性")]
        [Tooltip("距离")]public float distance = 10 * 10;
        [Tooltip("视角")] public float playerRadius = 75;
        [Tooltip("从哪个位置开始观察")]public Transform visionPoint;
        [Tooltip("观察的层")]public LayerMask visionLayer = 1 << 0;

        [Header("攻击的一些设置")]
        [Tooltip("造成的伤害")] public float damage = 10;
        [Tooltip("攻击距离")] public float attackSqrDistance = 1f;
        [Tooltip("攻击超出距离")] public float attackOutSqrDistance = 2 * 2f;
        [Tooltip("攻击范围")] public float attackSphereRadius = 0.3f;
        [Tooltip("攻击偏移")] public Vector3 attackOffset = new Vector3(0, 1, 1);


        [Header("怪物的一些属性")]
        [Tooltip("受伤状态持续时间")] public float hitTime = 10;
        [Tooltip("寻路速度")] public float runSpeed = 5;
        [Tooltip("回到出生点的速度")] public float toBirthSpeed = 8;
        [Tooltip("死亡时间")] public float dieTime = 8;
        public MonsterAttribute monsterAttribute = new MonsterAttribute();


        private CharacterController _characterController;
        private Animator _animator;
        private NavMeshAgent _agent;
        private PlayerBehaviour _playerBehaviour;

        private Vector3 _birthPoint; // 出生点
        // 一些动画
        private static readonly int _speedHash = Animator.StringToHash("Speed");
        private static readonly int _attackHash = Animator.StringToHash("Attack");
        private static readonly int _dieHash = Animator.StringToHash("Die");
        private static readonly int _hitHash = Animator.StringToHash("Hit");

        // 状态
        private MonsterBehaviourIdleStatus _idleStatus;
        private MonsterBehaviourAttackStatus _attackStatus;
        private MonsterBehaviourChaseStatus _chaseStatus;
        private MonsterBehaviourToBirthStatus _toBirthStatus;
        private MonsterBehaviourDiedStatus _diedStatus;
        private MonsterBehaviourStatus _status;

        // 委派（必须放类里面定义，因为每个monster都是独立的）
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

        #region 血量之类的
        private bool isHit;
        private Coroutine hitIe;
        /// <summary>
        /// 受伤
        /// </summary>
        public void OnChangeHP(int attack = 0, int magicAttack = 0)
        {
            // 受到伤害
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
        /// 加血
        /// </summary>
        private void OnAddHP(int add)
        {
            AddHP(add);
        }
        #endregion
        #region 一些方法
        /// <summary>
        /// 旋转到lookat中，平滑的
        /// </summary>
        private void LookAtLerp(Vector3 lookAt, float speed = 5)
        {
            Quaternion q = transform.rotation;
            transform.LookAt(lookAt);
            transform.rotation = Quaternion.Lerp(q, transform.rotation, speed * Time.deltaTime);
        }
        /// <summary>
        /// 停止寻路
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
        #region 一些检测
        private bool cacheIsAngleOfView;
        private int chacheIsAngleOfViewFrame = -1;
        /// <summary>
        /// 是否在视角内
        /// </summary>
        /// <returns></returns>
        private bool IsAngleOfView()
        {
            // 如果当前帧已经判断过了
            if (chacheIsAngleOfViewFrame == Time.frameCount)
            {
                return cacheIsAngleOfView;
            }
            chacheIsAngleOfViewFrame = Time.frameCount;
            // monster 到角色的方向的向量
            Vector3 v3 = _playerBehaviour.transform.position - transform.position;
            // 如果这个向量和角色向前的量形成的夹角小于playerRadius
            return cacheIsAngleOfView = Mathf.Abs(Vector3.Angle(transform.forward, v3)) <= playerRadius;
        }
        private bool cacheIsCanLookPlayer;
        private int chacheIsCanLookPlayerFrame = -1;
        /// <summary>
        /// 是否能看到角色，当角色被障碍物拦住时应返回false
        /// </summary>
        /// <returns></returns>
        private bool IsCanLookPlayer()
        {
            // 如果当前帧已经判断过了
            if (chacheIsCanLookPlayerFrame == Time.frameCount)
            {
                return cacheIsCanLookPlayer;
            }
            chacheIsCanLookPlayerFrame = Time.frameCount;
            cacheIsCanLookPlayer = false;

            // 向前发射一条射线
            Vector3 start = visionPoint.position;
            Vector3 dir = _playerBehaviour.transform.position + Vector3.up;
            RaycastHit hit;
            Debug.DrawLine(start, dir, Color.red);
            // 检测两个点之间是否有碰撞物
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
        /// 获得角色和monster之间的距离
        /// </summary>
        private float GetSqrDistance()
        {
            // 计算平方距离，避免开平方
            return Vector3.SqrMagnitude(transform.position - _playerBehaviour.transform.position);
        }
        /// <summary>
        /// 获得出生点和monster之间的距离
        /// </summary>
        private float GetBirthSqrDistance()
        {
            // 计算平方距离，避免开平方
            return Vector3.SqrMagnitude(transform.position - _birthPoint);
        }
        #endregion
        #region 是否满足改变状态
        /// <summary>
        /// 能否改变状态到idle
        /// </summary>
        private bool CanChangeIdle()
        {
            // 能看到角色且在范围内
            if (IsCanLookPlayer() && IsAngleOfView() && GetSqrDistance() < distance) return false;
            if (GetBirthSqrDistance() > 1f) return false;
            return true;
        }
        /// <summary>
        /// 切换为攻击
        /// </summary>
        private bool CanChangeAttack()
        {
            // 玩家是否在视野内
            if (!IsAngleOfView()) return false;
            // 玩家是否在攻击范围内
            if (GetSqrDistance() > attackSqrDistance) return false;
            return true;
        }
        /// <summary>
        /// 切换为回到出生点状态
        /// </summary>
        private bool CanChangeToBirth()
        {
            // 能看到角色且在范围内
            if (IsCanLookPlayer() && IsAngleOfView() && GetSqrDistance() < distance) return false;

            return true;
        }
        /// <summary>
        /// 切换为追寻角色状态
        /// </summary>
        private bool CanChangeChase()
        {
            //Debug.Log(!IsAngleOfView() + " " + (GetSqrDistance() > distance) + " " + (GetSqrDistance() < attackOutSqrDistance));
            if (isHit) return true;
            // 不在视角范围内
            if (!IsAngleOfView()) return false;
            // player 和 monster 的距离
            if (GetSqrDistance() > distance) return false;
            // 攻击范围终止距离
            if (GetSqrDistance() < attackOutSqrDistance) return false;

            return true;

        }
        /// <summary>
        /// 切换状态
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

        #region 动画的event
        private void OnAttack()
        {
            // 创建一个球体，并返回碰撞体
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
