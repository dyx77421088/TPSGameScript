using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MonsterAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void PlayAnimator(MonsterAnimEnum anim)
    {
        switch (anim)
        {
            case MonsterAnimEnum.Stand:
                animator.SetFloat("Speed", 0);
                break;
            case MonsterAnimEnum.Walk:
                animator.SetFloat("Speed", 1);
                break;
            case MonsterAnimEnum.Die:
                //animator.SetBool("Die", true);
                animator.SetTrigger("Die");
                break;
            case MonsterAnimEnum.Attack:
                animator.SetTrigger("Attack");
                break;
            case MonsterAnimEnum.Hit:
                animator.SetTrigger("Hit");
                break;
            case MonsterAnimEnum.UniqueSkill:
                animator.SetTrigger("UniqueSkill");
                break;
            default:
                break;
        }
    }

    

    public enum MonsterAnimEnum
    {
        Stand, // ’æ¡¢
        Walk, // “∆∂Ø
        Die, // À¿Õˆ
        Attack, // π•ª˜
        Hit, //  ‹…À
        UniqueSkill, // æ¯’–
    }
}
