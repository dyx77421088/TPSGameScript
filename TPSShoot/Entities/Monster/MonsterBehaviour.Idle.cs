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
            }

            public override void OnExit()
            {
            }

            public override void OnUpdate()
            {
            }
        }
    }
}
