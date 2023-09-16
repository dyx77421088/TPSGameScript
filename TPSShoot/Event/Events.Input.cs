using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 从键盘输入出发的事件
    /// </summary>
    public partial class Events
    {
        public static Event<int> PlayerSwapWeapon;// 切换武器
        public static Event FireRequest; // 开枪的请求
        public static Event ReloadRequest; // 换弹夹的请求
        public static Event JumpRequest; // 跳跃的请求
        public static Event AimRequest; // 瞄准的请求
    }
}
