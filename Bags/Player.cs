using System.Collections;
using System.Collections.Generic;
using TPSShoot;
using UnityEngine;

namespace Bags
{
    public class Player
    {
        private PlayerAttributteSettings _attributte = new PlayerAttributteSettings();
        #region 基础属性
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

        public int CurrentStrength { get => currentStrength; set => currentStrength = value; } // 力量
        public int CurrentIntellect { get => currentIntellect; set => currentIntellect = value; } // 智力
        public int CurrentAgility { get => currentAgility; set => currentAgility = value; } // 敏捷
        public int CurrentStamina { get => currentStamina; set => currentStamina = value; } // 体力

        // 其它的属性
        public int CurrentHP { get => (int)(baseHP 
                + currentStrength * _attributte.strength2HP
                + currentStamina * _attributte.stamina2HP);  } // 血量
        public int CurrentDefensive { get => (int)(
                currentStrength * _attributte.strength2Defensive
                + currentStamina * _attributte.stamina2Defensive
                );  } // 防御力
        public int CurrentMagicDefensive { get => (int)(
                currentStamina * _attributte.stamina2MagicDefensive
                + currentIntellect * _attributte.intellect2MagicDefensive
                );  } // 魔法防御力
        public int CurrentAggressivity { get => (int)(currentStrength * _attributte.strength2Aggressivity);  } // 物理攻击力
        public int CurrentMagicAggressivity { get => (int)(currentIntellect * _attributte.intellect2MagicAggressivity);  } // 魔法攻击力
        public float CurrentCritical { get => currentAgility * _attributte.agility2Critical;  } // 暴击
        public float CurrentSpeed { get => currentAgility * _attributte.agility2Speed;  } // 攻速
        #endregion
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.D))
            {
                List<Item> items = InventoryManage.Instance.items;
                Debug.Log(items.Count);
                int index = Random.Range(0, items.Count);
                //int index = Random.Range(0, 2);
                Knapsack.Instance.StoryItem(items[index].Id);
            }

            
            if (Input.GetKeyUp(KeyCode.F))
            {
                Chest.Instance.OpenOrHide();
            }

        }

        public void OnClickAttribute()
        {
            Knapsack.Instance.OnClickAttribute();
        }


        public void OnClickCharacter()
        {
            Knapsack.Instance.OnClickCharacter();
        }
    }
}
