using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Manger;
using TPSShoot.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TPSShoot.UI
{
    public class PlayerMobileSwordInput : CanvasElement
    {
        public Image qImage;
        public Image eImage;
        public Image rImage;
        [Tooltip("多少秒刷新一次Ui")]public float refresh = 0.1f; // 0.1秒刷新一次Ui
        public override void SubScribe()
        {
            Events.GamePause += Hide;
            Events.PlayerOpenBag += Hide;
            Events.PlayerDied += Hide;
            Events.DesktopInputMode += Hide;

            //Events.PlayerSwapWeapon += SwapWeapon;

            Events.MobileInputMode += NeedShow;
            Events.PlayerShowSwordWeapon += NeedShow;
            Events.PlayerCloseBag += NeedShow;
            Events.GameResume += NeedShow;
        }

        public override void UnSubScribe()
        {
            Events.GamePause -= Hide;
            Events.PlayerOpenBag -= Hide;
            Events.PlayerDied -= Hide;
            Events.DesktopInputMode -= Hide;

            //Events.PlayerSwapWeapon -= SwapWeapon;

            Events.MobileInputMode -= NeedShow;
            Events.PlayerShowSwordWeapon -= NeedShow;
            Events.PlayerCloseBag -= NeedShow;
            Events.GameResume -= NeedShow;
        }
        
        /// <summary>
        /// 是否需要显示，当武器为枪的时候就不需要显示
        /// </summary>
        private void NeedShow()
        {
            if (PlayerBehaviour.Instance.IsSwordWeapon && GameManager.Instance.isMobileInput) Show();
        }
        private void SwapWeapon(int count)
        {
            Debug.Log(count);
            if (count == (int)WeaponeNumber.Sword)
            {
                Show();
            }
        }

        #region 技能cd
        //private Coroutine progress;
        //protected override void FinishShow()
        //{
        //    //if (!PlayerBehaviour.Instance.IsSwordWeapon) { Hide(); return; }
        //    Debug.Log("进来show了");
        //    if (progress != null) { StopCoroutine(progress); }
        //    progress = StartCoroutine(UpdateIE());
        //}
        //protected override void FinishHide()
        //{
        //    Debug.Log("进来StartHide了");
        //    if (progress != null)
        //    {
        //        Debug.Log("不为空");
        //        StopCoroutine(progress);
        //        progress = null;
        //    }
        //}

        //public void UpdatePro(PlayerBehaviour.PlayerSwordAttackMode mode)
        //{
        //    switch (mode)
        //    {
        //        case PlayerBehaviour.PlayerSwordAttackMode.SkillAttack1:
        //            PlayerBehaviour.Instance.QCD = qImage.fillAmount = 0;
        //            break;
        //        case PlayerBehaviour.PlayerSwordAttackMode.SkillAttack2:
        //            PlayerBehaviour.Instance.ECD = eImage.fillAmount = 0;
        //            break;
        //        case PlayerBehaviour.PlayerSwordAttackMode.SkillAttack3:
        //            PlayerBehaviour.Instance.RCD = rImage.fillAmount = 0;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //private IEnumerator UpdateIE()
        //{
        //    while (PlayerBehaviour.Instance.IsAlive)
        //    {
        //        yield return new WaitForSeconds(refresh);
        //        if (qImage.fillAmount < 1)
        //            PlayerBehaviour.Instance.QCD = qImage.fillAmount += GetCD(10);
        //        if (eImage.fillAmount < 1)
        //            PlayerBehaviour.Instance.ECD = eImage.fillAmount += GetCD(15); // 
        //        if (rImage.fillAmount < 1)
        //            PlayerBehaviour.Instance.RCD = rImage.fillAmount += GetCD(30); // 1 / (10 * 0.01) cd
        //    }
        //}
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
        #endregion

        /// <summary>
        /// 切换武器
        /// </summary>
        public void OnClickChangeWeapone()
        {
            Events.PlayerSwapWeapon.Call((int)WeaponeNumber.Gun);
            Hide();
        }
        /// <summary>
        /// 攻击
        /// </summary>
        public void OnClickAttack()
        {
            Events.SwordAttackRequest.Call();
        }
        /// <summary>
        /// 技能
        /// </summary>
        public void OnClickSkillAttack(int mode)
        {
            Events.SwordSkillAttackRequest.Call((PlayerBehaviour.PlayerSwordAttackMode)mode);
            //Events.SwordSkillAttackRequest.Call((PlayerBehaviour.PlayerSwordAttackMode)Enum.Parse(typeof(PlayerBehaviour.PlayerSwordAttackMode), mode.ToString()));
        }
    }
}

