using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using TPSShoot.UI;
using UnityEditor;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 角色属性
    /// </summary>
    public partial class PlayerBehaviour
    {
        private float maxHP, currentHP; // 最大血量和当前血量
        private float maxMP, currentMP; // 最大蓝量和当前蓝量
        private int grade = 1; // 等级
        private int upgradeExp, currentExp; // 升级经验和当前经验
        public Attribute playerAttribute; // 角色的属性

        #region 一些get

        //获得最大血量
        public float MaxHP { get => maxHP = playerAttribute.CurrentHP; }
        // 当前的血量
        public float CurrentHP { get => currentHP; }
        //获得最大蓝量
        public float MaxMP { get => maxMP; }
        // 当前的蓝量
        public float CurrentMP { get => currentMP; }
        // 升级所需的经验
        public float UpgradeEXP { get => upgradeExp; }
        // 当前的经验
        public float CurrentEXP { get => currentExp; }
        // 当前的等级
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
        /// 升级请求
        /// </summary>
        private void OnUpgradeRequest()
        {
            if (currentExp < upgradeExp) return; // 不能升级
            grade++;
            Debug.Log("升级");
            currentExp -= upgradeExp; // 升级
            Events.PlayerChangeEXP.Call();
            Events.PlayerGradeChange.Call();
            UpdateUpgradeExp();
        }
        
        private void OnPlayerKillMonster(MonsterAttribute attr)
        {
            float exp = attr.exp;

            // 根据角色和monster的等级差，修改经验的获得
            int cha = Mathf.Abs(attr.grade - grade);
            if (cha > 30) exp *= 0.2f;
            else if (cha > 20) exp *= 0.5f;
            else if (cha > 10) exp *= 0.8f;

            // 根据击杀monster的类型，调整经验
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

            // 获得经验
            Events.PlayerAddExp.Call(addExp);
            string expStr = string.Format("<color={0}><size=18>经验+{1}</size></color>"
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
        /// 初始化血量
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
        /// 对角色造成伤害
        /// </summary>
        /// <param name="attack">物理攻击</param>
        /// <param name="magicAttack">魔法攻击</param>
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
            currentHP = Mathf.Clamp(currentHP, 0, maxHP); // 血量限制在0-最大血量

            // 通知订阅
            Events.PlayerChangeCurrentHP.Call();
            if (currentHP <= 0)
            {
                // 死亡处理
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
                AddHP(playerAttribute.CurrentBloodReturn);
                AddMP(1);
            }
        }
        // 升级所需的经验
        private void UpdateUpgradeExp()
        {
            upgradeExp = Mathf.Max(5, upgradeExp);
            // 10级之前每次升级经验乘以2,(10, 20, 40, 80, 160, 320, 640, 1280, 2560)
            if (grade < 9) upgradeExp *= 2; // 1-9
            else if (grade == 9) upgradeExp = 5000; // 9-10级需要的经验为5000
            else if (grade < 29) upgradeExp = (int)(upgradeExp * 1.1);
            else if (grade == 29) upgradeExp = 50000; // 29-30 需要50000经验
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
            #region 基础属性
            private int damage;
            private int baseHP = 100; // 基础血量
            private int baseStrength = 10; // 力量
            private int baseIntellect = 10; // 智力
            private int baseAgility = 10; // 敏捷
            private int baseStamina = 10; // 体力
            public int BaseStrength { get => baseStrength; set => baseStrength = value; }

            public int BaseIntellect { get => baseIntellect; set => baseIntellect = value; }
            public int BaseAgility { get => baseAgility; set => baseAgility = value; }
            public int BaseStamina { get => baseStamina; set => baseStamina = value; }
            #endregion

            #region 加上装备后的属性
            private int currentStrength = 10;
            private int currentIntellect = 10;
            private int currentAgility = 10;
            private int currentStamina = 10;

            public int Damage { get => damage; set => damage = value; }
            public int CurrentStrength { get => currentStrength; set => currentStrength = value; } // 力量
            public int CurrentIntellect { get => currentIntellect; set => currentIntellect = value; } // 智力
            public int CurrentAgility { get => currentAgility; set => currentAgility = value; } // 敏捷
            public int CurrentStamina { get => currentStamina; set => currentStamina = value; } // 体力

            // 其它的属性
            public int CurrentHP
            {
                get => (int)(baseHP
                    + currentStrength * _attributte.strength2HP
                    + currentStamina * _attributte.stamina2HP);
            } // 血量
            public int CurrentDefensive
            {
                get => (int)(
                    currentStrength * _attributte.strength2Defensive
                    + currentStamina * _attributte.stamina2Defensive
                    );
            } // 防御力
            public int CurrentMagicDefensive
            {
                get => (int)(
                    currentStamina * _attributte.stamina2MagicDefensive
                    + currentIntellect * _attributte.intellect2MagicDefensive
                    );
            } // 魔法防御力
            public int CurrentAggressivity { get => (int)(currentStrength * _attributte.strength2Aggressivity) + damage; } // 物理攻击力
            public int CurrentMagicAggressivity { get => (int)(currentIntellect * _attributte.intellect2MagicAggressivity) + damage; } // 魔法攻击力
            public float CurrentCritical { get => currentAgility * _attributte.agility2Critical; } // 暴击
            public float CurrentSpeed { get => currentAgility * _attributte.agility2Speed; } // 攻速
            public float CurrentBloodReturn { get => CurrentStamina * _attributte.stamina2BloodReturn; }
            
            #endregion

            #region 一些方法
            public string GetAttributeString()
            {
                string text = string.Format(
                            "<color=#7CFFF0>力量</color> <color=#ffffff>{0}</color>      <color=#7CFFF0>物理攻击</color> <color=#ffffff>{4}</color>\r\n" +
                            "<color=#7CFFF0>智力</color> <color=#ffffff>{1}</color>      <color=#7CFFF0>法术攻击</color> <color=#ffffff>{5}</color>\r\n" +
                            "<color=#7CFFF0>敏捷</color> <color=#ffffff>{2}</color>      <color=#7CFFF0>暴击率</color> <color=#ffffff>{6}%</color>\r\n" +
                            "<color=#7CFFF0>体力</color> <color=#ffffff>{3}</color>      <color=#7CFFF0>气血</color> <color=#ffffff>{7}</color>\r\n" +
                            "<color=#7CFFF0>防御力</color> <color=#ffffff>{8}</color>    <color=#7CFFF0>法术防御</color> <color=#ffffff>{8}</color>"
                            , CurrentStrength, CurrentIntellect, CurrentAgility, CurrentStamina,
                            CurrentAggressivity, CurrentMagicAggressivity, CurrentCritical, CurrentHP, CurrentDefensive,CurrentMagicDefensive
                            );

                return text;
            }
            #endregion
        }
    }
}
