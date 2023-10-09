using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/Monster")]
    [TaskDescription("�ܵ����˺��Ƿ����ĳ��ֵ")]
    public class IsDamageGreaterThan : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("monsterBehaviour")] public SharedMonsterBehaviourTree monsterBehaviour;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("����ĳ��ֵ")] public SharedFloat damage;

        public override TaskStatus OnUpdate()
        {
            //if
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
