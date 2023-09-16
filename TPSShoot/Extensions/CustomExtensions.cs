using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// ��չ����
    /// </summary>
    public static class CustomExtensions
    {
        /// <summary>
        /// �ѷ�Χ�ŵ�-180~180
        /// </summary>
        public static float Angle(this float angle)
        {
            angle %= 360;
            if (angle > 180) angle -= 360;
            return angle;
        }
    }
}
