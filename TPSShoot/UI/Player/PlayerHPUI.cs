using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.UI
{
    public class PlayerHPUI : CanvasElement
    {
        public Image hpImage;
        public Text hpText;
        public override void SubScribe()
        {
            //Events.GamePause += Hide;
            //Events.PlayerOpenBag += Hide;
            //Events.PlayerDied += Hide;

            //Events.PlayerCloseBag += Show;
            //Events.ApplicationLoaded += Show;
            Events.ApplicationLoaded += InitHP;
            //Events.GameResume += Show;
            Events.PlayerChangeCurrentHP += UpdateHP;
        }

        public override void UnSubScribe()
        {
            //Events.GamePause -= Hide;
            //Events.PlayerOpenBag -= Hide;
            //Events.PlayerDied -= Hide;

            //Events.PlayerCloseBag -= Show;
            //Events.ApplicationLoaded -= Show;
            Events.ApplicationLoaded -= InitHP;
            //Events.GameResume -= Show;
            Events.PlayerChangeCurrentHP -= UpdateHP;
        }

        public void UpdateHP()
        {
            PlayerBehaviour pb = PlayerBehaviour.Instance;
            float currentHp = pb.CurrentHP;
            float maxhp = pb.MaxHP;
            hpImage.fillAmount = currentHp / maxhp;
            hpText.text = (int)currentHp + "/" + (int)maxhp;
        }


        private void InitHP()
        {
            UpdateHP();
        }
    }
}
