using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using TPSShoot.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

namespace TPSShoot
{
    /// <summary>
    /// ��ɫʹ�õ������Ĳ��֣�����
    /// </summary>
    public partial class PlayerAIBehaviour
    {
        public PlayerAISword CurrentPlayerSword { get { return (PlayerAISword)CurrentWeapon; }}

        public bool IsSwordAttack {  get; private set; }
        public float QCD { get; set; }
        public float ECD { get; set; }
        public float RCD { get; set; }
        private float lastTime;

        private void InitCD()
        {
            QCD = ECD = RCD = 1;
        }
        #region һЩ����
        /// <summary>
        /// ������������
        /// </summary>
        public void OnSwrodAttackRequest()
        {
            if (PlayerBagBehaviour.Instance.IsOpenBag) return;
            // һЩ״̬�²��ܽ��й���
            if (!IsAlive) return;
            if (!CurrentWeapon) return;
            if (!IsSwordWeapon) return;
            if (IsSwordAttack) return;
            if (IsJump) return;

            SelectSwordAttackMode(IsRuning ? PlayerSwordAttackMode.SlidingTackleAttack : PlayerSwordAttackMode.NormalAttack);
        }
        /// <summary>
        /// ���ܹ�������
        /// </summary>
        public void OnSwordSkillAttackRequest(PlayerSwordAttackMode mode)
        {
            if (PlayerBagBehaviour.Instance.IsOpenBag) return;
            // һЩ״̬�²��ܽ��й���
            if (!IsAlive) return;
            if (!CurrentWeapon) return;
            if (!IsSwordWeapon) return;
            if (IsSwordAttack) return;
            //if (currentMP < 20) return;
            //if (mode == PlayerSwordAttackMode.SkillAttack1 && QCD < 1) return; // û��cd
            //if (mode == PlayerSwordAttackMode.SkillAttack2 && ECD < 1) return; // û��cd
            //if (mode == PlayerSwordAttackMode.SkillAttack3 && RCD < 1) return; // û��cd

            // ��������
            //AddMP(- 20);
            SelectSwordAttackMode(mode);
            // ʹ�ü��ܣ��޸�ui��
            //Events.PlayerSwordSkill.Call(mode);
        }
        private void OnSwordShow()
        {
            StartCoroutine(SwordShowIE());
        }
        #endregion
        private IEnumerator SwordShowIE()
        {
            yield return new WaitForSeconds(1);
            CurrentPlayerSword.playerSwordSound.Play(CurrentPlayerSword.playerSwordSound.unsheathSound);
        }
        #region ������ص�
        /// <summary>
        /// n���˺�, Խ������˺�Խ��
        /// </summary>
        
        private void OnSwordAttack1()
        {

            CurrentPlayerSword.OnAttack(1);
        }

        private void OnSwordAttack2()
        {
            CurrentPlayerSword.OnAttack(2);
        }
        private void OnSwordAttack3()
        {
            CurrentPlayerSword.OnAttack(3);
        }
        private void OnSwordAttack4()
        {
            CurrentPlayerSword.OnAttack(4);
        }
        private void OnSwordAttackStart()
        {
            IsSwordAttack = true;
            
        }
        private void OnSwordAttackFinish()
        {
            IsSwordAttack = false;
            _animator.SetInteger(PlayerAnimatorParameter.swordAttackModeInt, 0);
            _animator.SetFloat(PlayerAnimatorParameter.swordAttackIntervalFloat, 2);
            lastTime = Time.time;
        }
        
        #endregion
        

        private void SelectSwordAttackMode(PlayerSwordAttackMode mode)
        {
            // ����0��Ϊ�Ǽ��ܹ���
            if (mode > 0)
            {
                _animator.SetFloat(PlayerAnimatorParameter.swordAttackIntervalFloat, Time.time - lastTime);
            }
            _animator.SetInteger(PlayerAnimatorParameter.swordAttackModeInt, (int)mode);
            _animator.SetTrigger(PlayerAnimatorParameter.swordAttackTrigger);
        }

        public enum PlayerSwordAttackMode
        {
            None = 0,
            /// <summary>
            /// ��ͨ����
            /// </summary>
            NormalAttack = 1,
            /// <summary>
            /// ����
            /// </summary>
            SlidingTackleAttack = 2,
            /// <summary>
            /// ���ܹ���
            /// </summary>
            SkillAttack1 = -1,
            SkillAttack2 = -2,
            SkillAttack3 = -3,
        }
    }
}
