using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        private CameraPlayerSwordStatus _cameraPlayerSwordStatus;
        private CameraPlayerAimingStatus _cameraPlayerAimingStatus;
        private bool isPause;
        private void Awake()
        {
            _cameraPlayerStatus = new CameraPlayerStatus(this, cameraContainer.localPosition);
            _cameraPlayerAimingStatus = new CameraPlayerAimingStatus(this);
            _cameraPlayerSwordStatus = new CameraPlayerSwordStatus(this, cameraContainer.localPosition);

            
            
            Subscribe();
        }
        private void Start()
        {
            // ��ǰ���������״̬,��ʼ��
            InitStatus();
        }
        private void OnDestroy()
        {
            UnSubscribe();
        }

        // Update is called once per frame
        void Update()
        {
            if (isPause) return;
            _cameraStatus.OnUpdate();
        }

        private void Subscribe()
        {
            Events.PlayerAimActive += OnAimingActive;
            Events.PlayerAimOut += OnAimingOut;
            Events.PlayerShowSwordWeapon += OnSwordActive;
            Events.PlayerHideSwordWeapon += OnSwordOut;

            Events.GamePause += OnPause;
            Events.GameResume += OnResume;
        }

        private void UnSubscribe()
        {
            Events.PlayerAimActive -= OnAimingActive;
            Events.PlayerAimOut -= OnAimingOut;
            Events.PlayerShowSwordWeapon -= OnSwordActive;
            Events.PlayerHideSwordWeapon -= OnSwordOut;

            Events.GamePause -= OnPause;
            Events.GameResume -= OnResume;
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
        /// ����״̬����
        /// </summary>
        private void OnSwordActive()
        {
            ChangeStatus(_cameraPlayerSwordStatus);
        }

        /// <summary>
        /// ����״̬�˳�
        /// </summary>
        private void OnSwordOut()
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

        private void OnPause()
        {
            isPause = true;
        }
        private void OnResume()
        {
            isPause = false;
        }

        private void InitStatus()
        {
            if (PlayerBehaviour.Instance.CurrentWeapon is PlayerSword)
            {
                _cameraStatus = _cameraPlayerSwordStatus;
            }
            else
            {
                _cameraStatus = _cameraPlayerStatus;
            }
            
        }
    }
}
