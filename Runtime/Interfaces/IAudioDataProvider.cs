using System;
using UnityEngine;

namespace SimpleAudio
{
    public interface IAudioDataProvider : IDisposable
    {
        AudioClip GetClip(string audioKey);
    }
}