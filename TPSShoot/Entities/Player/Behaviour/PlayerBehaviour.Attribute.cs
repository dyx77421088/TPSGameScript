using Bags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 角色属性
    /// </summary>
    public partial class PlayerBehaviour
    {
        private int maxHP, currentHP; // 最大血量和当前血量
        private Player playerAttribute; // 角色的属性


        /// <summary>
        /// 获得最大血量
        /// </summary>
        public int GetMaxHP()
        {
            return maxHP = playerAttribute.CurrentHP;
        }
        // 当前的血量
        public int GetCurrentHP() { return  currentHP; }
        /// <summary>
        /// 对角色造成伤害
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
            if(attack <= 0) { return 0; }
            int damage = attack - playerAttribute.CurrentDefensive;
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
            int damage = magicAttack - playerAttribute.CurrentMagicAggressivity;
            return damage <= 1 ? 1 : Mathf.Clamp(damage, 1, damage);
        }

        public void AddHP(int addHP)
        {
            GetMaxHP();
            currentHP += addHP;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP); // 血量限制在0-最大血量

            // 通知订阅
            Events.PlayerChangeCurrentHP.Call();
            if (currentHP <= 0)
            {
                // 死亡处理
            }
        }

    }
}
