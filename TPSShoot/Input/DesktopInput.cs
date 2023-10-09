using System.Collections;
using System.Collections.Generic;
using TPSShoot.Manger;
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
        [Tooltip("背包")] public KeyCode bagKeyCode = KeyCode.B;
        [Tooltip("D")] public KeyCode yKeyCode = KeyCode.Y;
        [Tooltip("暂停")] public KeyCode pauseKeyCode = KeyCode.Escape;
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
        [Header("武器为剑相关的按键")]
        public KeyCode[] swordSkillAttack =
        {
            KeyCode.Q,
            KeyCode.E,
            KeyCode.R,
        };

        private void Awake()
        {
            Events.PlayerOpenBag += UnLockCursor;
            Events.GamePause += UnLockCursor;
            Events.GameResume += LockCursor;
            Events.PlayerCloseBag += LockCursor;
            Events.DesktopInputMode += OnShow;
            Events.MobileInputMode += OnHide;
        }

        private void OnDestroy()
        {
            Events.PlayerOpenBag -= UnLockCursor;
            Events.GamePause -= UnLockCursor;
            Events.GameResume -= LockCursor;
            Events.PlayerCloseBag -= LockCursor;
            Events.DesktopInputMode -= OnShow;
            Events.MobileInputMode -= OnHide;
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
            if (Input.GetKeyDown(jumpKeyCode))
            {
                Events.JumpRequest.Call();
            }
            // 开枪 or 剑普通攻击
            if (Input.GetMouseButton(0))
            {
                Events.SwordAttackRequest.Call();
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
            // 背包
            if (Input.GetKeyUp(bagKeyCode))
            {
                Events.BagRequest.Call();
            }
            // 暂停
            if (Input.GetKeyDown(pauseKeyCode))
            {
                Events.GamePauseRequest.Call();
            }
            // 测试背包用的
            if (Input.GetKeyUp(yKeyCode))
            {
                var t = new MonsterAttribute();
                t.grade = 50;
                t.StartInit();
                Events.PlayerKillMonster.Call(t);
            }
            // 剑的技能攻击
            if (Input.GetKeyUp(swordSkillAttack[0]))
            {
                Events.SwordSkillAttackRequest.Call(PlayerBehaviour.PlayerSwordAttackMode.SkillAttack1);
            }
            if (Input.GetKeyUp(swordSkillAttack[1]))
            {
                Events.SwordSkillAttackRequest.Call(PlayerBehaviour.PlayerSwordAttackMode.SkillAttack2);
            }
            if (Input.GetKeyUp(swordSkillAttack[2]))
            {
                Events.SwordSkillAttackRequest.Call(PlayerBehaviour.PlayerSwordAttackMode.SkillAttack3);
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

        /// <summary>
        /// 锁住鼠标
        /// </summary>
        private void LockCursor()
        {
            if (GameManager.Instance.isMobileInput) return;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void UnLockCursor()
        {
            // 退出全屏模式
            //Screen.fullScreen = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnShow()
        {
            gameObject.SetActive(true);
        }
        private void OnHide()
        {
            gameObject.SetActive(false);
        }
    }
}
