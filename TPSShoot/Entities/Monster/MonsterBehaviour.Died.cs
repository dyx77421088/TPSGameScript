using UnityEngine;
using UnityEngine.AI;

namespace TPSShoot
{
    /// <summary>
    /// 死亡状态
    /// </summary>
    public partial class MonsterBehaviour
    {
        private bool isDied;
        public bool IsDied { get { return isDied; } }
        public class MonsterBehaviourDiedStatus : MonsterBehaviourStatus
        {
            public MonsterBehaviourDiedStatus(MonsterBehaviour mb) : base(mb)
            {
            }

            public override void OnEnter()
            {
                mb.isDied = true;
                //Debug.Log("现在进入了死亡状态");
                mb._animator.SetTrigger(_dieHash);
                mb.StopNavAgent();
                mb.onMonsterDied?.Invoke();
                Destroy(mb.gameObject, mb.dieTime);
                // 消灭了type的monster
                Events.PlayerKillMonster.Call(mb.monsterAttribute);
            }

        }
    }
}
