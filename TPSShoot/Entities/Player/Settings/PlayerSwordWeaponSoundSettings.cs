using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerSwordWeaponSoundSettings
    {
        [Tooltip("·¢ÉùÒôµÄµØ·½")]public AudioSource weaponAudio;
        [Tooltip("³öÇÊ")]public AudioClip unsheathSound;
        [Tooltip("¹¥»÷1")]public AudioClip attack1Sound;
        [Tooltip("¹¥»÷2")]public AudioClip attack2Sound;
        [Tooltip("¹¥»÷3")] public AudioClip attack3Sound;
        [Tooltip("¹¥»÷4")] public AudioClip attack4Sound;
        public void Play(AudioClip clip)
        {
            if (clip == null) { return; }
            weaponAudio.clip = clip;
            weaponAudio.Play();
        }

        public void Play(int count)
        {
            switch(count)
            {
                case 1:
                    Play(attack1Sound);
                    break;
                case 2:
                    Play(attack2Sound);
                    break;
                case 3:
                    Play(attack3Sound);
                    break;
                case 4:
                    Play(attack4Sound);
                    break;
            }
        }

    }
}
