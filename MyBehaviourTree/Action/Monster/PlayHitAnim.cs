using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot.BehaviourTree
{
    /// <summary>
    /// �������˶���
    /// </summary>
    [TaskCategory("MyBehaviour/Monster")]
    [TaskDescription("�Ƿ��ܲ������˶���")]
    public class PlayHitAnim : Action
    {
        public SharedMonsterBehaviourTree monsterBehaviour;

        private bool isHit;
        private bool isHitAnim; // ���ڴ���hit������״̬��
        private bool canHitAnim = true; // �ܲ��ܲ���hit���� ����cd
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
