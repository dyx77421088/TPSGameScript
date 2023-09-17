using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// ��׼״̬�£�����ͷ��λ��
    /// </summary>
    public partial class TPSCamera
    {
        private class CameraPlayerAimingStatus : CameraStatus
        {
            private Vector3 pivotCurrentLocalRotation;
            public CameraPlayerAimingStatus(TPSCamera tpsCamera) : base(tpsCamera)
            {
            }

            public override void OnEnter()
            {
                pivotCurrentLocalRotation = tpsCamera.pivot.localEulerAngles;
                pivotCurrentLocalRotation.x = pivotCurrentLocalRotation.x.Angle();

                tpsCamera.cameraContainer.localPosition = 
                    PlayerBehaviour.Instance.CurrentWeapon.weaponScopeSettings.scopePosition.localPosition;
            }

            public override void OnExit()
            {
            }

            public override void OnUpdate()
            {
                if (PlayerBagBehaviour.Instance.IsOpenBag) return;
                // �޸�����ͷ��һЩ�����parent��λ�úͽ�ɫһ��
                UpdateTPSCamera();
                // y����ת
                RotateTPSCamera(InputController.HorizontalRotation);
                // x����ת
                RotatePivot(InputController.VerticalRotation);
                // ��ɫ����ת
                UpdatePlayerRotate();
            }

            

            /// <summary>
            /// �޸�����ͷ��һЩ�����parent��λ�úͽ�ɫһ��
            /// </summary>
            private void UpdateTPSCamera()
            {
                tpsCamera.transform.position = tpsCamera.target.position;
            }
            /// <summary>
            /// y����ת
            /// </summary>
            /// <param name="y"></param>
            private void RotateTPSCamera(float y)
            {
                tpsCamera.transform.Rotate(0, y, 0);
            }

            /// <summary>
            /// x����ת
            /// </summary>
            /// <param name="x"></param>
            private void RotatePivot(float x)
            {
                // ��x�����ת�������û�������
                bool isCrouch = PlayerBehaviour.Instance.IsCrouching;
                // ��x�����ת�����������е�ֵ�� 1
                Vector2 clamp = new Vector2(
                    isCrouch ? tpsCamera.playerCameraSettings.crouchMinAngle : tpsCamera.playerCameraSettings.standMinAngle,
                    isCrouch ? tpsCamera.playerCameraSettings.crouchMaxAngle : tpsCamera.playerCameraSettings.standMaxAngle
                    );

                pivotCurrentLocalRotation.x -= x;
                // ��x�����ת�����������е�ֵ�� 2
                pivotCurrentLocalRotation.x = Mathf.Clamp(pivotCurrentLocalRotation.x, clamp.x, clamp.y);
                pivotCurrentLocalRotation.z = 0;

                // ƽ������Quaternion.Slerp��Quaternion.Euler��v3(������ת�Ƕ�)ת��ΪQuaternion
                tpsCamera.pivot.localRotation = Quaternion.Slerp(
                    tpsCamera.pivot.localRotation, 
                    Quaternion.Euler(pivotCurrentLocalRotation),
                    Time.deltaTime * 15);
            }
            // �޸Ľ�ɫ��ת
            private void UpdatePlayerRotate()
            {
                Vector3 t = tpsCamera.target.transform.eulerAngles;
                // ��tpsCamera.transform.eulerAngles.yҲ��һ����
                t.y = tpsCamera.pivot.transform.eulerAngles.y;
                tpsCamera.target.transform.eulerAngles = t;
        }
        }
    }
}
