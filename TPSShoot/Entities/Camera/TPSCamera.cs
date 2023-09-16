using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public partial class TPSCamera : MonoBehaviour
    {
        public Transform target;

        public Transform pivot;
        public Transform cameraContainer;
        public Transform cameraTransform;

        [Header("设置")]
        public PlayerCameraSettings playerCameraSettings;

        // 当前摄像机的状态
        private CameraStatus _cameraStatus;
        private CameraPlayerStatus _cameraPlayerStatus;
        private CameraPlayerAimingStatus _cameraPlayerAimingStatus;
        private void Awake()
        {
            _cameraPlayerStatus = new CameraPlayerStatus(this, cameraContainer.localPosition);
            _cameraPlayerAimingStatus = new CameraPlayerAimingStatus(this);

            // 当前的摄像机的状态
            _cameraStatus = _cameraPlayerStatus;

            Subscribe();
        }

        private void OnDestroy()
        {
            UnSubscribe();
        }

        // Update is called once per frame
        void Update()
        {
            _cameraStatus.OnUpdate();
        }

        private void Subscribe()
        {
            Events.PlayerAimActive += OnAimingActive;
            Events.PlayerAimOut += OnAimingOut;
        }

        private void UnSubscribe()
        {
            Events.PlayerAimActive -= OnAimingActive;
            Events.PlayerAimOut -= OnAimingOut;
        }

        /// <summary>
        /// 瞄准被激活
        /// </summary>
        private void OnAimingActive()
        {
            ChangeStatus(_cameraPlayerAimingStatus);
        }
        /// <summary>
        /// 状态退出
        /// </summary>
        private void OnAimingOut()
        {
            ChangeStatus(_cameraPlayerStatus);
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        private void ChangeStatus(CameraStatus status)
        {
            status?.OnExit();
            _cameraStatus = status;
            status.OnEnter();
        }
    }
}
