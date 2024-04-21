using UnityEngine;

namespace SimpleAudio
{
    internal static class Logger
    {
        public static bool State;
        
        public static void Log(string log)
        {
            if (!State)
            {
                return;
            }
            
            Debug.Log($"[SimpleAudioService] => {log}");
        }
        
        public static void LogError(string log)
        {
            Debug.LogError($"[SimpleAudioService] => {log}");
        }
    }
}