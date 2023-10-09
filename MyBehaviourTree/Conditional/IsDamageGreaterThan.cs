using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/Monster")]
    [TaskDescription("受到的伤害是否大于某个值")]
    public class IsDamageGreaterThan : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("monsterBehaviour")] public SharedMonsterBehaviourTree monsterBehaviour;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("大于某个值")] public SharedFloat damage;

        public override TaskStatus OnUpdate()
        {
            //if
            return TaskStatus.Success;
        }

        private Transform CanSeeObj(Transform obj, float fieldOfViewAngle, float distance)
        {
            Vector3 dir = obj.position - transform.position; // 从本位置到目标位置的向量
            var angle = Vector3.Angle(transform.forward, dir);// 角度
            if (dir.sqrMagnitude < distance && angle < fieldOfViewAngle * 0.5f)
            {
                return obj;
            }
            return null;
        }
    }
}
