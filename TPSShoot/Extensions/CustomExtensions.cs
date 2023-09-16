using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class CustomExtensions
    {
        /// <summary>
        /// 把范围放到-180~180
        /// </summary>
        public static float Angle(this float angle)
        {
            angle %= 360;
            if (angle > 180) angle -= 360;
            return angle;
        }
    }
}
