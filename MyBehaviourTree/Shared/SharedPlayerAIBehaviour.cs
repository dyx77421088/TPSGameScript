using BehaviorDesigner.Runtime;
using System;

namespace TPSShoot.BehaviourTree
{
    [Serializable]
    public class SharedPlayerAIBehaviour : SharedVariable<PlayerAIBehaviour>
    {
        public static implicit operator SharedPlayerAIBehaviour(PlayerAIBehaviour value) 
        { 
            return new SharedPlayerAIBehaviour { mValue = value }; 
        }
    }
}

