using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    //[SerializeField] GameObject soundParent;
    [SerializeField] List<AudioSource> sounds;
    public bool audioMuted;
    [SerializeField] List<AudioSource> music;
    public bool musicMuted;

    public void Awake()
    {
        if(PlayerPrefs.GetInt("MuteAudio") == 1)
        {
            audioMuted = true;
            ToggleSounds();
        }

        if(PlayerPrefs.GetInt("MuteMusic") == 1)
        {
            musicMuted = true;
            ToggleMusic();
        }
    }

    public void ToggleSounds()
    {
        foreach(AudioSource sound in sounds)
        {
            sound.mute = !sound.mute;
        }
    }

    public void ToggleMusic()
    {
        foreach(AudioSource song in music)
        {
            song.mute = !song.mute;
        }
    }
}
