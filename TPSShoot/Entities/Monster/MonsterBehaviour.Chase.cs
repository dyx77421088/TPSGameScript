using UnityEngine;
using UnityEngine.AI;

namespace TPSShoot
{
    /// <summary>
    /// 追寻状态
    /// </summary>
    public partial class MonsterBehaviour
    {
        public class MonsterBehaviourChaseStatus : MonsterBehaviourStatus
        {
            private MonsterBehaviour _mb;
            public MonsterBehaviourChaseStatus(MonsterBehaviour mb) : base(mb)
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
