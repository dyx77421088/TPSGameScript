using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// �Ӽ�������������¼�
    /// </summary>
    public partial class Events
    {
        public static Event<int> PlayerSwapWeapon;// �л�����
        public static Event FireRequest; // ��ǹ������
        public static Event ReloadRequest; // �����е�����
        public static Event JumpRequest; // ��Ծ������
        public static Event AimRequest; // ��׼������
    }
}