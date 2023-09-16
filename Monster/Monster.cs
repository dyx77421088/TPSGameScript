using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int hp = 100; // ��ǰѪ��
    public int maxHp = 100; // ���Ѫ��
    public int aggressivity = 5; // ������
    public int physicsDefense = 4; // ���������
    public int magicDefense = 2; // ħ������

    public int damageAnimNum = 3; // �������ٵ��˺��ŻᲥ�����˶���
    public float hitInterval = 4; // ���˼��

    private float hitNum = 0;

    private MonsterAnimator monsterAnimator;
    void Start()
    {
        monsterAnimator = GetComponent<MonsterAnimator>();
    }

    /// <summary>
    /// �����ܵ��������˺���ħ���˺�
    /// </summary>
    /// <param name="physics">�����˺�</param>
    /// <param name="magic">ħ���˺�</param>
    public void BeInjured(int physics = 0, int magic = 0)
    {
        if (hp <= 0) return;
        int damage = GetPhysice(physics) + GetMagic(magic);
        hp -= damage;

        // ���˶���
        if (damage > damageAnimNum && hitNum >= hitInterval)
        {
            hitNum = 0;
            if (monsterAnimator != null)
            {
                monsterAnimator.PlayAnimator(MonsterAnimator.MonsterAnimEnum.Hit);
            }
        }
        // �޸�ui TODD

        if (hp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// ���������Ĵ���
    /// </summary>
    private void Die()
    {
        // ������������������������
        if (monsterAnimator != null)
        {
            monsterAnimator.PlayAnimator(MonsterAnimator.MonsterAnimEnum.Die);
        }
        // �����������Ʒ TODD

        // ������Ʒui��ʾ TODD

        Destroy(gameObject, 2);
    }

    private void Update()
    {
        hitNum += Time.deltaTime;
    }

    /// <summary>
    /// �ܵ��������˺��ļ��㹫ʽ
    /// </summary>
    /// <param name="physice">���ܵ��������˺�</param>
    /// <returns></returns>
    private int GetPhysice(int physice)
    {
        if (physice <= 0) return 0;
        int t = physice - physicsDefense;
        return t <= 0 ? 1 : t;
    }

    /// <summary>
    /// �ܵ���ħ���˺��ļ��㹫ʽ
    /// </summary>
    /// <param name="physice">���ܵ��������˺�</param>
    /// <returns></returns>
    private int GetMagic(int magic)
    {
        if (magic <= 0) return 0;
        int t = magic - magicDefense;
        return t <= 0 ? 1 : t;
    }
}
