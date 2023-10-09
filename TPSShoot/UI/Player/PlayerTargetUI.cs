using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.UI
{
    public class PlayerTargetUI : CanvasElement
    {
        public Image hpImage;
        public Text hpText;
        public Text gradeText;
        public Image avatar;

        private MonsterAttribute currentMa;
        private PlayerAIBehaviour currentPab;
        private bool currentTargetIsMonster;
        public override void SubScribe()
        {
            Events.GamePause += Hide;
            Events.PlayerOpenBag += Hide;
            Events.PlayerDied += Hide;

            Events.MonsterHit += UpdateHP;
            Events.PlayerAIHit += UpdateAIHitChangeHP;
            Events.PlayerAIAddHP += UpdateReturnBloodChangeHP;

            Events.TargetHide += UpdateHideHP;
        }

        public override void UnSubScribe()
        {
            Events.GamePause -= Hide;
            Events.PlayerOpenBag -= Hide;
            Events.PlayerDied -= Hide;

            Events.MonsterHit -= UpdateHP;
            Events.PlayerAIHit -= UpdateAIHitChangeHP;
            Events.PlayerAIAddHP -= UpdateReturnBloodChangeHP;

            Events.TargetHide -= UpdateHideHP;
        }

        public void UpdateHP(MonsterAttribute ma)
        {
            currentTargetIsMonster = true;
            currentMa = ma;

            float currentHp = ma.GetCurrentHP();
            float maxhp =  ma.GetMaxHP();
            hpImage.fillAmount = currentHp / maxhp;
            hpText.text = (int)currentHp + "/" + (int)maxhp;
            avatar.sprite = ma.avatar;
            gradeText.text = ma.grade.ToString();
            if (currentHp > 0) Show();
            else
            {
                currentMa = null;
                Hide();
            }
        }

        public void UpdateHideHP(MonsterAttribute ma, PlayerAIBehaviour pab)
        {
            if (ma != null && ma == currentMa) Hide();
            if (pab != null && pab == currentPab) Hide();
        }


        public void UpdateReturnBloodChangeHP(PlayerAIBehaviour pab)
        {
            if (IsShow) UpdateAIHP(pab, true);
        }
        public void UpdateAIHitChangeHP(PlayerAIBehaviour pab)
        {
            UpdateAIHP(pab, false);
        }
        public void UpdateAIHP(PlayerAIBehaviour pab, bool isReturnBlood)
        {
            // 如果是自动回血，且当前的目标是monster
            if (isReturnBlood && currentTargetIsMonster) return;

            currentTargetIsMonster = false;
            currentPab = pab;

            float currentHp = pab.CurrentHP;
            float maxhp = pab.MaxHP;
            hpImage.fillAmount = currentHp / maxhp;
            hpText.text = (int)currentHp + "/" + (int)maxhp;
            avatar.sprite = pab.Avatar;
            gradeText.text = pab.CurrentGrade.ToString();

            if (currentHp > 0) Show();
            else
            {
                currentPab = null;
                Hide();
            }
        }


    }
}
