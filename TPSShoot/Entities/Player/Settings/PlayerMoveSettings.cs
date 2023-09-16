using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerMoveSettings
    {

        [Tooltip("�Ϳյ��ٶ�")] public float airSpeed = 6f;
        [Tooltip("��Ծ���ٶ�")] public float jumpSpeed = 8f;
        [Tooltip("��Ծ����ʱ��")] public float jumpTime = 0.25f;


        [Tooltip("��ǰ�����ߵ��ٶ�")]public float forwardSpeed = 5f;
        [Tooltip("���ң����ߵ��ٶ�")]public float rightSpeed = 5f;
        [Tooltip("��̵��ٶ�")]public float sprintSpeed = 8f;

    }
}
