using System;
using UnityEngine;

namespace SimpleAudio
{
    public class DefaultAudioParameters : IAudioParameters
    {
        private const string SoundStateKey = "AudioParameters_SoundState";
        private const string BackgroundMusicStateKey = "AudioParameters_BackgroundMusicState";

        public event Action<bool> OnChangeSoundState;
        public event Action<bool> OnChangeBackgroundMusicState;

        public bool SoundState
        {
            get => GetState(SoundStateKey);
            set
            {
                if (value == GetState(SoundStateKey))
                {
                    return;
                }

                PlayerPrefs.SetInt(SoundStateKey, value ? 1 : 0);
                OnChangeSoundState?.Invoke(value);
            }
        }

        public bool BackgroundMusicState
        {
            get => GetState(BackgroundMusicStateKey);
            set
            {
                if (value == GetState(BackgroundMusicStateKey))
                {
                    return;
                }

                PlayerPrefs.SetInt(BackgroundMusicStateKey, value ? 1 : 0);
                OnChangeBackgroundMusicState?.Invoke(value);
            }
        }

        private bool GetState(string stateKey)
        {
            return PlayerPrefs.GetInt(stateKey, 1) == 1;
        }

    }
}