using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public class FootstepSound : MonoBehaviour
    {
        public AudioSource audioSource;
        /// <summary>
        /// ������ִ�е��ض�֡ʱ���ŽŲ���
        /// </summary>
        private void PlayFootstepSound()
        {
            PlaySound(audioSource, true, 0.9f, 1.1f);
        }

        private void PlaySound(AudioSource audioS/*, AudioClip clip*/, bool randomizePitch = false, float randomPitchMin = 1, float randomPitchMax = 1)
        {
            //audioS.clip = clip;

            // �� pitch ����Ϊ����1��ֵ�������Ƶ��������ʹ������������������ pitch ����ΪС��1��ֵ��������Ƶ��������ʹ��������������
            if (randomizePitch == true)
                audioS.pitch = Random.Range(randomPitchMin, randomPitchMax);

            audioS.Play();
        }
    }
}
