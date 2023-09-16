using Bags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// ��ɫ����
    /// </summary>
    public partial class PlayerBehaviour
    {
        private int maxHP, currentHP; // ���Ѫ���͵�ǰѪ��
        private Player playerAttribute; // ��ɫ������


        /// <summary>
        /// ������Ѫ��
        /// </summary>
        public int GetMaxHP()
        {
            return maxHP = playerAttribute.CurrentHP;
        }
        // ��ǰ��Ѫ��
        public int GetCurrentHP() { return  currentHP; }
        /// <summary>
        /// �Խ�ɫ����˺�
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
            if(attack <= 0) { return 0; }
            int damage = attack - playerAttribute.CurrentDefensive;
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
            int damage = magicAttack - playerAttribute.CurrentMagicAggressivity;
            return damage <= 1 ? 1 : Mathf.Clamp(damage, 1, damage);
        }

        public void AddHP(int addHP)
        {
            GetMaxHP();
            currentHP += addHP;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP); // Ѫ��������0-���Ѫ��

            // ֪ͨ����
            Events.PlayerChangeCurrentHP.Call();
            if (currentHP <= 0)
            {
                // ��������
            }
        }

    }
}
