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
    /// 这个怪物的类就是用来接收一些参数的
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public partial class MonsterBehaviour : MonoBehaviour
    {
        [Header("观察到player的一些属性")]
        [Tooltip("最近距离")]public float minDistance = 3 * 3;
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
        [Tooltip("受伤动画的cd")] public float hitAnimCD = 5;
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

        private bool isPause;
        // 状态
        private MonsterBehaviourIdleStatus _idleStatus;
        private MonsterBehaviourAttackStatus _attackStatus;
        private MonsterBehaviourChaseStatus _chaseStatus;
        private MonsterBehaviourToBirthStatus _toBirthStatus;
        private MonsterBehaviourDiedStatus _diedStatus;
        private MonsterBehaviourHitStatus _hitStatus;
        private MonsterBehaviourStatus _status;

        // 委派（必须放类里面定义，因为每个monster都是独立的）
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
            // 初始化等级
            monsterAttribute.grade = RandomUtils.RandomInt(monsterAttribute.minGrade, monsterAttribute.maxGrade);
            // 初始化属性
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
            Debug.Log("进来赋值了！！！");
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
        // hit动画状态
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
            Debug.Log("进来修改重力了");
            Vector3 gravityV3 = new Vector3();
            gravityV3.y -= _gravity;
            _characterController.Move(gravityV3 * Time.deltaTime);
        }
        #region 血量之类的
        private bool isHit;
        private bool isHitAnim;
        private bool canHitAnim = true; // 能不能播放hit动画
        private Coroutine hitIe;
        /// <summary>
        /// 受伤
        /// </summary>
        public void OnChangeHP(int grade, int attack = 0, int magicAttack = 0)
        {
            Debug.Log("受伤");
            // 受到伤害
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
        /// 加血
        /// </summary>
        private void OnAddHP(float add)
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
            if (_playerBehaviour == null) return false;
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
            if (_playerBehaviour == null) return Mathf.Infinity;
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
            if (GetSqrDistance() < minDistance) return false;
            return true;
        }
        /// <summary>
        /// 切换为追寻角色状态
        /// </summary>
        private bool CanChangeChase()
        {
            if (IsAttacking) return false;
            if (isHit) return true;
            if (GetSqrDistance() < minDistance) return true; // 角色靠太近了会被察觉
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
        /// <summary>
        /// 攻击
        /// </summary>
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
