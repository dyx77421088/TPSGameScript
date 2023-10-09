using System.Collections;
using System.Collections.Generic;
using TPSShoot.Manger;
using TPSShoot.UI;
using UnityEngine;

namespace TPSShoot.UI
{
    public class PlayerMobileInput : CanvasElement
    {
        [Header("����ҡ��")]
        public VirtualJoystick joystick;
        [Header("���������ƶ�����ͷ�ģ�")]
        public Touchpad touchpad;
        public override void SubScribe()
        {
            Events.GamePause += Hide;
            Events.PlayerOpenBag += Hide;
            Events.PlayerDied += Hide;
            Events.DesktopInputMode += Hide;

            Events.MobileInputMode += NeedShow;
            Events.PlayerCloseBag += NeedShow;
            Events.ApplicationLoaded += NeedShow;
            Events.GameResume += NeedShow;
        }

        public override void UnSubScribe()
        {
            Events.GamePause -= Hide;
            Events.PlayerOpenBag -= Hide;
            Events.PlayerDied -= Hide;
            Events.DesktopInputMode -= Hide;

            Events.MobileInputMode -= NeedShow;
            Events.PlayerCloseBag -= NeedShow;
            Events.ApplicationLoaded -= NeedShow;
            Events.GameResume -= NeedShow;
        }
        private void NeedShow()
        {
            if (GameManager.Instance.isMobileInput) Show();
        }
        protected virtual void Update()
        {
            InputController.HorizontalMove = joystick.HorizontalValue;
            InputController.VerticalMove = joystick.VerticalValue;

            InputController.HorizontalRotation = touchpad.HorizontalValue;
            InputController.VerticalRotation = touchpad.VerticalValue;
        }

        /// <summary>
        /// ��Ծ����
        /// </summary>
        public void OnClickJump()
        {
            Events.JumpRequest.Call();
        }
        /// <summary>
        /// ��
        /// </summary>
        public void OnClickRuning()
        {
            InputController.IsRun = !InputController.IsRun;
        }
        
    }
}

