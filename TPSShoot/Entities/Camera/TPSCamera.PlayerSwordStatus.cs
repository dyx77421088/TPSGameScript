using System;
using System.Collections;
using System.Collections.Generic;
using TPSShoot.Bags;
using UnityEngine;

namespace TPSShoot
{
    /// <summary>
    /// 拿剑状态下，摄像头的位置
    /// </summary>
    public partial class TPSCamera
    {
        private class CameraPlayerSwordStatus : CameraPlayerStatus
        {
            private Vector3 pivotCurrentLocalRotation;
            public CameraPlayerSwordStatus(TPSCamera tpsCamera, Vector3 defaultLocalPos) : base(tpsCamera, defaultLocalPos)
            {
            }

            //public override void OnEnter()
            //{
            //    //tpsCamera.cameraContainer.localPosition = defaultLocalPos;
            //    //tpsCamera.transform.eulerAngles = new Vector3(0, tpsCamera.transform.eulerAngles.y, 0);
            //    //pivotCurrentLocalRotation = tpsCamera.pivot.localEulerAngles;
            //    //pivotCurrentLocalRotation.x = pivotCurrentLocalRotation.x.Angle();

            //}

            //public override void OnExit()
            //{
            //}

            //public override void OnUpdate()
            //{
            //    if (PlayerBagBehaviour.Instance.IsOpenBag) return;
            //    // 修改摄像头和一些坐标的parent的位置和角色一致
            //    UpdateTPSCamera();
            //    // y轴旋转
            //    RotateTPSCamera(InputController.HorizontalRotation);
            //    // x轴旋转
            //    RotatePivot(InputController.VerticalRotation);
            //}

            

            ///// <summary>
            ///// 修改摄像头和一些坐标的parent的位置和角色一致
            ///// </summary>
            //private void UpdateTPSCamera()
            //{
            //    tpsCamera.transform.position = tpsCamera.target.position;
            //}
            ///// <summary>
            ///// y轴旋转
            ///// </summary>
            ///// <param name="y"></param>
            //private void RotateTPSCamera(float y)
            //{
            //    tpsCamera.transform.Rotate(0, y, 0);
            //}

            ///// <summary>
            ///// x轴旋转
            ///// </summary>
            ///// <param name="x"></param>
            //private void RotatePivot(float x)
            //{
            //    // 把x轴的旋转限制在用户所输入
            //    bool isCrouch = PlayerBehaviour.Instance.IsCrouching;
            //    // 把x轴的旋转限制在设置中的值内 1
            //    Vector2 clamp = new Vector2(
            //        isCrouch ? tpsCamera.playerCameraSettings.crouchMinAngle : tpsCamera.playerCameraSettings.standMinAngle,
            //        isCrouch ? tpsCamera.playerCameraSettings.crouchMaxAngle : tpsCamera.playerCameraSettings.standMaxAngle
            //        );

            //    pivotCurrentLocalRotation.x -= x;
            //    // 把x轴的旋转限制在设置中的值内 2
            //    pivotCurrentLocalRotation.x = Mathf.Clamp(pivotCurrentLocalRotation.x, clamp.x, clamp.y);
            //    pivotCurrentLocalRotation.z = 0;

            //    // 平滑过度Quaternion.Slerp，Quaternion.Euler：v3(三个旋转角度)转换为Quaternion
            //    tpsCamera.pivot.localRotation = Quaternion.Slerp(
            //        tpsCamera.pivot.localRotation, 
            //        Quaternion.Euler(pivotCurrentLocalRotation),
            //        Time.deltaTime * 15);
            //}
        }
    }
}
