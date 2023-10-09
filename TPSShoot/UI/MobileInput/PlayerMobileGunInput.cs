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
        /// 是否需要显示，当武器为剑的时候就不需要显示
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
        /// 切换武器
        /// </summary>
        public void OnClickChangeWeapone()
        {
            Events.PlayerSwapWeapon.Call((int)WeaponeNumber.Sword);
            Hide();
        }
        /// <summary>
        /// 开枪
        /// </summary>
        public void OnClickFire()
        {
            Events.FireRequest.Call();
        }
        /// <summary>
        /// 瞄准
        /// </summary>
        public void OnClickAim()
        {
            Events.AimRequest.Call();
        }
        /// <summary>
        /// 换弹
        /// </summary>
        public void OnClickReload()
        {
            Events.ReloadRequest.Call();
        }
    }
}

