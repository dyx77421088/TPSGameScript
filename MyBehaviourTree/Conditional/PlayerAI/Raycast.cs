using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    [TaskDescription("��������")]
    public class Raycast : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("���ĸ�λ�ÿ�ʼ")] public SharedTransform origin;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("�ĸ�����")] public SharedTransform direction;
        public LayerMask layerMask;

        private float lastTime;
        private float cd = 1;
        public override TaskStatus OnUpdate()
        {
            //float t = Time.time;
            //if (t - lastTime < cd)
            //{
            //    return TaskStatus.Failure;
            //}
            //lastTime = t;
            return Check() ? TaskStatus.Success : TaskStatus.Failure;
        }
        private bool Check()
        {
            RaycastHit hit;
            if (Physics.Raycast(origin.Value.position, direction.Value == null ? transform.forward : direction.Value.position, out hit, 4, layerMask))
            {
                Debug.Log(hit.collider.gameObject.name);
                return true;
            }
            return false;
        }
    }
}
