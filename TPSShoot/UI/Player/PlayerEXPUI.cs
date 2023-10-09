using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.UI
{
    public class PlayerEXPUI : CanvasElement
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
            Events.ApplicationLoaded += UpdateEXP;
            Events.PlayerLoaded += UpdateEXP;
            //Events.GameResume += Show;
            Events.PlayerChangeEXP += UpdateEXP;
        }

        public override void UnSubScribe()
        {
            //Events.GamePause -= Hide;
            //Events.PlayerOpenBag -= Hide;
            //Events.PlayerDied -= Hide;

            //Events.PlayerCloseBag -= Show;
            //Events.ApplicationLoaded -= Show;
            Events.ApplicationLoaded -= UpdateEXP;
            Events.PlayerLoaded -= UpdateEXP;
            //Events.GameResume -= Show;
            Events.PlayerChangeEXP -= UpdateEXP;
        }

        public void UpdateEXP()
        {
            PlayerBehaviour pb = PlayerBehaviour.Instance;
            float currentEXP = pb.CurrentEXP;
            float maxEXP = pb.UpgradeEXP;
            hpImage.fillAmount = currentEXP / maxEXP;
            hpText.text = (int)currentEXP + "/" + (int)maxEXP;
        }

    }
}
