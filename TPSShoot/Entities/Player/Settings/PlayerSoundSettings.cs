using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    [Serializable]
    public class PlayerSoundSettings
    {

        [Tooltip("播放声音的位置")] public AudioSource playerAudio;
        [Tooltip("跳跃的声音")] public AudioClip jumpSound;
        [Tooltip("着地的声音")] public AudioClip landSound;
        [Tooltip("打开瞄准的声音")] public AudioClip aimActiveSound;
        [Tooltip("关闭瞄准的声音")] public AudioClip aimOutSound;
        [Tooltip("受伤的声音")] public AudioClip hitSound;
        

        public void Play(AudioClip clip)
        {
            if (clip == null) { return; }
            playerAudio.clip = clip;
            playerAudio.Play();
        }

    }
}
