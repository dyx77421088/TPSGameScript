using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using TPSShoot;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    public class ChangeWeapon : Action
    {
        public SharedPlayerAIBehaviour playerAIBehaviour;
        public int index = 0;

        public override TaskStatus OnUpdate()
        {
            if (playerAIBehaviour.Value == null) { return TaskStatus.Failure; }
            if (playerAIBehaviour.Value.weaponSettings.allWeapon.Length <= index) return TaskStatus.Failure;
            if (playerAIBehaviour.Value.IsWeapingWeapon) return TaskStatus.Running;
            playerAIBehaviour.Value.OnSwapWeapon(index);
            return TaskStatus.Success;
        }
        public override void OnReset()
        {
            index = 0;
        }

    }
}
