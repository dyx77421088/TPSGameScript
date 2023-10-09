using System.Collections;
using System.Collections.Generic;
using TPSShoot.Manger;
using TPSShoot.UI;
using UnityEngine;

namespace TPSShoot.UI
{
    public class PlayerMobileGunInput : CanvasElement
    {
        public override void SubScribe()
        {
            Events.GamePause += Hide;
            Events.PlayerOpenBag += Hide;
            Events.PlayerDied += Hide;
            Events.DesktopInputMode += Hide;

            //Events.PlayerSwapWeapon += SwapWeapon;

            Events.PlayerShowGunWeapon += NeedShow;
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

            //Events.PlayerSwapWeapon -= SwapWeapon;

            Events.PlayerShowGunWeapon -= NeedShow;
            Events.MobileInputMode -= NeedShow;
            Events.PlayerCloseBag -= NeedShow;
            Events.ApplicationLoaded -= NeedShow;
            Events.GameResume -= NeedShow;
        }
        
        /// <summary>
        /// �Ƿ���Ҫ��ʾ��������Ϊ����ʱ��Ͳ���Ҫ��ʾ
        /// </summary>
        private void NeedShow()
        {
            if (PlayerBehaviour.Instance.IsGunWeapon && GameManager.Instance.isMobileInput) Show();
        }
        private void SwapWeapon(int count)
        {
            if (count == (int)WeaponeNumber.Gun)
            {
                Show();
            }

        }

        /// <summary>
        /// �л�����
        /// </summary>
        public void OnClickChangeWeapone()
        {
            Events.PlayerSwapWeapon.Call((int)WeaponeNumber.Sword);
            Hide();
        }
        /// <summary>
        /// ��ǹ
        /// </summary>
        public void OnClickFire()
        {
            Events.FireRequest.Call();
        }
        /// <summary>
        /// ��׼
        /// </summary>
        public void OnClickAim()
        {
            Events.AimRequest.Call();
        }
        /// <summary>
        /// ����
        /// </summary>
        public void OnClickReload()
        {
            Events.ReloadRequest.Call();
        }
    }
}

