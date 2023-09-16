using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute
{
    private int strength;
    private int intellect;
    private int agility;
    private int stamina;
    private int attack;
    private int magicAttack;
    private int hp;
    private float criticalStrike;
    private int defensive;

    /// <summary>
    /// ����
    /// </summary>
    public int Strength { get => strength; set => strength = value; }
    /// <summary>
    /// ����
    /// </summary>
    public int Intellect { get => intellect; set => intellect = value; }
    /// <summary>
    /// ����
    /// </summary>
    public int Agility { get => agility; set => agility = value; }
    /// <summary>
    /// ����
    /// </summary>
    public int Stamina { get => stamina; set => stamina = value; }
    /// <summary>
    /// ����
    /// </summary>
    public int Attack { get => attack; set => attack = value; }
    /// <summary>
    /// ħ������
    /// </summary>
    public int MagicAttack { get => magicAttack; set => magicAttack = value; }
    /// <summary>
    /// ����ֵ
    /// </summary>
    public int Hp { get => hp; set => hp = value; }
    /// <summary>
    /// ����
    /// </summary>
    public float CriticalStrike1 { get => criticalStrike; set => criticalStrike = value; }
    /// <summary>
    /// ������
    /// </summary>
    public int Defensive { get => defensive; set => defensive = value; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strength">����Ӱ��������(10*)�ͷ���(5*)</param>
    /// <param name="intellect">����Ӱ�취������(15*)</param>
    /// <param name="agility">����Ӱ�챩����(0.1*)</param>
    /// <param name="stamina">����Ӱ������(100*)�ͷ���(5*)</param>
    /// <param name="damage">�˺�Ӱ�취������(1*)��������(1*)</param>
    public Attribute(int strength, int intellect, int agility, int stamina, int damage, int hp=100, int defensive = 10)
    {
        this.strength = strength;
        this.intellect = intellect;
        this.agility = agility;
        this.stamina = stamina;
        this.hp = hp;
        this.defensive = defensive;

        // ����
        this.attack += strength * 10;
        this.defensive += strength * 5;

        // ����
        this.magicAttack += intellect * 15;

        // ����
        this.criticalStrike += agility * 0.1f;

        // ����
        this.hp += stamina * 100;
        this.defensive += stamina * 5;

        // �˺�
        this.attack += damage;
        this.magicAttack += damage;
    }

    public string GetAttributeString()
    {
        string text = string.Format(
                    "<color=#7CFFF0>����</color> <color=#ffffff>{0}</color>      <color=#7CFFF0>������</color> <color=#ffffff>{4}</color>\r\n" +
                    "<color=#7CFFF0>����</color> <color=#ffffff>{1}</color>      <color=#7CFFF0>��������</color> <color=#ffffff>{5}</color>\r\n" +
                    "<color=#7CFFF0>����</color> <color=#ffffff>{2}</color>      <color=#7CFFF0>������</color> <color=#ffffff>{6}%</color>\r\n" +
                    "<color=#7CFFF0>����</color> <color=#ffffff>{3}</color>      <color=#7CFFF0>��Ѫ</color> <color=#ffffff>{7}</color>\r\n"+
                    "<color=#7CFFF0>������</color> <color=#ffffff>{8}</color>"
                    , strength, intellect, agility, stamina,
                    attack, magicAttack, criticalStrike, hp, defensive
                    );

        return text;
    }
}
