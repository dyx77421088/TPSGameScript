using BehaviorDesigner.Runtime;
using System;

namespace TPSShoot.BehaviourTree
{
    [Serializable]
    public class SharedMonsterBehaviourTree : SharedVariable<MonsterBehaviourTree>
    {
        public static implicit operator SharedMonsterBehaviourTree(MonsterBehaviourTree value) 
        { 
            return new SharedMonsterBehaviourTree { mValue = value }; 
        }
    }
}

