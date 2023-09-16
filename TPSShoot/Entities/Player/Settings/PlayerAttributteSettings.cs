using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerAttributteSettings
    {
        [Header("力量换成其它的属性")]
        [Tooltip("一点力量换成的血量")]public float strength2HP = 10;
        [Tooltip("一点力量换成的防御力")]public float strength2Defensive = 2;
        [Tooltip("一点力量换成的攻击力")]public float strength2Aggressivity = 5;

        [Header("智力换成其它的属性")]
        [Tooltip("一点智力换成的魔法防御力")] public float intellect2MagicDefensive = 8;
        [Tooltip("一点智力换成的魔法攻击力")] public float intellect2MagicAggressivity = 5;

        [Header("敏捷换成其它的属性")]
        [Tooltip("一点敏捷换成的暴击率")] public float agility2Critical = 0.5f;
        [Tooltip("一点敏捷换成的攻击速度")] public float agility2Speed = 0.1f;

        [Header("体力换成其它的属性")]
        [Tooltip("一点体力换成的血量")] public float stamina2HP = 25;
        [Tooltip("一点体力换成的防御力")] public float stamina2Defensive = 3;
        [Tooltip("一点体力换成的魔法防御力")] public float stamina2MagicDefensive = 3;
    }
}
