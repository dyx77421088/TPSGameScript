using System.Collections;
using System.Collections.Generic;
using TPSShoot.Manger;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.UI
{
    public class PlayerSkillUI : CanvasElement
    {
        public Image qImage;
        public Image eImage;
        public Image rImage;
        public float refresh = 0.1f; // 0.1秒刷新一次Ui


        public override void SubScribe()
        {
            Events.GamePause += Hide;
            Events.PlayerOpenBag += Hide;
            Events.PlayerDied += Hide;
            Events.PlayerHideSwordWeapon += Hide;
            Events.MobileInputMode += Hide;

            Events.PlayerCloseBag += NeedShow;
            Events.ApplicationLoaded += NeedShow;
            Events.GameResume += NeedShow;
            Events.PlayerShowSwordWeapon += NeedShow;
            Events.DesktopInputMode += NeedShow;

            Events.PlayerSwordSkill += UpdatePro;
        }

        public override void UnSubScribe()
        {
            Events.GamePause -= Hide;
            Events.PlayerOpenBag -= Hide;
            Events.PlayerDied -= Hide;
            Events.PlayerHideSwordWeapon -= Hide;
            Events.MobileInputMode -= Hide;

            Events.PlayerCloseBag -= NeedShow;
            Events.ApplicationLoaded -= NeedShow;
            Events.GameResume -= NeedShow;
            Events.PlayerShowSwordWeapon -= NeedShow;
            Events.DesktopInputMode -= NeedShow;

            Events.PlayerSwordSkill -= UpdatePro;
        }

        private void NeedShow()
        {
            if (!PlayerBehaviour.Instance.IsSwordWeapon) { return; }
            if (GameManager.Instance.isMobileInput) return;

            Show();
        }

        private Coroutine progress;
        protected override void StartShow()
        {
            
            if (progress != null) { StopCoroutine(progress); }
            progress = StartCoroutine(UpdateIE());
        }
        protected override void StartHide()
        {
            if (progress != null) 
            { 
                StopCoroutine(progress); 
                progress = null;
            }
        }

        public void UpdatePro(PlayerBehaviour.PlayerSwordAttackMode mode)
        {
            switch (mode)
            {
                case PlayerBehaviour.PlayerSwordAttackMode.SkillAttack1:
                    PlayerBehaviour.Instance.QCD = qImage.fillAmount = 0;
                    break;
                case PlayerBehaviour.PlayerSwordAttackMode.SkillAttack2:
                    PlayerBehaviour.Instance.ECD = eImage.fillAmount = 0;
                    break;
                case PlayerBehaviour.PlayerSwordAttackMode.SkillAttack3:
                    PlayerBehaviour.Instance.RCD = rImage.fillAmount = 0;
                    break;
                default:
                    break;
            }
        }
        private IEnumerator UpdateIE()
        {
            while (PlayerBehaviour.Instance.IsAlive)
            {
                yield return new WaitForSeconds(refresh);
                if (qImage.fillAmount < 1)
                    PlayerBehaviour.Instance.QCD = qImage.fillAmount += GetCD(10);
                if (eImage.fillAmount < 1) 
                    PlayerBehaviour.Instance.ECD = eImage.fillAmount += GetCD(15); // 
                if (rImage.fillAmount < 1) 
                    PlayerBehaviour.Instance.RCD = rImage.fillAmount += GetCD(30); // 1 / (10 * 0.01) cd
            }
        }
        /// <summary>
        /// 获得
        /// </summary>
        /// <param name="cd"></param>
        private float GetCD(float cd)
        {
            // refresh: new WaitForSeconds(refresh) , 每次等待这么多秒
            // 1 / refresh => 每秒执行的次数
            // (1 / refresh) * addCd 每次执行中需要加上的进度，1为最大， 所以最后用1除
            // 1 / ((1 / refresh) * addCd) = cd
            return 1 / (cd * (1 / refresh));
        }
    }
}
