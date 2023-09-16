using Bags;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ʾ
/// </summary>
public class CharacterAttribute : MonoBehaviour
{
    private Text attributeText;
    private Player player;
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
        player = GameObject.Find("PlayerBag").GetComponent<Player>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showText()
    {
        // ѭ��װ��ͳ������
        //Character.Instance
        int strength = player.BaseStrength;
        int intellect = player.BaseIntellect;
        int agility = player.BaseAgility;
        int stamina = player.BaseStamina;

        player.CurrentAgility = agility;
        player.CurrentIntellect = intellect;
        player.CurrentStamina = stamina;
        player.CurrentStrength = strength;

        Character.Instance.SetAttribute(attributeText, strength, intellect, agility, stamina);
    }
}
