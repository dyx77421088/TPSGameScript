using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    [TaskDescription("��ǰѪ���Ƿ����һ��")]
    public class IsHit : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("playerAIBehaviour")] public SharedPlayerAIBehaviour playerAIBehaviour;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("����˺��Ķ���")] public SharedTransform hitTransform;

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
