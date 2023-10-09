using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public class DamageManager
    {
        /// <summary>
        /// 通过对方的物理攻击力和自己的防御计算受到的物理伤害，
        /// </summary>
        /// <param name="attack">对方的物理攻击力</param>
        /// <param name="defensive">自己的防御力</param>
        /// <returns></returns>
        public static float GetDamage(float attack, float defensive)
        {
            if (attack <= 0) { return 0; }
            float damage = (attack + defensive) / defensive;
            return damage <= 1 ? 1 : Mathf.Clamp(damage, 1, damage);
        }
        // 魔法伤害
        public static float GetMagicDamage(float magicAttack, float magicDefensive)
        {
            if (magicAttack <= 0) { return 0; }
            float damage = (magicAttack + magicDefensive) / magicDefensive;
            return damage <= 1 ? 1 : Mathf.Clamp(damage, 1, damage);
        }

    }
}
