using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource backgroundMusic;
        public AudioSource soundEffect;

        public void Start()
        {
            DontDestroyOnLoad(this);
        }

        public void PlayBGM(AudioClip clip)
        {
            backgroundMusic.clip = clip;
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            soundEffect.clip = clip;
            soundEffect.Play();
        }
    }
}