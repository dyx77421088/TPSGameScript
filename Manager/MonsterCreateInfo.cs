using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TPSShoot.Manger
{
    public class MonsterCreateInfo : MonoBehaviour
    {
        [HideInInspector] public GameObject go;
        public bool isFirstBirth = true; // ��һ�ξ�ֱ�Ӳ���

        public Vector3 GetBirthPosition()
        {
            return transform.position;
        }

        public Quaternion GetBirthRotation()
        {
            return transform.rotation;
        }
    }
}

