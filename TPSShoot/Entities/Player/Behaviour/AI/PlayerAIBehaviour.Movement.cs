using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using Unity.VisualScripting;
using UnityEngine;

namespace TPSShoot
{
    public partial class PlayerAIBehaviour
    {
        // 角色当前的状态
        // 是否奔跑状态
        public bool IsRuning { get; private set; }
        // 是否下蹲状态
        public bool IsCrouching { get; private set; } = false;
        // 跳跃
        public bool IsJump { get; private set; }
        // 地面（当角色滞空时速度有所不同）
        public bool IsGround { get; private set; } = true;

        private float _forward;
        private float _right;
        private readonly float _movementSpeed = 15f;

        // 重力相关的
        public float _gravity;
        private bool _resetGravity;
        /// <summary>
        /// 移动
        /// </summary>
        public void UpdateWalk(float forwar, float right)
        {
            if (!IsAlive) return;
            _forward = Mathf.MoveTowards(_forward, forwar, _movementSpeed * Time.deltaTime);
            _right = Mathf.MoveTowards(_right, right, _movementSpeed * Time.deltaTime);

            _animator.SetFloat(PlayerAnimatorParameter.forwardFloat, _forward);
            _animator.SetFloat(PlayerAnimatorParameter.rightFloat, _right);
        }
        /// <summary>
        /// idle 状态
        /// </summary>
        public void Idle()
        {
            if (!IsAlive) return;
            UpdateWalk(0, 0);
            UpdateRun(false);
        }
        /// <summary>
        /// 切换为跑步状态
        /// </summary>
        public void UpdateRun(bool isRuning = true)
        {
            if (!IsAlive) return;
            // 一些状态下不能切换
            //if (!IsJump && !IsFire && 
            //    (IsGunWeapon && _forward > 0.3f || IsNoWeapon || IsSwordWeapon) && !IsWeapingWeapon

            //) IsRuning = true;
            //else IsRuning = false;
            IsRuning = isRuning;
            _animator.SetBool(PlayerAnimatorParameter.isRunBool, IsRuning);
        }

        private void UpdateMovementSpeed()
        {
            // 剑的攻击状态下不能移动
            if (IsSwordAttack) return;

            Vector3 movement = new Vector3(_right, 0, _forward);
            movement.Normalize();

            if (IsGround) // 脚是否接地
            {
                movement.x *= moveSettings.rightSpeed; // x轴速度不变
                movement.z *= IsRuning ? moveSettings.sprintSpeed : moveSettings.forwardSpeed;
            }
            else
            {
                movement *= moveSettings.airSpeed;
            }
            // TransformDirection => 局部坐标转换为世界坐标
            _characterController.Move(transform.TransformDirection(movement) * Time.deltaTime);
        }
        /// <summary>
        /// 地面检测
        /// </summary>
        private void UpdateGround()
        {
            IsGround = CheckGround();

            _animator.SetBool(PlayerAnimatorParameter.isGoundBool, IsGround);

            _animator.SetBool(PlayerAnimatorParameter.isJumpBool, IsJump);

        }
        /// <summary>
        /// 改变重力
        /// </summary>
        private void UpdateGravity()
        {
            // 如果角色已经是着地状态就给固定重力
            if (IsGround)
            {
                _gravity = 50f;
                _resetGravity = false;
            }
            else
            {
                if (!_resetGravity)
                {
                    _gravity = 1.2f;
                    _resetGravity = true;
                }
                _gravity += Time.deltaTime * 9.8f;
            }


            Vector3 gravityV3 = new Vector3();
            if (_jumpingTrigger)
            {
                gravityV3.y = moveSettings.jumpSpeed;
            }
            else
            {
                gravityV3.y -= _gravity;
            }

            _characterController.Move(gravityV3 * Time.deltaTime);
        }
        
        /// <summary>
        /// 角色跳跃的请求
        /// </summary>
        public bool OnJumpRequested()
        {
            // 一些状态下不能跳跃
            if (!IsAlive) return false; 
            if (IsSwordAttack) return false;
            if (IsJump) return false;
            // 如果是下蹲状态就从下蹲到正常
            if (IsCrouching)
            {

            }
            else
            {
                Debug.Log("进行跳跃");
                Jump();
                
            }
            return true;
        }
        /// <summary>
        /// 检测是否着地
        /// </summary>
        private bool CheckGround()
        {
            RaycastHit hit;
            Vector3 start = transform.position + transform.up;
            Vector3 dir = Vector3.down;
            float radius = _characterController.radius;
            if (Physics.SphereCast(start, radius, dir, out hit, _characterController.height * 0.6f)) return true;
            return false;
        }

        #region 角色跳跃相关的
        private bool _jumpingTrigger;
        private void Jump()
        {
            if (_jumpingTrigger) return; // 如果是跳跃状态
            // 角色在地面
            if (IsGround)
            {
                _jumpingTrigger = true;
                soundSettings.Play(soundSettings.jumpSound);
                // n秒后把_jumpingTrigger设为false
                StartCoroutine(SetJumpingTriggerFalse());
            }
        }

        private IEnumerator SetJumpingTriggerFalse()
        {
            yield return new WaitForSeconds(moveSettings.jumpTime);
            _jumpingTrigger = false;
        }

        // 一些event
        private void StartJumping()
        {
            IsJump = true;
        }
        private void FinishedJumping()
        {
            IsJump = false;
        }

        private void LandSound()
        {
            soundSettings.Play(soundSettings.landSound);
        }
        #endregion
    }
}
