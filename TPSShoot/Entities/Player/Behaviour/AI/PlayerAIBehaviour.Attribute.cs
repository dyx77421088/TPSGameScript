using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using TPSShoot.UI;
using TPSShoot.Utils;
using UnityEditor;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// ��ɫ����
    /// </summary>
    public partial class PlayerAIBehaviour
    {
        public Sprite avatar;
        public float maxHP, currentHP; // ���Ѫ���͵�ǰѪ��
        private float maxMP, currentMP; // ��������͵�ǰ����
        

        #region һЩget
        public Sprite Avatar {  get { return avatar; } }
        //������Ѫ��
        public float MaxHP { get => maxHP = aiAttribute.currentHP; }
        // ��ǰ��Ѫ��
        public float CurrentHP { get => currentHP; }
        //����������
        public float MaxMP { get => maxMP; }
        // ��ǰ������
        public float CurrentMP { get => currentMP; }
        // ��ǰ�ĵȼ�
        public int CurrentGrade { get => aiAttribute.grade; }
        #endregion
        private void OnInitAttribute()
        {
            aiAttribute.grade = RandomUtils.RandomInt(aiAttribute.minGrade, aiAttribute.maxGrade);
            aiAttribute.StartInit();
            currentHP = aiAttribute.currentHP;
        }
        
        public float GetAggressivity()
        {
            return aiAttribute.aggressivity;
        }
        /// <summary>
        /// ��ʼ��Ѫ��
        /// </summary>
        public void InitHP()
        {
            currentHP = maxHP = aiAttribute.currentHP;
        }
        public void InitMP()
        {
            currentMP = maxMP = 100;
        }
        /// <summary>
        /// �Խ�ɫ����˺�
        /// </summary>
        /// <param name="attack">������</param>
        /// <param name="magicAttack">ħ������</param>
        private void Hit(float grade, float attack = 0, float magicAttack = 0)
        {
            attack = Mathf.Clamp(attack, 0, 9999);
            magicAttack = Mathf.Clamp(magicAttack, 0, 9999);
            if (attack == 0 && magicAttack == 0) { return; }

            float damage = DamageManager.GetDamage(attack, aiAttribute.defensive) 
                + DamageManager.GetMagicDamage(magicAttack, aiAttribute.magicDefensive);
            if (damage > 0)
            {
                AddHP(-damage);
                Events.PlayerAIHit.Call(this);
            }
        }


        public void AddHP(float addHP)
        {
            if (!IsAlive) return;
            maxHP = aiAttribute.currentHP;
            currentHP += addHP;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP); // Ѫ��������0-���Ѫ��

            // ֪ͨ����
            //Events.PlayerChangeCurrentHP.Call();
            //if ()
            if (currentHP <= 0)
            {
                // ��������
                _animator.SetTrigger(PlayerAnimatorParameter.diedTrigger);
                isAlive = false;
                Destroy(gameObject, 15);
                Events.PlayerKillMonster.Call(aiAttribute);
            }
        }
        public void AddMP(float addMP)
        {
            currentMP += addMP;
            currentMP = Mathf.Clamp(currentMP, 0, maxMP); // ����������0-�������

            // ֪ͨ����
            Events.PlayerChangeCurrentMP.Call();
        }
        /// <summary>
        /// ÿ���Ѫ������
        /// </summary>
        private IEnumerator BloodReturn()
        {
            while (IsAlive)
            {
                yield return new WaitForSeconds(1);
                AddHP(aiAttribute.returnHP);
                AddMP(1);
                if (IsHit) Events.PlayerAIAddHP.Call(this);
            }
        }
        
    }
}
