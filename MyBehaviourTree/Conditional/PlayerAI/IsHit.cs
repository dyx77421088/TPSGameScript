using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    [TaskDescription("当前血量是否低于一半")]
    public class IsHit : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("playerAIBehaviour")] public SharedPlayerAIBehaviour playerAIBehaviour;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("造成伤害的对象")] public SharedTransform hitTransform;

        public override TaskStatus OnUpdate()
        {
            if (playerAIBehaviour.Value ==  null) { return TaskStatus.Failure; }
            hitTransform.Value = playerAIBehaviour.Value.HitTransform;
            return playerAIBehaviour.Value.IsHit ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            playerAIBehaviour.Value = null;
        }

    }
}
