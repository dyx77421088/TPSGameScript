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
        private void OnHit(int playerGrade, int attack = 0, int magicAttack = 0)
        {
            float damage;
            monsterAttribute.OnHit(playerGrade, out damage, attack, magicAttack);

            if(damage > 0)
            {
                AddHP(-damage);
                // �ܵ������ٷ�֮10���ϵ��˺�
                if (damage > monsterAttribute.GetMaxHP() * 0.1)
                {
                    // �������˶���
                    onMonsterHitAnim?.Invoke();
                }

                // ֪ͨ��ʾ
                Events.MonsterHit.Call(monsterAttribute);
            }
        }
        public void AddHP(float addHP)
        {
            monsterAttribute.currentHP += addHP;
            monsterAttribute.currentHP = Mathf.Clamp(monsterAttribute.currentHP, 0, monsterAttribute.maxHP); // Ѫ��������0-���Ѫ��

            onMonsterHPChange?.Invoke();
            if (monsterAttribute.currentHP <= 0)
            {
                // ��������
                onMonsterDied?.Invoke();
            }
        }
    }
}
