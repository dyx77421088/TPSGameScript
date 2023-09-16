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
        if ((transform.position - player.position).sqrMagnitude > idleDistance) // �����ɫ�͹����������Զ��ֹͣ
        {
            if (monsterAnimator != null)
            {
                monsterAnimator.PlayAnimator(MonsterAnimator.MonsterAnimEnum.Stand); // ֹͣ
            }
        }
        else if ((transform.position - player.position).sqrMagnitude > attackDistance)
        {
            transform.LookAt(player);
            if (monsterAnimator != null)
            {
                monsterAnimator.PlayAnimator(MonsterAnimator.MonsterAnimEnum.Walk); // ��ǰ��
            }

            transform.position += transform.forward * speed * Time.deltaTime; // �ƶ�
        }
        else
        {
            if (monsterAnimator != null)
            {
                monsterAnimator.PlayAnimator(MonsterAnimator.MonsterAnimEnum.Attack); // �ﵽ��������
            }
        }
    }

}
