using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSoundtrack : MonoBehaviour
{
    static AudioSourceSoundtrack instance;

    void Awake()
    {
        // Check if there is already an instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Prevents this GameObject from being destroyed
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances if they exist
        }
    }
}
