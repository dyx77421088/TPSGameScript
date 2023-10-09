using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using TPSShoot.UI;
using UnityEditor;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// ��ɫ����
    /// </summary>
    public partial class PlayerBehaviour
    {
        private float maxHP, currentHP; // ���Ѫ���͵�ǰѪ��
        private float maxMP, currentMP; // ��������͵�ǰ����
        private int grade = 1; // �ȼ�
        private int upgradeExp, currentExp; // ��������͵�ǰ����
        public Attribute playerAttribute; // ��ɫ������

        #region һЩget

        //������Ѫ��
        public float MaxHP { get => maxHP = playerAttribute.CurrentHP; }
        // ��ǰ��Ѫ��
        public float CurrentHP { get => currentHP; }
        //����������
        public float MaxMP { get => maxMP; }
        // ��ǰ������
        public float CurrentMP { get => currentMP; }
        // ��������ľ���
        public float UpgradeEXP { get => upgradeExp; }
        // ��ǰ�ľ���
        public float CurrentEXP { get => currentExp; }
        // ��ǰ�ĵȼ�
        public int CurrentGrade { get => grade; }
        #endregion
        private void OnInitAttribute()
        {
            playerAttribute = new Attribute(attributteSettings);
            grade = 1;
            InitHP();
            InitMP();
            UpdateUpgradeExp();
        }
        /// <summary>
        /// ��������
        /// </summary>
        private void OnUpgradeRequest()
        {
            if (currentExp < upgradeExp) return; // ��������
            grade++;
            Debug.Log("����");
            currentExp -= upgradeExp; // ����
            Events.PlayerChangeEXP.Call();
            Events.PlayerGradeChange.Call();
            UpdateUpgradeExp();
        }
        
        private void OnPlayerKillMonster(MonsterAttribute attr)
        {
            float exp = attr.exp;

            // ���ݽ�ɫ��monster�ĵȼ���޸ľ���Ļ��
            int cha = Mathf.Abs(attr.grade - grade);
            if (cha > 30) exp *= 0.2f;
            else if (cha > 20) exp *= 0.5f;
            else if (cha > 10) exp *= 0.8f;

            // ���ݻ�ɱmonster�����ͣ���������
            switch (attr.type)
            {
                case MonsterType.Bone:
                    exp *= 0.8f;
                    break;
                case MonsterType.Broadsword:
                    exp *= 1.5f;
                    break;
                case MonsterType.Boss1:
                    exp *= 3f;
                    break;
                case MonsterType.AI:
                    exp *= 5f;
                    break;
                default:
                    break;
            }

            int addExp = (int)Mathf.Clamp(exp, 1, exp);

            // ��þ���
            Events.PlayerAddExp.Call(addExp);
            string expStr = string.Format("<color={0}><size=18>����+{1}</size></color>"
                , "#ffff00", addExp);
            Events.PlayerInfoTipShow.Call(expStr, PlayerInfoTipUI.PlayerInfoTipPoint.Center);
        }

        private void OnPlayerAddExp(int addExp)
        {
            currentExp += addExp;
            if (currentExp >= upgradeExp)
            {
                Events.PlayerUpgradeRequest.Call();
            }

            Events.PlayerChangeEXP.Call();
        }
        public int GetAggressivity()
        {
            return playerAttribute.CurrentAggressivity;
        }
        /// <summary>
        /// ��ʼ��Ѫ��
        /// </summary>
        public void InitHP()
        {
            currentHP = maxHP = playerAttribute.CurrentHP;
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
        public void OnHit(float attack = 0, float magicAttack = 0)
        {
            attack = Mathf.Clamp(attack, 0, 9999);
            magicAttack = Mathf.Clamp(magicAttack, 0, 9999);
            if (attack == 0 && magicAttack == 0) { return; }

            float damage = DamageManager.GetDamage(attack, playerAttribute.CurrentDefensive)
                + DamageManager.GetMagicDamage(magicAttack, playerAttribute.CurrentMagicDefensive);

            if (damage > 0)
            {
                AddHP(-damage);
            }
        }


        public void AddHP(float addHP)
        {
            maxHP = playerAttribute.CurrentHP;
            currentHP += addHP;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP); // Ѫ��������0-���Ѫ��

            // ֪ͨ����
            Events.PlayerChangeCurrentHP.Call();
            if (currentHP <= 0)
            {
                // ��������
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
                AddHP(playerAttribute.CurrentBloodReturn);
                AddMP(1);
            }
        }
        // ��������ľ���
        private void UpdateUpgradeExp()
        {
            upgradeExp = Mathf.Max(5, upgradeExp);
            // 10��֮ǰÿ�������������2,(10, 20, 40, 80, 160, 320, 640, 1280, 2560)
            if (grade < 9) upgradeExp *= 2; // 1-9
            else if (grade == 9) upgradeExp = 5000; // 9-10����Ҫ�ľ���Ϊ5000
            else if (grade < 29) upgradeExp = (int)(upgradeExp * 1.1);
            else if (grade == 29) upgradeExp = 50000; // 29-30 ��Ҫ50000����
            else if (grade < 49) upgradeExp = (int)(upgradeExp * 1.2);
            else upgradeExp = 2000000;

            
        }
        public class Attribute
        {
            private PlayerAttributteSettings _attributte;
            public Attribute(PlayerAttributteSettings settings)
            {
                _attributte = settings;
            }
            #region ��������
            private int damage;
            private int baseHP = 100; // ����Ѫ��
            private int baseStrength = 10; // ����
            private int baseIntellect = 10; // ����
            private int baseAgility = 10; // ����
            private int baseStamina = 10; // ����
            public int BaseStrength { get => baseStrength; set => baseStrength = value; }

            public int BaseIntellect { get => baseIntellect; set => baseIntellect = value; }
            public int BaseAgility { get => baseAgility; set => baseAgility = value; }
            public int BaseStamina { get => baseStamina; set => baseStamina = value; }
            #endregion

            #region ����װ���������
            private int currentStrength = 10;
            private int currentIntellect = 10;
            private int currentAgility = 10;
            private int currentStamina = 10;

            public int Damage { get => damage; set => damage = value; }
            public int CurrentStrength { get => currentStrength; set => currentStrength = value; } // ����
            public int CurrentIntellect { get => currentIntellect; set => currentIntellect = value; } // ����
            public int CurrentAgility { get => currentAgility; set => currentAgility = value; } // ����
            public int CurrentStamina { get => currentStamina; set => currentStamina = value; } // ����

            // ����������
            public int CurrentHP
            {
                get => (int)(baseHP
                    + currentStrength * _attributte.strength2HP
                    + currentStamina * _attributte.stamina2HP);
            } // Ѫ��
            public int CurrentDefensive
            {
                get => (int)(
                    currentStrength * _attributte.strength2Defensive
                    + currentStamina * _attributte.stamina2Defensive
                    );
            } // ������
            public int CurrentMagicDefensive
            {
                get => (int)(
                    currentStamina * _attributte.stamina2MagicDefensive
                    + currentIntellect * _attributte.intellect2MagicDefensive
                    );
            } // ħ��������
            public int CurrentAggressivity { get => (int)(currentStrength * _attributte.strength2Aggressivity) + damage; } // ��������
            public int CurrentMagicAggressivity { get => (int)(currentIntellect * _attributte.intellect2MagicAggressivity) + damage; } // ħ��������
            public float CurrentCritical { get => currentAgility * _attributte.agility2Critical; } // ����
            public float CurrentSpeed { get => currentAgility * _attributte.agility2Speed; } // ����
            public float CurrentBloodReturn { get => CurrentStamina * _attributte.stamina2BloodReturn; }
            
            #endregion

            #region һЩ����
            public string GetAttributeString()
            {
                string text = string.Format(
                            "<color=#7CFFF0>����</color> <color=#ffffff>{0}</color>      <color=#7CFFF0>������</color> <color=#ffffff>{4}</color>\r\n" +
                            "<color=#7CFFF0>����</color> <color=#ffffff>{1}</color>      <color=#7CFFF0>��������</color> <color=#ffffff>{5}</color>\r\n" +
                            "<color=#7CFFF0>����</color> <color=#ffffff>{2}</color>      <color=#7CFFF0>������</color> <color=#ffffff>{6}%</color>\r\n" +
                            "<color=#7CFFF0>����</color> <color=#ffffff>{3}</color>      <color=#7CFFF0>��Ѫ</color> <color=#ffffff>{7}</color>\r\n" +
                            "<color=#7CFFF0>������</color> <color=#ffffff>{8}</color>    <color=#7CFFF0>��������</color> <color=#ffffff>{8}</color>"
                            , CurrentStrength, CurrentIntellect, CurrentAgility, CurrentStamina,
                            CurrentAggressivity, CurrentMagicAggressivity, CurrentCritical, CurrentHP, CurrentDefensive,CurrentMagicDefensive
                            );

                return text;
            }
            #endregion
        }
    }
}
