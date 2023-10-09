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
    /// 角色属性
    /// </summary>
    public partial class PlayerAIBehaviour
    {
        public Sprite avatar;
        public float maxHP, currentHP; // 最大血量和当前血量
        private float maxMP, currentMP; // 最大蓝量和当前蓝量
        

        #region 一些get
        public Sprite Avatar {  get { return avatar; } }
        //获得最大血量
        public float MaxHP { get => maxHP = aiAttribute.currentHP; }
        // 当前的血量
        public float CurrentHP { get => currentHP; }
        //获得最大蓝量
        public float MaxMP { get => maxMP; }
        // 当前的蓝量
        public float CurrentMP { get => currentMP; }
        // 当前的等级
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
        /// 初始化血量
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
        /// 对角色造成伤害
        /// </summary>
        /// <param name="attack">物理攻击</param>
        /// <param name="magicAttack">魔法攻击</param>
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
            currentHP = Mathf.Clamp(currentHP, 0, maxHP); // 血量限制在0-最大血量

            // 通知订阅
            //Events.PlayerChangeCurrentHP.Call();
            //if ()
            if (currentHP <= 0)
            {
                // 死亡处理
                _animator.SetTrigger(PlayerAnimatorParameter.diedTrigger);
                isAlive = false;
                Destroy(gameObject, 15);
                Events.PlayerKillMonster.Call(aiAttribute);
            }
        }
        public void AddMP(float addMP)
        {
            currentMP += addMP;
            currentMP = Mathf.Clamp(currentMP, 0, maxMP); // 蓝量限制在0-最大蓝量

            // 通知订阅
            Events.PlayerChangeCurrentMP.Call();
        }
        /// <summary>
        /// 每秒回血，回蓝
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
