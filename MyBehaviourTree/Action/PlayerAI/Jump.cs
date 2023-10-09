using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TMPro;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    public class Jump : Action
    {
        public SharedPlayerAIBehaviour playerBehaviourTree;

        public override TaskStatus OnUpdate()
        {
            if (playerBehaviourTree.Value == null) return TaskStatus.Failure;
            return playerBehaviourTree.Value.OnJumpRequested() ? TaskStatus.Success : TaskStatus.Failure;
        }

        public override void OnReset()
        {
            playerBehaviourTree.Value = null;
        }

    }
}
