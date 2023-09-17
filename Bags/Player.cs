using System.Collections;
using System.Collections.Generic;
using TPSShoot;
using UnityEngine;

namespace Bags
{
    public class Player
    {
        private PlayerAttributteSettings _attributte = new PlayerAttributteSettings();
        #region ��������
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

        public int CurrentStrength { get => currentStrength; set => currentStrength = value; } // ����
        public int CurrentIntellect { get => currentIntellect; set => currentIntellect = value; } // ����
        public int CurrentAgility { get => currentAgility; set => currentAgility = value; } // ����
        public int CurrentStamina { get => currentStamina; set => currentStamina = value; } // ����

        // ����������
        public int CurrentHP { get => (int)(baseHP 
                + currentStrength * _attributte.strength2HP
                + currentStamina * _attributte.stamina2HP);  } // Ѫ��
        public int CurrentDefensive { get => (int)(
                currentStrength * _attributte.strength2Defensive
                + currentStamina * _attributte.stamina2Defensive
                );  } // ������
        public int CurrentMagicDefensive { get => (int)(
                currentStamina * _attributte.stamina2MagicDefensive
                + currentIntellect * _attributte.intellect2MagicDefensive
                );  } // ħ��������
        public int CurrentAggressivity { get => (int)(currentStrength * _attributte.strength2Aggressivity);  } // ��������
        public int CurrentMagicAggressivity { get => (int)(currentIntellect * _attributte.intellect2MagicAggressivity);  } // ħ��������
        public float CurrentCritical { get => currentAgility * _attributte.agility2Critical;  } // ����
        public float CurrentSpeed { get => currentAgility * _attributte.agility2Speed;  } // ����
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
