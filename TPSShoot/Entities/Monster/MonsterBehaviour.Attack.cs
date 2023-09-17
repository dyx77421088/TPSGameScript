using UnityEngine;
using UnityEngine.AI;

namespace TPSShoot
{
    /// <summary>
    /// 攻击状态
    /// </summary>
    public partial class MonsterBehaviour
    {
        public class MonsterBehaviourAttackStatus : MonsterBehaviourStatus
        {
            private MonsterBehaviour _mb;
            public MonsterBehaviourAttackStatus(MonsterBehaviour mb) : base(mb)
            {
                _mb = mb;
            }

            public override void OnEnter()
            {
                Debug.Log("现在进入了攻击状态");
                _mb.StopNavAgent();
                _mb._animator.SetBool(_attackHash, true);
            }

            public override void OnExit()
            {
                _mb._animator.SetBool(_attackHash, false);
            }

            public override void OnUpdate()
            {
                // 朝向目标
                _mb.LookAtLerp(_mb._playerBehaviour.transform.position);
                // 是否能切换为追寻状态
                if (_mb.CanChangeChase()) _mb.ChangeStatus(_mb._chaseStatus);
                // 被障碍物挡住了
                else if (IsPlayerLost()) _mb.ChangeStatus(_mb._toBirthStatus);
            }

            private bool IsPlayerLost()
            {
                // 看不到角色了
                return !_mb.IsCanLookPlayer();
            }
        }
    }
}
