using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Manger;
using TPSShoot.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.UI
{
    public class PlayerMobileBagInput : CanvasElement
    {
        public override void SubScribe()
        {
            Events.GamePause += Hide;
            Events.PlayerDied += Hide;
            Events.DesktopInputMode += Hide;


            Events.MobileInputMode += NeedShow;
            Events.PlayerShowSwordWeapon += NeedShow;
            Events.GameResume += NeedShow;
        }

        public override void UnSubScribe()
        {
            Events.GamePause -= Hide;
            Events.PlayerOpenBag -= Hide;
            Events.PlayerDied -= Hide;
            Events.DesktopInputMode -= Hide;


            Events.MobileInputMode -= NeedShow;
            Events.PlayerShowSwordWeapon -= NeedShow;
            Events.GameResume -= NeedShow;
        }

        private void NeedShow()
        {
            if (GameManager.Instance.isMobileInput) Show();
        }
        /// <summary>
        /// ±³°üµÄÇëÇó
        /// </summary>
        public void OnBagRequest()
        {
            Events.BagRequest.Call();
        }
    }
}

