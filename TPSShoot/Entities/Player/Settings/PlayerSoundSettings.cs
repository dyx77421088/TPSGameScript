using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerSoundSettings
    {

        [Tooltip("����������λ��")] public AudioSource playerAudio;
        [Tooltip("��Ծ������")] public AudioClip jumpSound;
        [Tooltip("�ŵص�����")] public AudioClip landSound;
        [Tooltip("����׼������")] public AudioClip aimActiveSound;
        [Tooltip("�ر���׼������")] public AudioClip aimOutSound;
        [Tooltip("���˵�����")] public AudioClip hitSound;
        

        public void Play(AudioClip clip)
        {
            if (clip == null) { return; }
            playerAudio.clip = clip;
            playerAudio.Play();
        }

    }
}
