using Bags;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// monster属性
    /// </summary>
    
    public partial class MonsterBehaviour
    {
        /// <summary>
        /// 获得最大血量
        /// </summary>
        public int GetMaxHP()
        {
            return monsterAttribute.maxHP;
        }
        // 当前的血量
        public int GetCurrentHP() { return monsterAttribute.currentHP; }
        /// <summary>
        /// 获得血量百分比
        /// </summary>
        public float GetHPPercentage()
        {
            return monsterAttribute.currentHP * 1.0f / monsterAttribute.maxHP;
        }
        /// <summary>
        /// 对怪物造成伤害
        /// </summary>
        /// <param name="attack">物理攻击</param>
        /// <param name="magicAttack">魔法攻击</param>
        public void OnHit(int attack = 0, int magicAttack = 0)
        {
            attack = Mathf.Clamp(attack, 0, 9999);
            magicAttack = Mathf.Clamp(magicAttack, 0, 9999);
            if (attack == 0 && magicAttack == 0) { return; }

            int damage = GetDamage(attack) + GetMagicDamage(magicAttack);

            if (damage > 0)
            {
                AddHP(-damage);
            }
        }

        /// <summary>
        /// 获得受到的物理伤害值
        /// 伤害 - 防御力，最少受到一点伤害
        /// </summary>
        /// <returns></returns>
        private int GetDamage(int attack)
        {
            if (attack <= 0) { return 0; }
            int damage = attack - monsterAttribute.defensive;
            return damage <= 1 ? 1 : Mathf.Clamp(damage, 1, damage);
        }
        /// <summary>
        /// 获得受到的魔法伤害值
        /// 伤害 - 魔法防御力，最少受到一点伤害
        /// </summary>
        /// <returns></returns>
        private int GetMagicDamage(int magicAttack)
        {
            if (magicAttack <= 0) { return 0; }
            int damage = magicAttack - monsterAttribute.magicAggressivity;
            return damage <= 1 ? 1 : Mathf.Clamp(damage, 1, damage);
        }

        public void AddHP(int addHP)
        {
            GetMaxHP();
            monsterAttribute.currentHP += addHP;
            monsterAttribute.currentHP = Mathf.Clamp(monsterAttribute.currentHP, 0, monsterAttribute.maxHP); // 血量限制在0-最大血量

            // 通知订阅
            onMonsterHPChange?.Invoke();
            if (monsterAttribute.currentHP <= 0)
            {
                // 死亡处理
                onMonsterDied?.Invoke();
            }
        }


        [Serializable]
        public class MonsterAttribute
        {
            [Tooltip("最大血量")] public int maxHP = 100;
            [Tooltip("当前血量")] public int currentHP = 100;
            [Tooltip("防御力")] public int defensive = 10;
            [Tooltip("魔法防御力")] public int magicDefensive = 10;
            [Tooltip("物理攻击力")] public int aggressivity = 10;
            [Tooltip("魔法攻击力")] public int magicAggressivity = 10;
            [Tooltip("在idle状态下每10帧回血")] public int addHP = 1;
        }
    }
}
