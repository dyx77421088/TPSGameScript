using System.Collections;
using System.Collections.Generic;
using TPSShoot.Manger;
using UnityEngine;

namespace TPSShoot
{
    public class DesktopInput : MonoBehaviour
    {
        [Header("����ƶ��ٶ�")]
        public float mouseSpeed = 5f;

        [Header("һЩ���Ŀ���")]
        [Tooltip("��Ծ")]public KeyCode jumpKeyCode = KeyCode.Space;
        [Tooltip("����")] public KeyCode reloadKeyCode = KeyCode.R;
        [Tooltip("��")] public KeyCode runKeyCode = KeyCode.LeftShift;
        [Tooltip("����")] public KeyCode bagKeyCode = KeyCode.B;
        [Tooltip("D")] public KeyCode yKeyCode = KeyCode.Y;
        [Tooltip("��ͣ")] public KeyCode pauseKeyCode = KeyCode.Escape;
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
        [Header("����Ϊ����صİ���")]
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
            // ˮƽ�ʹ�ֱ��ֵ
            InputController.VerticalMove = Input.GetAxis("Vertical");
            InputController.HorizontalMove = Input.GetAxis("Horizontal");

            InputController.VerticalRotation = Input.GetAxis("Mouse Y") * mouseSpeed;
            InputController.HorizontalRotation = Input.GetAxis("Mouse X") * mouseSpeed;

            // һЩ������ֵ
            InputController.IsRun = Input.GetKey(runKeyCode);
            if (Input.GetKeyDown(jumpKeyCode))
            {
                Events.JumpRequest.Call();
            }
            // ��ǹ or ����ͨ����
            if (Input.GetMouseButton(0))
            {
                Events.SwordAttackRequest.Call();
                Events.FireRequest.Call();
            }
            // ��׼
            if (Input.GetMouseButtonDown(1))
            {
                Events.AimRequest.Call();
            }
            // ����
            if (Input.GetKeyDown(reloadKeyCode))
            {
                Events.ReloadRequest.Call();
            }
            // ����
            if (Input.GetKeyUp(bagKeyCode))
            {
                Events.BagRequest.Call();
            }
            // ��ͣ
            if (Input.GetKeyDown(pauseKeyCode))
            {
                Events.GamePauseRequest.Call();
            }
            // ���Ա����õ�
            if (Input.GetKeyUp(yKeyCode))
            {
                var t = new MonsterAttribute();
                t.grade = 50;
                t.StartInit();
                Events.PlayerKillMonster.Call(t);
            }
            // ���ļ��ܹ���
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
            // һЩ���ּ�
            for (int i = 0; i < swapWeaponKeyCodes.Length; i++)
            {
                if (Input.GetKeyDown(swapWeaponKeyCodes[i]))
                {
                    // �������±��0��ʼ
                    Events.PlayerSwapWeapon.Call(i - 1);
                }
            }

        }

        /// <summary>
        /// ��ס���
        /// </summary>
        private void LockCursor()
        {
            if (GameManager.Instance.isMobileInput) return;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        private void UnLockCursor()
        {
            // �˳�ȫ��ģʽ
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
