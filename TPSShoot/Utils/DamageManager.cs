using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public class DamageManager
    {
        /// <summary>
        /// ͨ���Է��������������Լ��ķ��������ܵ��������˺���
        /// </summary>
        /// <param name="attack">�Է�����������</param>
        /// <param name="defensive">�Լ��ķ�����</param>
        /// <returns></returns>
        public static float GetDamage(float attack, float defensive)
        {
            if (attack <= 0) { return 0; }
            float damage = (attack + defensive) / defensive;
            return damage <= 1 ? 1 : Mathf.Clamp(damage, 1, damage);
        }
        // ħ���˺�
        public static float GetMagicDamage(float magicAttack, float magicDefensive)
        {
            if (magicAttack <= 0) { return 0; }
            float damage = (magicAttack + magicDefensive) / magicDefensive;
            return damage <= 1 ? 1 : Mathf.Clamp(damage, 1, damage);
        }

    }
}
