using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 一些角色动画的参数
    /// </summary>
    public class PlayerAnimatorParameter
    {
        public static readonly string forwardFloat = "Forward";
        public static readonly string rightFloat = "Right";
        public static readonly string weaponModeInt = "WeaponMode";
        public static readonly string changeWeaponModeTrigger = "ChangeWeapon";
        public static readonly string reloadTrigger = "Reload"; // 换弹夹
        public static readonly string isAimBool = "IsAiming"; // 瞄准状态
        public static readonly string isRunBool = "IsRun"; // 奔跑状态
        public static readonly string isJumpBool = "IsJump"; // 跳跃状态
        public static readonly string isGoundBool = "IsGround"; // 着地状态
        public static readonly string diedTrigger = "Die"; // 死亡

        public static readonly string swordAttackTrigger = "SwordAttack"; // 剑攻击
        public static readonly string swordAttackModeInt = "SwordAttackMode"; // 剑攻击的模式
        public static readonly string swordAttackIntervalFloat = "SwordAttackInterval"; // 剑攻击的模式



    }
}
