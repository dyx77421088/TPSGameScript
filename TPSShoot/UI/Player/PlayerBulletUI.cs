using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.UI
{
    public class PlayerBulletUI : CanvasElement
    {
        public Text buttleText;
        public override void SubScribe()
        {
            Events.GamePause += Hide;
            Events.PlayerOpenBag += Hide;
            Events.PlayerDied += Hide;
            Events.PlayerHideWeapon += Hide;

            Events.PlayerCloseBag += Show;
            Events.PlayerShowWeapon += Show;
            Events.ApplicationLoaded += Show;
            Events.GameResume += Show;

            Events.PlayerFire += UpdateBullet;
            Events.PlayerReloaded += UpdateBullet;
        }

        public override void UnSubScribe()
        {
            Events.GamePause -= Hide;
            Events.PlayerOpenBag -= Hide;
            Events.PlayerDied -= Hide;
            Events.PlayerHideWeapon -= Hide;

            Events.PlayerCloseBag -= Show;
            Events.PlayerShowWeapon -= Show;
            Events.ApplicationLoaded -= Show;
            Events.GameResume -= Show;

            Events.PlayerFire -= UpdateBullet;
            Events.PlayerReloaded -= UpdateBullet;
        }
        protected override void StartShow()
        {
            UpdateBullet();
        }
        public void UpdateBullet()
        {
            PlayerWeapon pw = PlayerBehaviour.Instance.CurrentWeapon;
            buttleText.text = string.Format("{0}/{1}", pw.currentBullet, pw.bulletsAmount);
        }

    }
}
