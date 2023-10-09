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
        private void OnHit(int playerGrade, int attack = 0, int magicAttack = 0)
        {
            float damage;
            monsterAttribute.OnHit(playerGrade, out damage, attack, magicAttack);

            if(damage > 0)
            {
                AddHP(-damage);
                // 受到超过百分之10以上的伤害
                if (damage > monsterAttribute.GetMaxHP() * 0.1)
                {
                    // 播放受伤动画
                    onMonsterHitAnim?.Invoke();
                }

                // 通知显示
                Events.MonsterHit.Call(monsterAttribute);
            }
        }
        public void AddHP(float addHP)
        {
            monsterAttribute.currentHP += addHP;
            monsterAttribute.currentHP = Mathf.Clamp(monsterAttribute.currentHP, 0, monsterAttribute.maxHP); // 血量限制在0-最大血量

            onMonsterHPChange?.Invoke();
            if (monsterAttribute.currentHP <= 0)
            {
                // 死亡处理
                onMonsterDied?.Invoke();
            }
        }
    }
}
