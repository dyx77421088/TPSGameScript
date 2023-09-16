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
        [Tooltip("����")]public float distance = 60 * 60;
        [Tooltip("�ӽ�")] public float playerRadius = 75;
        [Tooltip("���ĸ�λ�ÿ�ʼ�۲�")]public Transform visionPoint;
        [Tooltip("�۲�Ĳ�")]public LayerMask visionLayer = 1 << 0;

        [Header("������һЩ����")]
        [Tooltip("��ɵ��˺�")] public float damage = 10;
        [Tooltip("��������")] public float attackDistance = 1f;
        [Tooltip("������Χ")] public float attackSphereRadius = 0.3f;


        private CharacterController _characterController;
        private Animator _animator;
        private NavMeshAgent _agent;
        private PlayerBehaviour _playerBehaviour;

        // һЩ����
        private static readonly int _speedHash = Animator.StringToHash("Speed");
        private static readonly int _attackHash = Animator.StringToHash("Attack");
        private static readonly int _dieHash = Animator.StringToHash("Die");
        private static readonly int _hitHash = Animator.StringToHash("Hit");

        // ״̬
        private MonsterBehaviourIdleStatus _idleStatus;
        private MonsterBehaviourStatus _status;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _playerBehaviour = PlayerBehaviour.Instance;
        }
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
            Vector3 start = transform.position;
            Vector3 dir = _playerBehaviour.transform.position + Vector3.up;
            RaycastHit hit;
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
        #endregion
        #region �Ƿ�����ı�״̬
        /// <summary>
        /// �ܷ�ı�״̬��idle
        /// </summary>
        private bool CanChangeIdle()
        {
            return false;
        }
        /// <summary>
        /// �л�Ϊ����
        /// </summary>
        private bool CanChangeAttack()
        {
            // ����Ƿ�����Ұ��
            if (!IsAngleOfView()) return false;
            // ����Ƿ��ڹ�����Χ��
            if (GetSqrDistance() > attackDistance) return false;
            return true;
        }
        /// <summary>
        /// �л�Ϊ����״̬
        /// </summary>
        private bool CanChangeSerach()
        {
            // ����ܿ�����ɫ
            if (IsCanLookPlayer()) return false;
            
            return true;
        }
        /// <summary>
        /// �л�Ϊ׷Ѱ��ɫ״̬
        /// </summary>
        private bool CanChangeChase()
        {
            // �����ӽǷ�Χ��
            if (!IsAngleOfView()) return false;
            // player �� monster �ľ���
            if (GetSqrDistance() > distance) return false;

            return true;

        }
        /// <summary>
        /// �л�״̬
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
