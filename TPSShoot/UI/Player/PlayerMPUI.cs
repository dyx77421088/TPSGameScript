using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.UI
{
    public class PlayerMPUI : CanvasElement
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
            Events.PlayerChangeCurrentMP += UpdateMP;
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
            Events.PlayerChangeCurrentMP -= UpdateMP;
        }

        public void UpdateMP()
        {
            PlayerBehaviour pb = PlayerBehaviour.Instance;
            float currentMp = pb.CurrentMP;
            float maxmp = pb.MaxMP;
            hpImage.fillAmount = currentMp / maxmp;
            hpText.text = (int)currentMp + "/" + (int)maxmp;
        }


        private void InitHP()
        {
            UpdateMP();
        }
    }
}
