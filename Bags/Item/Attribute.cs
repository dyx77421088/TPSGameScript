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
    /// 力量
    /// </summary>
    public int Strength { get => strength; set => strength = value; }
    /// <summary>
    /// 智力
    /// </summary>
    public int Intellect { get => intellect; set => intellect = value; }
    /// <summary>
    /// 敏捷
    /// </summary>
    public int Agility { get => agility; set => agility = value; }
    /// <summary>
    /// 体力
    /// </summary>
    public int Stamina { get => stamina; set => stamina = value; }
    /// <summary>
    /// 攻击
    /// </summary>
    public int Attack { get => attack; set => attack = value; }
    /// <summary>
    /// 魔法攻击
    /// </summary>
    public int MagicAttack { get => magicAttack; set => magicAttack = value; }
    /// <summary>
    /// 生命值
    /// </summary>
    public int Hp { get => hp; set => hp = value; }
    /// <summary>
    /// 暴击
    /// </summary>
    public float CriticalStrike1 { get => criticalStrike; set => criticalStrike = value; }
    /// <summary>
    /// 防御力
    /// </summary>
    public int Defensive { get => defensive; set => defensive = value; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strength">力量影响物理攻击(10*)和防御(5*)</param>
    /// <param name="intellect">智力影响法术攻击(15*)</param>
    /// <param name="agility">敏捷影响暴击率(0.1*)</param>
    /// <param name="stamina">体力影响生命(100*)和防御(5*)</param>
    /// <param name="damage">伤害影响法术攻击(1*)和物理攻击(1*)</param>
    public Attribute(int strength, int intellect, int agility, int stamina, int damage, int hp=100, int defensive = 10)
    {
        this.strength = strength;
        this.intellect = intellect;
        this.agility = agility;
        this.stamina = stamina;
        this.hp = hp;
        this.defensive = defensive;

        // 力量
        this.attack += strength * 10;
        this.defensive += strength * 5;

        // 智力
        this.magicAttack += intellect * 15;

        // 敏捷
        this.criticalStrike += agility * 0.1f;

        // 体力
        this.hp += stamina * 100;
        this.defensive += stamina * 5;

        // 伤害
        this.attack += damage;
        this.magicAttack += damage;
    }

    public string GetAttributeString()
    {
        string text = string.Format(
                    "<color=#7CFFF0>力量</color> <color=#ffffff>{0}</color>      <color=#7CFFF0>物理攻击</color> <color=#ffffff>{4}</color>\r\n" +
                    "<color=#7CFFF0>智力</color> <color=#ffffff>{1}</color>      <color=#7CFFF0>法术攻击</color> <color=#ffffff>{5}</color>\r\n" +
                    "<color=#7CFFF0>敏捷</color> <color=#ffffff>{2}</color>      <color=#7CFFF0>暴击率</color> <color=#ffffff>{6}%</color>\r\n" +
                    "<color=#7CFFF0>体力</color> <color=#ffffff>{3}</color>      <color=#7CFFF0>气血</color> <color=#ffffff>{7}</color>\r\n"+
                    "<color=#7CFFF0>防御力</color> <color=#ffffff>{8}</color>"
                    , strength, intellect, agility, stamina,
                    attack, magicAttack, criticalStrike, hp, defensive
                    );

        return text;
    }
}
