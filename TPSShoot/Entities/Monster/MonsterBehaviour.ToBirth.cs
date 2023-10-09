using UnityEngine;
using UnityEngine.AI;

namespace TPSShoot
{
    /// <summary>
    /// 到出生点
    /// </summary>
    public partial class MonsterBehaviour
    {
        public class MonsterBehaviourToBirthStatus : MonsterBehaviourStatus
        {
            private MonsterBehaviour _mb;
            public MonsterBehaviourToBirthStatus(MonsterBehaviour mb) : base(mb)
            {
                _mb = mb;
            }

            public override void OnEnter()
            {
                //Debug.Log("现在进入了回到出生点状态");
                _mb._animator.SetFloat(_speedHash, 1);
                _mb.StartNavAgent(_mb.toBirthSpeed, _mb._birthPoint);
            }

            public override void OnExit()
            {
                _mb._animator.SetFloat(_speedHash, 0);
            }

            public override void OnUpdate()
            {
                // 朝向目标
                _mb.LookAtLerp(_mb._agent.steeringTarget);
                // 能否切换为攻击状态
                if (_mb.CanChangeAttack()) _mb.ChangeStatus(_mb._attackStatus);
                // 能否切换为追寻状态
                else if (_mb.CanChangeChase()) _mb.ChangeStatus(_mb._chaseStatus);
                // 是否能切换为idle状态
                else if (_mb.CanChangeIdle()) _mb.ChangeStatus(_mb._idleStatus);
                // 被障碍物挡住了
            }
        }
    }
}
