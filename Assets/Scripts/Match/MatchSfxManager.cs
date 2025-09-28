using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSfxManager : MonoBehaviour
{
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource matchMainAudioSource;
    [SerializeField] AudioSource matchSfxAudioSource;
    [SerializeField] AudioClip sfx_win;
    [SerializeField] AudioClip sfx_lose;
    [SerializeField] AudioClip sfx_startWhisle;
    [SerializeField] AudioClip sfx_Upgrade;
    [SerializeField] AudioClip sfx_Cheer;
    [SerializeField] AudioClip sfx_booing;

    AudioSource mainSFXaudio;

    float volumeSFX;
    // Start is called before the first frame update
    void Start()
    {
        mainAudioSource = GameObject.Find("SoundTrack Source").GetComponent<AudioSource>();

        MuteByPriority(mainAudioSource);
    }

    public void MuteByPriority(AudioSource audio)
    {
        mainSFXaudio = audio;
        volumeSFX = audio.volume;
        print("HERE!!!!!!!!!!!!");
        audio.volume = 0;
        
    }
    public void ResetVolume()
    {
        mainSFXaudio.volume = volumeSFX;
    }
}
