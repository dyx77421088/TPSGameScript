using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.UI
{
    public class PlayerInfoUI : CanvasElement
    {
        public Text grade;
        public override void SubScribe()
        {
            Events.GamePause += Hide;
            Events.PlayerOpenBag += Hide;
            Events.PlayerDied += Hide;

            Events.PlayerCloseBag += Show;
            Events.ApplicationLoaded += Show;
            Events.PlayerLoaded += Show;
            Events.GameResume += Show;

            Events.ApplicationLoaded += UpdateGrade;
            Events.PlayerLoaded += UpdateGrade;
            Events.PlayerGradeChange += UpdateGrade;
        }

        public override void UnSubScribe()
        {
            Events.GamePause -= Hide;
            Events.PlayerOpenBag -= Hide;
            Events.PlayerDied -= Hide;

            Events.PlayerCloseBag -= Show;
            Events.ApplicationLoaded -= Show;
            Events.PlayerLoaded -= Show;
            Events.GameResume -= Show;

            Events.ApplicationLoaded -= UpdateGrade;
            Events.PlayerLoaded -= UpdateGrade;
            Events.PlayerGradeChange -= UpdateGrade;
        }

        public void UpdateGrade()
        {
            PlayerBehaviour pb = PlayerBehaviour.Instance;
            grade.text = pb.CurrentGrade.ToString();
        }

    }
}
