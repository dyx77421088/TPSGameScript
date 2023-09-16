using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerAttributteSettings
    {
        [Header("������������������")]
        [Tooltip("һ���������ɵ�Ѫ��")]public float strength2HP = 10;
        [Tooltip("һ���������ɵķ�����")]public float strength2Defensive = 2;
        [Tooltip("һ���������ɵĹ�����")]public float strength2Aggressivity = 5;

        [Header("������������������")]
        [Tooltip("һ���������ɵ�ħ��������")] public float intellect2MagicDefensive = 8;
        [Tooltip("һ���������ɵ�ħ��������")] public float intellect2MagicAggressivity = 5;

        [Header("���ݻ�������������")]
        [Tooltip("һ�����ݻ��ɵı�����")] public float agility2Critical = 0.5f;
        [Tooltip("һ�����ݻ��ɵĹ����ٶ�")] public float agility2Speed = 0.1f;

        [Header("������������������")]
        [Tooltip("һ���������ɵ�Ѫ��")] public float stamina2HP = 25;
        [Tooltip("һ���������ɵķ�����")] public float stamina2Defensive = 3;
        [Tooltip("һ���������ɵ�ħ��������")] public float stamina2MagicDefensive = 3;
    }
}
