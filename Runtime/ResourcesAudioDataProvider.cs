using System.Collections.Generic;
using UnityEngine;

namespace SimpleAudio
{
    public class ResourcesAudioDataProvider : IAudioDataProvider
    {
        private readonly Dictionary<string, AudioClip> resources = new(10);
        private readonly string defaultRootAudioPath;

        public ResourcesAudioDataProvider(string rootAudioPath = null)
        {
            defaultRootAudioPath = rootAudioPath;
        }

        public AudioClip GetClip(string audioKey)
        {
            return GetClip(audioKey, defaultRootAudioPath);
        }
        
        public AudioClip GetClip(string audioKey, string rootPath)
        {
            if (resources.TryGetValue(audioKey, out var clip))
            {
                return clip;
            }

            var path = GetAudioPath(rootPath, audioKey);
            var resource = Resources.Load<AudioClip>(path);
            
            if (resource == null)
            {
                Logger.LogError($"Resource is null by path: {path}");
            }

            resources.Add(audioKey, resource);
            return resource;
        }

        private string GetAudioPath(string rootPath, string audioKey) =>
            string.IsNullOrEmpty(rootPath) ? audioKey : $"{rootPath}/{audioKey}";
        
        public void Dispose()
        {
            resources.Clear();
        }
    }
}