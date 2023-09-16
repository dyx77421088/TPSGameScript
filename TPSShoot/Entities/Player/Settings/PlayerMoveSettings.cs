using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerMoveSettings
    {

        [Tooltip("滞空的速度")] public float airSpeed = 6f;
        [Tooltip("跳跃的速度")] public float jumpSpeed = 8f;
        [Tooltip("跳跃持续时间")] public float jumpTime = 0.25f;


        [Tooltip("向前（后）走的速度")]public float forwardSpeed = 5f;
        [Tooltip("向右（左）走的速度")]public float rightSpeed = 5f;
        [Tooltip("冲刺的速度")]public float sprintSpeed = 8f;

    }
}
