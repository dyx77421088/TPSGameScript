using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TMPro;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/PlayerAI")]
    public class Attack : Action
    {
        public SharedPlayerAIBehaviour playerBehaviourTree;
        public SharedTransform target;

        public override TaskStatus OnUpdate()
        {
            if (playerBehaviourTree.Value == null) return TaskStatus.Failure;
            if (target.Value != null) LookAtLerp(target.Value.position);
            OnSwordAttack();
            OnGunAttack();
            return TaskStatus.Success;
        }
        private void LookAtLerp(Vector3 lookAt, float speed = 5)
        {
            Vector3 targetDirection = lookAt - transform.position;
            targetDirection.y = 0f; // 保持Y轴值为0
            Quaternion rotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
            playerBehaviourTree.Value.Idle();
        }
        private void OnSwordAttack()
        {
            playerBehaviourTree.Value.OnSwordSkillAttackRequest(PlayerAIBehaviour.PlayerSwordAttackMode.SkillAttack1);
        }
        private void OnGunAttack()
        {
            playerBehaviourTree.Value.OnFireRequest(target.Value.position);
        }




        public override void OnReset()
        {
            playerBehaviourTree.Value = null;
        }

    }
}
