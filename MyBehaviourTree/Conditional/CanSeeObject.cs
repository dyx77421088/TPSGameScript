using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour")]
    public class CanSeeObject : Conditional
    {
        public SharedTransform targetObject;
        public LayerMask objectLayerMask;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("角度")] public SharedFloat fieldOfViewAngle = 90;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("距离的平方")] public SharedFloat viewDistance = 1000;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("发现的目标")] public SharedTransform objectInSight;

        public override TaskStatus OnUpdate()
        {
            // 优先object指定的
            if (targetObject.Value != null)
            {
                objectInSight.Value = CanSeeObj(targetObject.Value, fieldOfViewAngle.Value, viewDistance.Value);
            }
            else // 图层的
            {

            }
            if (objectInSight.Value == null) return TaskStatus.Failure;
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
