using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    [TaskCategory("MyBehaviour/Monster")]
    [TaskDescription("武器是否为指定的这个状态")]
    public class IsCurrentWeapon : Conditional
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("playerAIBehaviour")] public SharedPlayerAIBehaviour playerAIBehaviour;
        public CurrentWeaponType type = CurrentWeaponType.None;

        public override TaskStatus OnUpdate()
        {
            if (playerAIBehaviour.Value == null) return TaskStatus.Failure;
            if (playerAIBehaviour.Value.IsWeapingWeapon)
            {
                return TaskStatus.Running;
            }
            var t = GetCurrentWeaponType();
            return t == type ? TaskStatus.Success : TaskStatus.Failure;
        }

        private CurrentWeaponType GetCurrentWeaponType()
        {
            if (playerAIBehaviour.Value.IsNoWeapon) return CurrentWeaponType.None;
            if (playerAIBehaviour.Value.IsGunWeapon) return CurrentWeaponType.Gun;
            if (playerAIBehaviour.Value.IsSwordWeapon) return CurrentWeaponType.Sword;
            return CurrentWeaponType.None;
        }

        public override void OnReset()
        {
            type = CurrentWeaponType.None;
        }

    }
}
