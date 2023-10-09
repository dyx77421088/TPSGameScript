using UnityEngine;
using UnityEngine.AI;

namespace TPSShoot
{
    /// <summary>
    /// hit状态
    /// </summary>
    public partial class MonsterBehaviour
    {
        public class MonsterBehaviourHitStatus : MonsterBehaviourStatus
        {
            public MonsterBehaviourHitStatus(MonsterBehaviour mb) : base(mb)
            {
            }

            public override void OnEnter()
            {
                Debug.Log("进入了hit" + mb.isHitAnim);
                // 关闭寻路
                mb._agent.isStopped = true;
            }

            public override void OnExit()
            {
                Debug.Log("退出hit" + mb.isHitAnim);
                mb._agent.isStopped = false;
            }

            public override void OnUpdate()
            {
                Debug.Log("hit" + mb.isHit + " hitanim" + mb.isHitAnim);
                if (!mb.isHitAnim)
                {
                    // 是否能切换为攻击状态
                    if (mb.CanChangeAttack()) mb.ChangeStatus(mb._attackStatus);
                    // 是否能切换为追寻状态
                    else if (mb.CanChangeChase()) mb.ChangeStatus(mb._chaseStatus);
                    else if (mb.CanChangeToBirth())
                    {
                        mb.ChangeStatus(mb._toBirthStatus);
                    }
                }
            }
        }
    }
}
