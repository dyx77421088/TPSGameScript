using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using Unity.VisualScripting;
using UnityEngine;

namespace TPSShoot
{
    public partial class PlayerBehaviour
    {
        // ��ɫ��ǰ��״̬
        // �Ƿ���״̬
        public bool IsRuning { get; private set; }
        // �Ƿ��¶�״̬
        public bool IsCrouching { get; private set; } = false;
        // ��Ծ
        public bool IsJump { get; private set; }
        // ���棨����ɫ�Ϳ�ʱ�ٶ�������ͬ��
        public bool IsGround { get; private set; } = true;

        private float _forward;
        private float _right;
        private readonly float _movementSpeed = 15f;

        // ������ص�
        public float _gravity;
        private bool _resetGravity;
        /// <summary>
        /// �ƶ�
        /// </summary>
        private void UpdateWalk()
        {
            _forward = Mathf.MoveTowards(_forward, InputController.VerticalMove, _movementSpeed * Time.deltaTime);
            _right = Mathf.MoveTowards(_right, InputController.HorizontalMove, _movementSpeed * Time.deltaTime);

            _animator.SetFloat(PlayerAnimatorParameter.forwardFloat, _forward);
            _animator.SetFloat(PlayerAnimatorParameter.rightFloat, _right);
        }

        /// <summary>
        /// �л�Ϊ�ܲ�״̬
        /// </summary>
        private void UpdateRun()
        {
            
            // һЩ״̬�²����л�
            if (InputController.IsRun && !IsReload && !IsJump && !IsFire && 
                (IsGunWeapon && _forward > 0.3f || IsNoWeapon || IsSwordWeapon) && !IsWeapingWeapon
                && !IsAiming
            ) IsRuning = true;
            else IsRuning = false;

            _animator.SetBool(PlayerAnimatorParameter.isRunBool, IsRuning);
        }

        private void UpdateMovementSpeed()
        {
            // ���Ĺ���״̬�²����ƶ�
            if (IsSwordAttack) return;

            Vector3 movement = new Vector3(_right, 0, _forward);
            movement.Normalize();

            if (IsGround) // ���Ƿ�ӵ�
            {
                movement.x *= moveSettings.rightSpeed; // x���ٶȲ���
                movement.z *= IsRuning ? moveSettings.sprintSpeed : moveSettings.forwardSpeed;
            }
            else
            {
                movement *= moveSettings.airSpeed;
            }
            // TransformDirection => �ֲ�����ת��Ϊ��������
            _characterController.Move(transform.TransformDirection(movement) * Time.deltaTime);
        }
        /// <summary>
        /// ������
        /// </summary>
        private void UpdateGround()
        {
            IsGround = CheckGround();

            _animator.SetBool(PlayerAnimatorParameter.isGoundBool, IsGround);

            _animator.SetBool(PlayerAnimatorParameter.isJumpBool, IsJump);

        }
        /// <summary>
        /// �ı�����
        /// </summary>
        private void UpdateGravity()
        {
            // �����ɫ�Ѿ����ŵ�״̬�͸��̶�����
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
        /// ��ɫ��Ծ������
        /// </summary>
        private void OnJumpRequested()
        {
            // һЩ״̬�²�����Ծ
            if (PlayerBagBehaviour.Instance.IsOpenBag) return;
            if (IsSwordAttack) return;
            if (IsJump) return;
            // ������¶�״̬�ʹ��¶׵�����
            if (IsCrouching)
            {

            }
            else
            {
                Jump();
            }
        }
        /// <summary>
        /// ����Ƿ��ŵ�
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

        #region ��ɫ��Ծ��ص�
        private bool _jumpingTrigger;
        private void Jump()
        {
            if (_jumpingTrigger) return; // �������Ծ״̬
            // ��ɫ�ڵ���
            if (IsGround)
            {
                _jumpingTrigger = true;
                soundSettings.Play(soundSettings.jumpSound);
                // n����_jumpingTrigger��Ϊfalse
                StartCoroutine(SetJumpingTriggerFalse());
            }
        }

        private IEnumerator SetJumpingTriggerFalse()
        {
            yield return new WaitForSeconds(moveSettings.jumpTime);
            _jumpingTrigger = false;
        }

        // һЩevent
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
