using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleAudio
{
    public class SimpleAudioService : MonoBehaviour, IDisposable
    {
        [SerializeField] private bool dontDestroy = true;
        private AudioSource backMusicSource;
        private List<AudioSource> soundSources;

        private IAudioDataProvider audioDataProvider;
        private IAudioParameters audioParameters;
        private ITransitionParameters transitionParameters;
        private Coroutine backMusicPlayIterator;
        
        public IAudioParameters Parameters => audioParameters;
        public IEnumerable<AudioSource> GetAllSoundSources() => soundSources;
        public AudioSource BackMusicSource => backMusicSource;

        public bool LogState
        {
            get => Logger.State;
            set => Logger.State = value;
        }

        private void Awake()
        {
            if (dontDestroy)
            {
                DontDestroyOnLoad(gameObject);
            }
            
            InitSources();
        }

        public void Init(
            IAudioDataProvider audioDataProvider = null,
            IAudioParameters audioParameters = null,
            ITransitionParameters transitionParameters = null,
            bool logState = true)
        {
            this.audioDataProvider = audioDataProvider ?? new ResourcesAudioDataProvider();
            this.audioParameters = audioParameters ?? new DefaultAudioParameters();
            this.transitionParameters = transitionParameters ?? new DefaultTransitionParameters();
            LogState = logState;
        }

        public void PlaySound(string audioKey)
        {
            var clip = audioDataProvider.GetClip(audioKey);

            foreach (var soundSource in soundSources)
            {
                if (soundSource.isPlaying)
                {
                    continue;
                }

                Play(soundSource, clip);
                return;
            }
            
            var source = CreateAudioSource();
            Play(source, clip);
            soundSources.Add(source);
        }

        public void PlayBackgroundMusic(string audioKey)
        {
            var clip = audioDataProvider.GetClip(audioKey);

            if (backMusicPlayIterator != null)
            {
                StopCoroutine(backMusicPlayIterator);
            }
            
            backMusicPlayIterator = StartCoroutine(PlayThroughFade(backMusicSource, clip, transitionParameters.BackgroundMusicTransitionDelay));
        }

        private IEnumerator PlayThroughFade(AudioSource source, AudioClip clip, float duration = 0.5f)
        {
            if (source.isPlaying)
            {
                while (source.volume > 0)
                {
                    source.volume -= Time.deltaTime / (duration / 2);
                    yield return null;
                }
            }

            Play(source, clip);
            
            while (source.volume <= 1)
            {
                source.volume = Mathf.Clamp(source.volume + Time.deltaTime / (duration / 2), 0, 1);
                yield return null;
            }
        }

        private void InitSources()
        {
            soundSources = new List<AudioSource>(4);
            var soundSource = CreateAudioSource();
            backMusicSource = CreateAudioSource("BackSource");
            soundSources.Add(soundSource);
        }

        private void Play(AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.Play();
            Logger.Log($"Play clip:{clip.name}; source:{source.gameObject.name}");
        }

        private AudioSource CreateAudioSource(string tag = null)
        {
            var sourceObject = new GameObject();
            sourceObject.transform.SetParent(transform);
            sourceObject.name = $"AudioSource({tag ?? "Default"})";
            var source = sourceObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            return source;
        }

        public void Dispose()
        {
            audioDataProvider.Dispose();
        }
    }
}
