using System.Collections.Generic;

using UnityEngine;
using Singleton;

public enum AudioClipNames : int
{
    None = 0,

    Coin,
    Jump,
    MainGameBGM,
    MainMenuBGM,
    GameSet,

    Size,
}

[System.Serializable]
public class Sound
{
    public AudioClipNames AudioClipNames;
    public AudioClip clip;
    public bool loop = false;
    public float volume = 1f;
}

namespace Manager
{
    public class SoundManager : SingletonBase<SoundManager>
    {
        [Header("Global Volume Settings")]
        [Range(0f, 1f)] [SerializeField] private float bgmVolume = 0.5f;
        [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1.0f;

        [Header("Audio Clips")]
        public List<Sound> sounds;
        private Dictionary<AudioClipNames, Sound> soundDict;

        private AudioSource bgmSource;
        private AudioSource sfxSource;

        private void Awake()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            bgmSource = audioSources[0];
            sfxSource = audioSources[1];

            soundDict = new Dictionary<AudioClipNames, Sound>();
            foreach (Sound sound in sounds)
            {
                if (!soundDict.ContainsKey(sound.AudioClipNames))
                {
                    soundDict.Add(sound.AudioClipNames, sound);
                }
            }
        }

        public void PlayBGM(AudioClipNames clipName)
        {
            StopBGM();

            if (clipName == AudioClipNames.None || !soundDict.ContainsKey(clipName))
                return;

            Sound sound = soundDict[clipName];
            bgmSource.clip = sound.clip;
            bgmSource.volume = sound.volume * bgmVolume;
            bgmSource.loop = sound.loop;
            bgmSource.Play();
        }

        public void StopBGM()
        {
            bgmSource.Stop();
        }

        public void PlaySFX(AudioClipNames clipName)
        {
            if (clipName == AudioClipNames.None || !soundDict.ContainsKey(clipName))
                return;

            Sound sound = soundDict[clipName];
            sfxSource.PlayOneShot(sound.clip, sound.volume * sfxVolume);
        }
    }
}