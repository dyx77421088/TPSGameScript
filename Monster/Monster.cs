using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int hp = 100; // 当前血量
    public int maxHp = 100; // 最大血量
    public int aggressivity = 5; // 攻击力
    public int physicsDefense = 4; // 物理防御力
    public int magicDefense = 2; // 魔法防御

    public int damageAnimNum = 3; // 超过多少的伤害才会播放受伤动画
    public float hitInterval = 4; // 受伤间隔

    private float hitNum = 0;

    private MonsterAnimator monsterAnimator;
    void Start()
    {
        monsterAnimator = GetComponent<MonsterAnimator>();
    }

    /// <summary>
    /// 怪物受到的物理伤害和魔法伤害
    /// </summary>
    /// <param name="physics">物理伤害</param>
    /// <param name="magic">魔法伤害</param>
    public void BeInjured(int physics = 0, int magic = 0)
    {
        if (hp <= 0) return;
        int damage = GetPhysice(physics) + GetMagic(magic);
        hp -= damage;

        // 受伤动画
        if (damage > damageAnimNum && hitNum >= hitInterval)
        {
            hitNum = 0;
            if (monsterAnimator != null)
            {
                monsterAnimator.PlayAnimator(MonsterAnimator.MonsterAnimEnum.Hit);
            }
        }
        // 修改ui TODD

        if (hp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 怪物死亡的处理
    /// </summary>
    private void Die()
    {
        // 播放死亡动画，并销毁物体
        if (monsterAnimator != null)
        {
            monsterAnimator.PlayAnimator(MonsterAnimator.MonsterAnimEnum.Die);
        }
        // 死亡后掉落物品 TODD

        // 掉落物品ui显示 TODD

        Destroy(gameObject, 2);
    }

    private void Update()
    {
        hitNum += Time.deltaTime;
    }

    /// <summary>
    /// 受到的物理伤害的计算公式
    /// </summary>
    /// <param name="physice">所受到的物理伤害</param>
    /// <returns></returns>
    private int GetPhysice(int physice)
    {
        if (physice <= 0) return 0;
        int t = physice - physicsDefense;
        return t <= 0 ? 1 : t;
    }

    /// <summary>
    /// 受到的魔法伤害的计算公式
    /// </summary>
    /// <param name="physice">所受到的物理伤害</param>
    /// <returns></returns>
    private int GetMagic(int magic)
    {
        if (magic <= 0) return 0;
        int t = magic - magicDefense;
        return t <= 0 ? 1 : t;
    }
}
