using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    /// <summary>
    /// 播放受伤动画
    /// </summary>
    [TaskCategory("MyBehaviour/Monster")]
    [TaskDescription("是否能播放受伤动画")]
    public class PlayHitAnim : Action
    {
        public SharedMonsterBehaviourTree monsterBehaviour;

        private bool isHit;
        private bool isHitAnim; // 正在处于hit动画的状态中
        private bool canHitAnim = true; // 能不能播放hit动画 内置cd
        public override TaskStatus OnUpdate()
        {
            if (monsterBehaviour != null && canHitAnim)
            {
                StartCoroutine(hitAnimIE());
                monsterBehaviour.Value.animator.SetTrigger(MonsterBehaviourTree.hitHash);
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
        public override void OnReset()
        {
        }

        private IEnumerator hitAnimIE()
        {
            canHitAnim = false;
            isHitAnim = true;
            yield return new WaitForSeconds(2);
            isHitAnim = false;
            yield return new WaitForSeconds(monsterBehaviour.Value.hitAnimCD);
            canHitAnim = true;

        }
    }
}
