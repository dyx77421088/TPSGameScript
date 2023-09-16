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

        [Header("����")]
        public PlayerCameraSettings playerCameraSettings;

        // ��ǰ�������״̬
        private CameraStatus _cameraStatus;
        private CameraPlayerStatus _cameraPlayerStatus;
        private CameraPlayerAimingStatus _cameraPlayerAimingStatus;
        private void Awake()
        {
            _cameraPlayerStatus = new CameraPlayerStatus(this, cameraContainer.localPosition);
            _cameraPlayerAimingStatus = new CameraPlayerAimingStatus(this);

            // ��ǰ���������״̬
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
        /// ��׼������
        /// </summary>
        private void OnAimingActive()
        {
            ChangeStatus(_cameraPlayerAimingStatus);
        }
        /// <summary>
        /// ״̬�˳�
        /// </summary>
        private void OnAimingOut()
        {
            ChangeStatus(_cameraPlayerStatus);
        }

        /// <summary>
        /// �ı�״̬
        /// </summary>
        private void ChangeStatus(CameraStatus status)
        {
            status?.OnExit();
            _cameraStatus = status;
            status.OnEnter();
        }
    }
}
