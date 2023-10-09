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
        [Tooltip("������ˢ��һ��Ui")]public float refresh = 0.1f; // 0.1��ˢ��һ��Ui
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
        /// �Ƿ���Ҫ��ʾ��������Ϊǹ��ʱ��Ͳ���Ҫ��ʾ
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

        #region ����cd
        //private Coroutine progress;
        //protected override void FinishShow()
        //{
        //    //if (!PlayerBehaviour.Instance.IsSwordWeapon) { Hide(); return; }
        //    Debug.Log("����show��");
        //    if (progress != null) { StopCoroutine(progress); }
        //    progress = StartCoroutine(UpdateIE());
        //}
        //protected override void FinishHide()
        //{
        //    Debug.Log("����StartHide��");
        //    if (progress != null)
        //    {
        //        Debug.Log("��Ϊ��");
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
        /// ���
        /// </summary>
        /// <param name="cd"></param>
        private float GetCD(float cd)
        {
            // refresh: new WaitForSeconds(refresh) , ÿ�εȴ���ô����
            // 1 / refresh => ÿ��ִ�еĴ���
            // (1 / refresh) * addCd ÿ��ִ������Ҫ���ϵĽ��ȣ�1Ϊ��� ���������1��
            // 1 / ((1 / refresh) * addCd) = cd
            return 1 / (cd * (1 / refresh));
        }
        #endregion

        /// <summary>
        /// �л�����
        /// </summary>
        public void OnClickChangeWeapone()
        {
            Events.PlayerSwapWeapon.Call((int)WeaponeNumber.Gun);
            Hide();
        }
        /// <summary>
        /// ����
        /// </summary>
        public void OnClickAttack()
        {
            Events.SwordAttackRequest.Call();
        }
        /// <summary>
        /// ����
        /// </summary>
        public void OnClickSkillAttack(int mode)
        {
            Events.SwordSkillAttackRequest.Call((PlayerBehaviour.PlayerSwordAttackMode)mode);
            //Events.SwordSkillAttackRequest.Call((PlayerBehaviour.PlayerSwordAttackMode)Enum.Parse(typeof(PlayerBehaviour.PlayerSwordAttackMode), mode.ToString()));
        }
    }
}

