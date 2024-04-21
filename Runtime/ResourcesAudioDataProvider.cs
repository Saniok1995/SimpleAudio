using System.Collections.Generic;
using UnityEngine;

namespace SimpleAudio
{
    public class ResourcesAudioDataProvider : IAudioDataProvider
    {
        private readonly Dictionary<string, AudioClip> resources = new(10);

        public AudioClip GetClip(string audioKey)
        {
            if (resources.ContainsKey(audioKey))
            {
                return resources[audioKey];
            }

            var path = GetAudioPath(audioKey);
            var resource = Resources.Load<AudioClip>(path);
            if (resource == null)
            {
                Logger.LogError($"resource is null - path ({path})");
            }

            resources.Add(audioKey, resource);
            return resource;
        }
        
        // TODO move it in config (list of path) with code generation file
        private string GetAudioPath(string audioKey) => $"Audio/{audioKey}";
        
        public void Dispose()
        {
            resources.Clear();
        }
    }
}