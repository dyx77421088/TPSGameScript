using Bags;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// monster����
    /// </summary>
    
    public partial class MonsterBehaviour
    {
        /// <summary>
        /// ������Ѫ��
        /// </summary>
        public int GetMaxHP()
        {
            return monsterAttribute.maxHP;
        }
        // ��ǰ��Ѫ��
        public int GetCurrentHP() { return monsterAttribute.currentHP; }
        /// <summary>
        /// ���Ѫ���ٷֱ�
        /// </summary>
        public float GetHPPercentage()
        {
            return monsterAttribute.currentHP * 1.0f / monsterAttribute.maxHP;
        }
        /// <summary>
        /// �Թ�������˺�
        /// </summary>
        /// <param name="attack">������</param>
        /// <param name="magicAttack">ħ������</param>
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
        /// ����ܵ��������˺�ֵ
        /// �˺� - �������������ܵ�һ���˺�
        /// </summary>
        /// <returns></returns>
        private int GetDamage(int attack)
        {
            if (attack <= 0) { return 0; }
            int damage = attack - monsterAttribute.defensive;
            return damage <= 1 ? 1 : Mathf.Clamp(damage, 1, damage);
        }
        /// <summary>
        /// ����ܵ���ħ���˺�ֵ
        /// �˺� - ħ���������������ܵ�һ���˺�
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
            monsterAttribute.currentHP = Mathf.Clamp(monsterAttribute.currentHP, 0, monsterAttribute.maxHP); // Ѫ��������0-���Ѫ��

            // ֪ͨ����
            onMonsterHPChange?.Invoke();
            if (monsterAttribute.currentHP <= 0)
            {
                // ��������
                onMonsterDied?.Invoke();
            }
        }


        [Serializable]
        public class MonsterAttribute
        {
            [Tooltip("���Ѫ��")] public int maxHP = 100;
            [Tooltip("��ǰѪ��")] public int currentHP = 100;
            [Tooltip("������")] public int defensive = 10;
            [Tooltip("ħ��������")] public int magicDefensive = 10;
            [Tooltip("��������")] public int aggressivity = 10;
            [Tooltip("ħ��������")] public int magicAggressivity = 10;
            [Tooltip("��idle״̬��ÿ10֡��Ѫ")] public int addHP = 1;
        }
    }
}
