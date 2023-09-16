using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerWeaponSoundSettings
    {
        [Tooltip("�������ĵط�")]public AudioSource weaponAudio;
        [Tooltip("�������ĵط�")]public AudioClip fireSound;
        [Tooltip("�������ĵط�")]public AudioClip idleSound;
        [Tooltip("���ӵ�������")] public AudioClip reloadSound;
        [Tooltip("�þ����ӵ�������")] public AudioClip reloadExhaustSound;
        public void Play(AudioClip clip)
        {
            if (clip == null) { return; }
            weaponAudio.clip = clip;
            weaponAudio.Play();
        }

    }
}
