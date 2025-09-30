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
        float fadeTime = 2f;

        mainSFXaudio = audio;
        volumeSFX = audio.volume;
        audio.volume = 0;

        matchMainAudioSource.volume = 0f;

        StartCoroutine(FadeInAudio(fadeTime));
    }
    private IEnumerator FadeInAudio(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            matchMainAudioSource.volume = Mathf.Lerp(0f, volumeSFX, elapsed / duration);

            //mainSFXaudio.volume = matchMainAudioSource.volume;

            yield return null;
        }

        matchMainAudioSource.volume = volumeSFX;
        //mainSFXaudio.volume = volumeSFX;
    }
    public void ResetVolume()
    {
        mainSFXaudio.volume = volumeSFX;
    }
}
