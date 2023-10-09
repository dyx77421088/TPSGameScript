using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    [TaskDescription("当前血量是否低于一半,如果使用userOther，则判断指定的hp，若使用greater 则判断是否大于")]
    public class CurrentHPLessThanHalf : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("playerAIBehaviour")] public SharedPlayerAIBehaviour playerAIBehaviour;
        public bool userOther;
        public bool isGreater; // 是否大于
        public float hp;

        public override TaskStatus OnUpdate()
        {
            if (playerAIBehaviour.Value == null) return TaskStatus.Failure;
            float less = userOther ? hp : playerAIBehaviour.Value.MaxHP * 0.5f;
            bool b;
            if (isGreater) b = playerAIBehaviour.Value.CurrentHP > less;
            else b = playerAIBehaviour.Value.CurrentHP <= less;

            return b ? TaskStatus.Success : TaskStatus.Failure;
        }

    }
}
