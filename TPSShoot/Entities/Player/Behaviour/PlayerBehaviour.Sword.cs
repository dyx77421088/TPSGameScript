using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using TPSShoot.Utils;
using UnityEngine;
using static TPSShoot.MonsterBehaviour;
using UnityEngine.Timeline;

namespace TPSShoot
{
    /// <summary>
    /// 角色使用的武器的部分（剑）
    /// </summary>
    public partial class PlayerBehaviour
    {
        public PlayerSword CurrentPlayerSword { get { return (PlayerSword)CurrentWeapon; }}

        public bool IsSwordAttack {  get; private set; }
        public float QCD { get; set; }
        public float ECD { get; set; }
        public float RCD { get; set; }
        private float lastTime;

        private void InitCD()
        {
            QCD = ECD = RCD = 1;
        }
        #region 一些订阅
        /// <summary>
        /// 剑攻击的请求
        /// </summary>
        private void OnSwrodAttackRequest()
        {
            if (PlayerBagBehaviour.Instance.IsOpenBag) return;
            // 一些状态下不能进行攻击
            if (!CurrentWeapon) return;
            if (!IsSwordWeapon) return;
            if (IsSwordAttack) return;
            if (IsJump) return;

            SelectSwordAttackMode(IsRuning ? PlayerSwordAttackMode.SlidingTackleAttack : PlayerSwordAttackMode.NormalAttack);
        }
        /// <summary>
        /// 技能攻击请求
        /// </summary>
        private void OnSwordSkillAttackRequest(PlayerSwordAttackMode mode)
        {
            if (PlayerBagBehaviour.Instance.IsOpenBag) return;
            // 一些状态下不能进行攻击
            if (!CurrentWeapon) return;
            if (!IsSwordWeapon) return;
            if (IsSwordAttack) return;
            if (currentMP < 20) return;
            if (mode == PlayerSwordAttackMode.SkillAttack1 && QCD < 1) return; // 没有cd
            if (mode == PlayerSwordAttackMode.SkillAttack2 && ECD < 1) return; // 没有cd
            if (mode == PlayerSwordAttackMode.SkillAttack3 && RCD < 1) return; // 没有cd

            // 减少蓝量
            AddMP(- 20);
            SelectSwordAttackMode(mode);
            // 使用技能（修改ui）
            Events.PlayerSwordSkill.Call(mode);
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
        #region 攻击相关的
        /// <summary>
        /// n段伤害, 越后面的伤害越高
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
        
        private void OnDrawGizmos()
        {
            if (IsSwordWeapon)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(transform.position + transform.rotation * CurrentPlayerSword.attackOffset, CurrentPlayerSword.attackSphereRadius);
            }
        }
        #endregion


        private void SelectSwordAttackMode(PlayerSwordAttackMode mode)
        {
            // 大于0的为非技能攻击
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
            /// 普通攻击
            /// </summary>
            NormalAttack = 1,
            /// <summary>
            /// 滑铲
            /// </summary>
            SlidingTackleAttack = 2,
            /// <summary>
            /// 技能攻击
            /// </summary>
            SkillAttack1 = -1,
            SkillAttack2 = -2,
            SkillAttack3 = -3,
        }
    }
}
