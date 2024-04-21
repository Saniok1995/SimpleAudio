using System;

namespace SimpleAudio
{
    public interface IAudioParameters
    {
        bool SoundState { get; set; }
        bool BackgroundMusicState { get; set; }
        event Action<bool> OnChangeSoundState;
        event Action<bool> OnChangeBackgroundMusicState;
    }
}