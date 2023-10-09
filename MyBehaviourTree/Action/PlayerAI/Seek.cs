using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    public class Seek : Action
    {
        public SharedPlayerAIBehaviour playerBehaviourTree;
        public SharedTransform target;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("切换为跑步状态需要距离多远")]public SharedFloat runDistance = 10;
        [BehaviorDesigner.Runtime.Tasks.Tooltip("到达距离")]public SharedFloat arriveDistance = 0.1f;

        public override TaskStatus OnUpdate()
        {
            if (playerBehaviourTree.Value == null ||  target.Value == null) return TaskStatus.Failure;
            if (Vector3.SqrMagnitude(target.Value.position - transform.position) <= arriveDistance.Value)
            {
                playerBehaviourTree.Value.Idle(); // idle状态
                return TaskStatus.Success;
            }

            // 当距离超过一定时，切换为奔跑状态
            if (Vector3.SqrMagnitude(target.Value.position - transform.position) > runDistance.Value) playerBehaviourTree.Value.UpdateRun();
            else playerBehaviourTree.Value.UpdateRun(false);

            LookAtLerp(target.Value.position);
            if (Check()) playerBehaviourTree.Value.OnJumpRequested();
            playerBehaviourTree.Value.UpdateWalk(1, 0);
            return TaskStatus.Running;
        }


        private void LookAtLerp(Vector3 lookAt, float speed = 5)
        {
            Vector3 targetDirection = lookAt - transform.position;
            targetDirection.y = 0f; // 保持Y轴值为0
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime); ;
        }

        private bool Check()
        {
            RaycastHit hit;
            if (Physics.Raycast(playerBehaviourTree.Value.forwardPosition.position, transform.forward, out hit, 2, LayerMask.GetMask(Layers.Terrain)))
            {
                Debug.Log(hit.collider.gameObject.name);
                return true;
            }
            return false;
        }




        public override void OnReset()
        {
            playerBehaviourTree.Value = null;
            target.Value = null;
            runDistance.Value = 10;
            arriveDistance.Value = 0.1f;
        }

    }
}
