using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class MonsterAttribute
    {
        [Tooltip("ͷ��")] public Sprite avatar;
        [Tooltip("����")] public MonsterType type;
        [Tooltip("���Ѫ��")] public float maxHP = 100;
        [Tooltip("��ǰѪ��")] public float currentHP = 100;
        [Tooltip("������")] public float defensive = 10;
        [Tooltip("ħ��������")] public float magicDefensive = 10;
        [Tooltip("��������")] public float aggressivity = 10;
        [Tooltip("ħ��������")] public float magicAggressivity = 10;
        [Tooltip("��idle״̬��ÿ10֡��Ѫ")] public float returnHP = 1;
        [Tooltip("��͵ȼ�")] public int minGrade = 1;
        [Tooltip("��ߵȼ�")] public int maxGrade = 50;

        [Header("ÿ�����ӵ�����")]
        [Tooltip("Ѫ��")] public float addHP = 10;
        [Tooltip("������")] public float addDefensive = 1;
        [Tooltip("ħ��������")] public float addMagicDefensive = 1;
        [Tooltip("��������")] public float addAggressivity = 1;
        [Tooltip("ħ��������")] public float addMagicAggressivity = 1;
        [Tooltip("��idle״̬��ÿ10֡��Ѫ")] public float addReturnHP = 1;

        [HideInInspector] public int grade; // ��ǰ�ĵȼ�
        [HideInInspector] public int exp; // ���ܿɻ�õľ���


        public int GetExp { get => exp; }

        public void StartInit()
        {
            InitExp();
            InitAttributeByGrade();
        }
        // ��ʼ������
        private void InitExp()
        {
            int exp = 1; // 1���ľ���
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
        /// ������Ѫ��
        /// </summary>
        public float GetMaxHP()
        {
            return maxHP;
        }
        // ��ǰ��Ѫ��
        public float GetCurrentHP() { return currentHP; }
        /// <summary>
        /// ���Ѫ���ٷֱ�
        /// </summary>
        public float GetHPPercentage()
        {
            return currentHP * 1.0f / GetMaxHP();
        }
        /// <summary>
        /// ���ݵȼ���ʼ������
        /// </summary>
        private void InitAttributeByGrade()
        {
            // Ѫ��
            maxHP += addHP * grade;
            currentHP = maxHP;
            // ����
            defensive += addDefensive * grade;
            magicDefensive += addMagicDefensive * grade;
            aggressivity += addAggressivity * grade;
            magicAggressivity += addMagicAggressivity * grade;
            returnHP += addReturnHP * grade;
        }
        private float hitCD = 5;
        private float lastTime;
        /// <summary>
        /// �Թ�������˺�
        /// </summary>
        /// <param name="playerGrade">��ҵĵȼ�</param>
        /// <param name="attack">������</param>
        /// <param name="magicAttack">ħ������</param>
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