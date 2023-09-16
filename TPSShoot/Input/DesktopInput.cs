using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public class DesktopInput : MonoBehaviour
    {
        [Header("鼠标移动速度")]
        public float mouseSpeed = 5f;

        [Header("一些键的控制")]
        [Tooltip("跳跃")]public KeyCode jumpKeyCode = KeyCode.Space;
        [Tooltip("换弹")] public KeyCode reloadKeyCode = KeyCode.R;
        [Tooltip("跑")] public KeyCode runKeyCode = KeyCode.LeftShift;
        public KeyCode[] swapWeaponKeyCodes = 
        {
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
        };

        private void Awake()
        {
            
        }

        // Update is called once per frame
        private void Update()
        {
            // 水平和垂直的值
            InputController.VerticalMove = Input.GetAxis("Vertical");
            InputController.HorizontalMove = Input.GetAxis("Horizontal");

            InputController.VerticalRotation = Input.GetAxis("Mouse Y") * mouseSpeed;
            InputController.HorizontalRotation = Input.GetAxis("Mouse X") * mouseSpeed;

            // 一些按键的值
            InputController.IsRun = Input.GetKey(runKeyCode);
            if (Input.GetKey(jumpKeyCode))
            {
                Events.JumpRequest.Call();
            }
            // 开枪
            if (Input.GetMouseButton(0))
            {
                Events.FireRequest.Call();
            }
            // 瞄准
            if (Input.GetMouseButtonDown(1))
            {
                Events.AimRequest.Call();
            }
            // 换弹
            if (Input.GetKeyDown(reloadKeyCode))
            {
                Events.ReloadRequest.Call();
            }
            // 一些数字键
            for (int i = 0; i < swapWeaponKeyCodes.Length; i++)
            {
                if (Input.GetKeyDown(swapWeaponKeyCodes[i]))
                {
                    // 武器的下标从0开始
                    Events.PlayerSwapWeapon.Call(i - 1);
                }
            }

        }
    }
}
