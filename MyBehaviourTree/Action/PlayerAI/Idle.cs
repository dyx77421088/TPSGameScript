using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TMPro;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    public class Idle : Action
    {
        public SharedPlayerAIBehaviour playerBehaviourTree;

        public override TaskStatus OnUpdate()
        {
            if (playerBehaviourTree.Value == null) return TaskStatus.Failure;
            playerBehaviourTree.Value.Idle();
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            playerBehaviourTree.Value = null;
        }

    }
}
