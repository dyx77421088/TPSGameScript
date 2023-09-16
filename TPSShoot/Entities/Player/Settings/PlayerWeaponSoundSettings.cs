using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerWeaponSoundSettings
    {
        [Tooltip("发声音的地方")]public AudioSource weaponAudio;
        [Tooltip("发声音的地方")]public AudioClip fireSound;
        [Tooltip("发声音的地方")]public AudioClip idleSound;
        [Tooltip("换子弹的声音")] public AudioClip reloadSound;
        [Tooltip("用尽后换子弹的声音")] public AudioClip reloadExhaustSound;
        public void Play(AudioClip clip)
        {
            if (clip == null) { return; }
            weaponAudio.clip = clip;
            weaponAudio.Play();
        }

    }
}
