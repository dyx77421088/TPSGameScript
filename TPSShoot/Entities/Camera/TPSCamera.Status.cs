using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public partial class TPSCamera
    {
        private abstract class CameraStatus
        {
            protected TPSCamera tpsCamera;
            public CameraStatus(TPSCamera tpsCamera)
            {

                this.tpsCamera = tpsCamera;

            }

            public virtual void OnEnter()
            {

            }

            public virtual void OnExit()
            {

            }

            public virtual void OnUpdate()
            {

            }
        }
    }
}
