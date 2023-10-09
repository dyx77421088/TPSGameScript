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
        [BehaviorDesigner.Runtime.Tasks.Tooltip("�Ƕ�")] public SharedFloat fieldOfViewAngle = 90;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("�����ƽ��")] public SharedFloat viewDistance = 1000;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("���ֵ�Ŀ��")] public SharedTransform objectInSight;

        public override TaskStatus OnUpdate()
        {
            // ����objectָ����
            if (targetObject.Value != null)
            {
                objectInSight.Value = CanSeeObj(targetObject.Value, fieldOfViewAngle.Value, viewDistance.Value);
            }
            else // ͼ���
            {

            }
            if (objectInSight.Value == null) return TaskStatus.Failure;
            return TaskStatus.Success;
        }

        private Transform CanSeeObj(Transform obj, float fieldOfViewAngle, float distance)
        {
            Vector3 dir = obj.position - transform.position; // �ӱ�λ�õ�Ŀ��λ�õ�����
            var angle = Vector3.Angle(transform.forward, dir);// �Ƕ�
            if (dir.sqrMagnitude < distance && angle < fieldOfViewAngle * 0.5f)
            {
                return obj;
            }
            return null;
        }
    }
}
