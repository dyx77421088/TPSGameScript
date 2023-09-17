using UnityEngine;
using UnityEngine.AI;

namespace TPSShoot
{
    /// <summary>
    /// idle状态
    /// </summary>
    public partial class MonsterBehaviour
    {
        public class MonsterBehaviourIdleStatus : MonsterBehaviourStatus
        {
            private MonsterBehaviour _mb;
            public MonsterBehaviourIdleStatus(MonsterBehaviour mb) : base(mb)
            {
                _mb = mb;
            }

            public override void OnEnter()
            {
                Debug.Log("现在进入了IDLE状态");
                _mb._animator.SetFloat(_speedHash, 0);
            }

            public override void OnExit()
            {

            }

            public override void OnUpdate()
            {
                // 是否能切换为攻击状态
                if (_mb.CanChangeAttack()) _mb.ChangeStatus(_mb._attackStatus);
                // 是否能切换为追寻状态
                else if (_mb.CanChangeChase()) _mb.ChangeStatus(_mb._chaseStatus);
                else if(Time.frameCount % 10 == 0)
                {
                    _mb.OnAddHP(_mb.monsterAttribute.addHP);
                }
            }
        }
    }
}
