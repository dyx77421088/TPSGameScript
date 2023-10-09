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
            Events.PlayerHideGunWeapon += Hide;

            Events.PlayerCloseBag += Show;
            Events.PlayerShowGunWeapon += Show;
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
            Events.PlayerHideGunWeapon -= Hide;

            Events.PlayerCloseBag -= Show;
            Events.PlayerShowGunWeapon -= Show;
            Events.ApplicationLoaded -= Show;
            Events.GameResume -= Show;

            Events.PlayerFire -= UpdateBullet;
            Events.PlayerReloaded -= UpdateBullet;
        }
        protected override void StartShow()
        {
            if (!PlayerBehaviour.Instance.IsGunWeapon )
            {
                Hide();

                return;
            }
            UpdateBullet();
        }
        public void UpdateBullet()
        {
            PlayerGun playerGun = (PlayerGun)PlayerBehaviour.Instance.CurrentWeapon;
            buttleText.text = string.Format("{0}/{1}", playerGun.currentBullet, playerGun.bulletsAmount);
            
        }

    }
}
