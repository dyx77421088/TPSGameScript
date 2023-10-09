using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class MonsterAttribute
    {
        [Tooltip("头像")] public Sprite avatar;
        [Tooltip("类型")] public MonsterType type;
        [Tooltip("最大血量")] public float maxHP = 100;
        [Tooltip("当前血量")] public float currentHP = 100;
        [Tooltip("防御力")] public float defensive = 10;
        [Tooltip("魔法防御力")] public float magicDefensive = 10;
        [Tooltip("物理攻击力")] public float aggressivity = 10;
        [Tooltip("魔法攻击力")] public float magicAggressivity = 10;
        [Tooltip("在idle状态下每10帧回血")] public float returnHP = 1;
        [Tooltip("最低等级")] public int minGrade = 1;
        [Tooltip("最高等级")] public int maxGrade = 50;

        [Header("每级增加的属性")]
        [Tooltip("血量")] public float addHP = 10;
        [Tooltip("防御力")] public float addDefensive = 1;
        [Tooltip("魔法防御力")] public float addMagicDefensive = 1;
        [Tooltip("物理攻击力")] public float addAggressivity = 1;
        [Tooltip("魔法攻击力")] public float addMagicAggressivity = 1;
        [Tooltip("在idle状态下每10帧回血")] public float addReturnHP = 1;

        [HideInInspector] public int grade; // 当前的等级
        [HideInInspector] public int exp; // 击败可获得的经验


        public int GetExp { get => exp; }

        public void StartInit()
        {
            InitExp();
            InitAttributeByGrade();
        }
        // 初始化经验
        private void InitExp()
        {
            int exp = 1; // 1级的经验
            for (int i = 2; i < grade; ++i)
            {
                if (i <= 10) exp += i;
                else if (i < 20) exp = (int)(exp * 1.1);
                else if (i <= 50) exp = exp + (i * 10);
                else exp = 999;
            }
            this.exp = exp;
        }
        /// <summary>
        /// 获得最大血量
        /// </summary>
        public float GetMaxHP()
        {
            return maxHP;
        }
        // 当前的血量
        public float GetCurrentHP() { return currentHP; }
        /// <summary>
        /// 获得血量百分比
        /// </summary>
        public float GetHPPercentage()
        {
            return currentHP * 1.0f / GetMaxHP();
        }
        /// <summary>
        /// 根据等级初始化属性
        /// </summary>
        private void InitAttributeByGrade()
        {
            // 血量
            maxHP += addHP * grade;
            currentHP = maxHP;
            // 属性
            defensive += addDefensive * grade;
            magicDefensive += addMagicDefensive * grade;
            aggressivity += addAggressivity * grade;
            magicAggressivity += addMagicAggressivity * grade;
            returnHP += addReturnHP * grade;
        }
        private float hitCD = 5;
        private float lastTime;
        /// <summary>
        /// 对怪物造成伤害
        /// </summary>
        /// <param name="playerGrade">玩家的等级</param>
        /// <param name="attack">物理攻击</param>
        /// <param name="magicAttack">魔法攻击</param>
        public void OnHit(int playerGrade, out float outDamage, int attack = 0, int magicAttack = 0)
        {
            float t = Time.time;
            attack = Mathf.Clamp(attack, 0, 9999);
            magicAttack = Mathf.Clamp(magicAttack, 0, 9999);
            if (attack == 0 && magicAttack == 0) { outDamage = 0; return; }

            float damage = DamageManager.GetDamage(attack, defensive)
                + DamageManager.GetMagicDamage(magicAttack, magicAggressivity);

            outDamage = damage;
        }


    }

}