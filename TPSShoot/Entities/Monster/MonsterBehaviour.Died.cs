using UnityEngine;
using UnityEngine.AI;

namespace TPSShoot
{
    /// <summary>
    /// 死亡状态
    /// </summary>
    public partial class MonsterBehaviour
    {
        public class MonsterBehaviourDiedStatus : MonsterBehaviourStatus
        {
            public MonsterBehaviourDiedStatus(MonsterBehaviour mb) : base(mb)
            {
            }

            public override void OnEnter()
            {
                Debug.Log("现在进入了寻路状态");
                mb._animator.SetTrigger(_dieHash);
                mb.StopNavAgent();
                mb.onMonsterDied?.Invoke();
                Destroy(mb.gameObject, mb.dieTime);
            }

        }
    }
}
