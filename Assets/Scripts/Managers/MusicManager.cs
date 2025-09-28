using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public AudioSource MainAudioSource;//Main Audio Source
    public List<AudioClip> songs = new List<AudioClip>();

    bool canChangeSong = true;

    UiManager uiManager;
    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents this GameObject from being destroyed when changing scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
        uiManager = GameObject.Find("UIManager").GetComponent<UiManager>();
        
    }
    private void Start()
    {
        //Start Playing Songs
        //PlayRandomAudioClip();
        StartCoroutine(DelayForMusicStart());
        
    }
    private void Update()
    {
        
    }
    IEnumerator DelayForMusicStart()
    {
        yield return new WaitForSeconds(5); // 3-second delay
        PlayRandomAudioClip();             // Start playing music
        
    }
    void PlayRandomAudioClip()
    {
        
        if (songs.Count == 0) return;
        
        // Choose a random clip from the list
        AudioClip selectedClip = songs[Random.Range(0, songs.Count)];
        MainAudioSource.clip = selectedClip;
        uiManager.UpdateCurrentSongAlert(MainAudioSource.clip.name);
        MainAudioSource.Play();

        // Start the coroutine to play the next song when this one finishes
        StartCoroutine(WaitForClipToEnd(selectedClip.length));
    }

    IEnumerator WaitForClipToEnd(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        uiManager.ReturnAnimTest();
        PlayRandomAudioClip(); // Play a new random clip when the current one ends
    }
    IEnumerator IntroDelay()
    {
        yield return new WaitForSeconds(3);
    }

    //Adjust Audio Priority
    public void RestoreMutedAudioSources()
    {
        AudioSource[] sources = FindObjectsOfType<AudioSource>();

        foreach (var source in sources)
        {
            if (source.priority == 256)
            {
                source.priority = 128; // volta pro padrão audível
                Debug.Log($"AudioSource {source.name} restaurado para prioridade {source.priority}");
            }
        }
    }
}
