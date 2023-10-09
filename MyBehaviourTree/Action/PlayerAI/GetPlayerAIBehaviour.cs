using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using TPSShoot;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    public class GetPlayerAIBehaviour : Action
    {
        public SharedPlayerAIBehaviour playerBehaviourTree;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("向前位置发射射线的位置")]public SharedTransform forwardPosition;

        public override TaskStatus OnUpdate()
        {
            playerBehaviourTree.Value = GetComponent<PlayerAIBehaviour>();
            if (playerBehaviourTree.Value != null )
            {
                forwardPosition.Value = playerBehaviourTree.Value.forwardPosition;
            }
            return playerBehaviourTree.Value == null ? TaskStatus.Failure : TaskStatus.Success;
        }
        public override void OnReset()
        {
            playerBehaviourTree.Value = null;
        }

    }
}
