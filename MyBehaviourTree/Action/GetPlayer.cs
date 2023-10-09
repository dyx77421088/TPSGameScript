using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using TPSShoot;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/Monster")]
    public class GetPlayer : Action
    {
        public SharedTransform sharedPlayer;

        public override TaskStatus OnUpdate()
        {
            sharedPlayer.Value = PlayerBehaviour.Instance.ikSettings.sprine; // …œ∞Î…Ìµƒik
            return sharedPlayer.Value == null ? TaskStatus.Failure : TaskStatus.Success;
        }
        public override void OnReset()
        {
            sharedPlayer.Value = null;
        }

    }
}
