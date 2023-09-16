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
        [Tooltip("距离")]public float distance = 60 * 60;
        [Tooltip("视角")] public float playerRadius = 75;
        [Tooltip("从哪个位置开始观察")]public Transform visionPoint;
        [Tooltip("观察的层")]public LayerMask visionLayer = 1 << 0;

        [Header("攻击的一些设置")]
        [Tooltip("造成的伤害")] public float damage = 10;
        [Tooltip("攻击距离")] public float attackDistance = 1f;
        [Tooltip("攻击范围")] public float attackSphereRadius = 0.3f;


        private CharacterController _characterController;
        private Animator _animator;
        private NavMeshAgent _agent;
        private PlayerBehaviour _playerBehaviour;

        // 一些动画
        private static readonly int _speedHash = Animator.StringToHash("Speed");
        private static readonly int _attackHash = Animator.StringToHash("Attack");
        private static readonly int _dieHash = Animator.StringToHash("Die");
        private static readonly int _hitHash = Animator.StringToHash("Hit");

        // 状态
        private MonsterBehaviourIdleStatus _idleStatus;
        private MonsterBehaviourStatus _status;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _playerBehaviour = PlayerBehaviour.Instance;
        }
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
            Vector3 start = transform.position;
            Vector3 dir = _playerBehaviour.transform.position + Vector3.up;
            RaycastHit hit;
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
        #endregion
        #region 是否满足改变状态
        /// <summary>
        /// 能否改变状态到idle
        /// </summary>
        private bool CanChangeIdle()
        {
            return false;
        }
        /// <summary>
        /// 切换为攻击
        /// </summary>
        private bool CanChangeAttack()
        {
            // 玩家是否在视野内
            if (!IsAngleOfView()) return false;
            // 玩家是否在攻击范围内
            if (GetSqrDistance() > attackDistance) return false;
            return true;
        }
        /// <summary>
        /// 切换为搜索状态
        /// </summary>
        private bool CanChangeSerach()
        {
            // 如果能看见角色
            if (IsCanLookPlayer()) return false;
            
            return true;
        }
        /// <summary>
        /// 切换为追寻角色状态
        /// </summary>
        private bool CanChangeChase()
        {
            // 不在视角范围内
            if (!IsAngleOfView()) return false;
            // player 和 monster 的距离
            if (GetSqrDistance() > distance) return false;

            return true;

        }
        /// <summary>
        /// 切换状态
        /// </summary>
        private void ChangeStatus(MonsterBehaviourStatus status)
        {
            if (status == _status) return;
            _status?.OnExit();
            _status = status;
            _status.OnEnter();
        }
        #endregion

        public class MonsterBehaviourStatus
        {
            private MonsterBehaviour mb;
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
