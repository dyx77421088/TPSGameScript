using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSShoot
{
    public class FootstepSound : MonoBehaviour
    {
        public AudioSource audioSource;
        /// <summary>
        /// 当动画执行到特定帧时播放脚步声
        /// </summary>
        private void PlayFootstepSound()
        {
            PlaySound(audioSource, true, 0.9f, 1.1f);
        }

        private void PlaySound(AudioSource audioS/*, AudioClip clip*/, bool randomizePitch = false, float randomPitchMin = 1, float randomPitchMax = 1)
        {
            //audioS.clip = clip;

            // 将 pitch 设置为大于1的值将提高音频的音调，使其听起来更高音。将 pitch 设置为小于1的值将降低音频的音调，使其听起来更低音
            if (randomizePitch == true)
                audioS.pitch = Random.Range(randomPitchMin, randomPitchMax);

            audioS.Play();
        }
    }
}
