using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 属性显示
/// </summary>
namespace TPSShoot.Bags
{
    public class CharacterAttribute : MonoBehaviour
    {
        private Text attributeText;
        private PlayerBehaviour.Attribute player;
        private static CharacterAttribute instance;
        public static CharacterAttribute Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GameObject.Find("Character Attribute Text").GetComponent<CharacterAttribute>();
                }
                return instance;
            }
        }

        void Start()
        {
            attributeText = GetComponent<Text>();
            player = PlayerBehaviour.Instance.playerAttribute;


        }

        public void showText()
        {
            // 循环装备统计属性
            //Character.Instance
            int strength = player.BaseStrength;
            int intellect = player.BaseIntellect;
            int agility = player.BaseAgility;
            int stamina = player.BaseStamina;

            //PlayerBagBehaviour.Instance.GetCharacter().SetAttribute(attributeText, strength, intellect, agility, stamina, player);
        }
    }
}
