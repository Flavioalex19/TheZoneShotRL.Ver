using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    Camera mainCamera;
    GameManager gameManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] Toggle hdrToggle;
    [SerializeField] Slider brightnessSlider;
    [SerializeField] Button quitBtn;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainCamera = Camera.main;
        audioSource = GameObject.Find("SoundTrack Source").GetComponent<AudioSource>();
        // Initialize slider with current volume
        volumeSlider.value = audioSource.volume;

        // Listen for changes
        volumeSlider.onValueChanged.AddListener(SetMasterVolume);
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
        hdrToggle.onValueChanged.AddListener(SetHDR);

        //Quit
        quitBtn.onClick.AddListener(() => gameManager.QuitApp());

    }

    public void SetMasterVolume(float value)
    {
        audioSource.volume = value; // value is between 0f and 1f
    }
    public void SetBrightness(float value)
    {
        /*
        if (exposureOverride != null)
        {
            exposureOverride.fixedExposure.value = value; // -2 a 3 = brilho escuro a claro
        }
        */
        RenderSettings.ambientIntensity = Mathf.LinearToGammaSpace(value + 1); // 0 a 2
        PlayerPrefs.SetFloat("Brightness", value);
    }

    public void SetHDR(bool enabled)
    {
        if (mainCamera != null)
        {
            mainCamera.allowHDR = enabled;
        }

        //PlayerPrefs.SetInt("HDR", enabled ? 1 : 0);
    }
    private void LoadSettings()
    {
        // Carrega brilho
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0f); // 0 = padrão
        brightnessSlider.value = savedBrightness;
        SetBrightness(savedBrightness);

        // Carrega HDR
        bool savedHDR = PlayerPrefs.GetInt("HDR", 1) == 1; // true por padrão (HDR ligado)
        hdrToggle.isOn = savedHDR;
        SetHDR(savedHDR);
    }
}
