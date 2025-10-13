using System.Collections.Generic;

using UnityEngine;
using Singleton;

[System.Serializable]
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
        public bool Initialized { get; private set; } = false;

        [Header("Global Volume Settings")]
        [Range(0f, 1f)] [SerializeField] private float bgmVolume = 0.5f;
        [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1.0f;

        [Header("Sound Resources")]
        public List<Sound> sounds;
        private Dictionary<AudioClipNames, Sound> soundDict;

        [Header("Audio Sources")]
        private AudioSource bgmAudioSource;
        private AudioSource sfxAudioSource;

        private void Awake()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            bgmAudioSource = audioSources[0];
            sfxAudioSource = audioSources[1];

            soundDict = new Dictionary<AudioClipNames, Sound>();
            foreach (Sound sound in sounds)
            {
                if (!soundDict.ContainsKey(sound.AudioClipNames))
                {
                    soundDict.Add(sound.AudioClipNames, sound);
                }
            }

            Initialized = true;
        }

        public void PlayBGM(AudioClipNames clipName)
        {
            if (clipName == AudioClipNames.None || !soundDict.ContainsKey(clipName))
                return;

            if (bgmAudioSource == null)
                return;

            if (bgmAudioSource.isPlaying)
            {
                StopBGM();
                StopSFX();
            }

            Sound sound = soundDict[clipName];
            bgmAudioSource.clip = sound.clip;
            bgmAudioSource.volume = sound.volume * bgmVolume;
            bgmAudioSource.loop = sound.loop;
            bgmAudioSource.Play();
        }

        public void StopBGM()
        {
            bgmAudioSource.Stop();
        }

        public void StopSFX()
        {
            sfxAudioSource.Stop();
        }

        public float PlaySFX(AudioClipNames clipName)
        {
            if (clipName == AudioClipNames.None || !soundDict.ContainsKey(clipName))
                return 0f;

            Sound sound = soundDict[clipName];
            sfxAudioSource.PlayOneShot(sound.clip, sound.volume * sfxVolume);
            return sound.clip.length;
        }
    }
}