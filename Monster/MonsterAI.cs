using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public float speed = 1.2f;
    public float attackDistance = 2.8f;
    public float idleDistance = 10f;

    private Transform player;
    private MonsterAnimator monsterAnimator;


    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        monsterAnimator = GetComponent<MonsterAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - player.position).sqrMagnitude > idleDistance) // 如果角色和怪物距离相差过远就停止
        {
            if (monsterAnimator != null)
            {
                monsterAnimator.PlayAnimator(MonsterAnimator.MonsterAnimEnum.Stand); // 停止
            }
        }
        else if ((transform.position - player.position).sqrMagnitude > attackDistance)
        {
            transform.LookAt(player);
            if (monsterAnimator != null)
            {
                monsterAnimator.PlayAnimator(MonsterAnimator.MonsterAnimEnum.Walk); // 向前走
            }

            transform.position += transform.forward * speed * Time.deltaTime; // 移动
        }
        else
        {
            if (monsterAnimator != null)
            {
                monsterAnimator.PlayAnimator(MonsterAnimator.MonsterAnimEnum.Attack); // 达到攻击距离
            }
        }
    }

}
