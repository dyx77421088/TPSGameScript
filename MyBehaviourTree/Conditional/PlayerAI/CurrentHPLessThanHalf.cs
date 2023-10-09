using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    [TaskDescription("��ǰѪ���Ƿ����һ��,���ʹ��userOther�����ж�ָ����hp����ʹ��greater ���ж��Ƿ����")]
    public class CurrentHPLessThanHalf : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("playerAIBehaviour")] public SharedPlayerAIBehaviour playerAIBehaviour;
        public bool userOther;
        public bool isGreater; // �Ƿ����
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
