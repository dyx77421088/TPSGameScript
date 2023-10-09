using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using TPSShoot;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/Monster")]
    public class GetMonsterBehaviourTree : Action
    {
        public SharedMonsterBehaviourTree monsterBehaviourTree;

        public override TaskStatus OnUpdate()
        {
            monsterBehaviourTree.Value = GetComponent<MonsterBehaviourTree>();
            return monsterBehaviourTree.Value == null ? TaskStatus.Failure : TaskStatus.Success;
        }
        public override void OnReset()
        {
            monsterBehaviourTree.Value = null;
        }

    }
}
