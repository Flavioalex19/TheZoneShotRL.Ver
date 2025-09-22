using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        // Initialize slider with current volume
        volumeSlider.value = audioSource.volume;

        // Listen for changes
        volumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SetMasterVolume(float value)
    {
        audioSource.volume = value; // value is between 0f and 1f
    }
}
