using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    
    [Header("Referências")]
    [Tooltip("Arraste aqui o Slider que vai controlar o volume dos SFX")]
    public Slider volumeSlider;

    [Header("Configuração")]
    [Tooltip("Tag que os objetos com AudioSource de SFX devem ter")]
    public string sfxTag = "sfx";
    private const string VOLUME_KEY = "SFX_Volume";
    private float currentVolume = 1f;

    [Header("=== BRIGHTNESS ===")]
    public Slider brightnessSlider;
    public Image brightnessOverlay;
    private const string BRIGHTNESS_KEY = "Brightness";
    private float currentBrightness = 0.85f;

    [Header("=== FULLSCREEN ===")]
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public bool isFullscreen = true;
    private const string FULLSCREEN_KEY = "IsFullScreen";

    private List<Resolution> uniqueResolutions = new List<Resolution>();
    private void Awake()
    {
        // Carrega o volume salvo só uma vez
        currentVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 1f);
        ApplyVolume(currentVolume);
        //ApplyVolumeToAllSFX(currentVolume);
        if (resolutionDropdown != null && uniqueResolutions.Count == 0)
        {
            PopulateResolutions();
        }
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = isFullscreen;
        }
        currentBrightness = PlayerPrefs.GetFloat(BRIGHTNESS_KEY, 0.85f);
        ApplyBrightness(currentBrightness);
        if (PlayerPrefs.HasKey(FULLSCREEN_KEY))
        {
            isFullscreen = PlayerPrefs.GetInt(FULLSCREEN_KEY) == 1;
        }

        Screen.fullScreen = isFullscreen;

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = isFullscreen;
        }
    }
    private void Update()
    {
        if(isFullscreen)
        Screen.fullScreen = true;
        else Screen.fullScreen = false;
    }
    private void OnEnable()
    {
        // Aplica os valores salvos
        ApplyVolume(currentVolume);
        ApplyBrightness(currentBrightness);

        // === VOLUME ===
        if (volumeSlider != null)
        {
            volumeSlider.value = currentVolume;
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // === BRIGHTNESS ===
        if (brightnessSlider != null)
        {
            brightnessSlider.value = currentBrightness;
            brightnessSlider.onValueChanged.RemoveListener(SetBrightness);
            brightnessSlider.onValueChanged.AddListener(SetBrightness);
        }

        // === FULLSCREEN ===
        

        // === RESOLUTION ===
        
    }

    private void OnDisable()
    {
        if (volumeSlider != null) volumeSlider.onValueChanged.RemoveListener(SetVolume);
        if (brightnessSlider != null) brightnessSlider.onValueChanged.RemoveListener(SetBrightness);
        //if (fullscreenToggle != null) fullscreenToggle.onValueChanged.RemoveListener(SetFullscreen);
        if (resolutionDropdown != null) resolutionDropdown.onValueChanged.RemoveListener(SetResolution);
    }

    // ==================== VOLUME ====================
    public void SetVolume(float value)
    {
        currentVolume = Mathf.Clamp01(value);
        ApplyVolume(currentVolume);

        PlayerPrefs.SetFloat(VOLUME_KEY, currentVolume);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(float vol)
    {
        AudioListener.volume = vol;

        GameObject[] sfxObjects = GameObject.FindGameObjectsWithTag(sfxTag);
        foreach (GameObject obj in sfxObjects)
        {
            AudioSource src = obj.GetComponent<AudioSource>();
            if (src != null) src.volume = vol;
        }
    }

    // ==================== BRIGHTNESS ====================
    public void SetBrightness(float value)
    {
        currentBrightness = Mathf.Clamp01(value);
        ApplyBrightness(currentBrightness);

        PlayerPrefs.SetFloat(BRIGHTNESS_KEY, currentBrightness);
        PlayerPrefs.Save();
    }

    private void ApplyBrightness(float value)
    {
        if (brightnessOverlay == null) return;

        float alpha = 1f - value;
        Color c = brightnessOverlay.color;
        c.a = alpha;
        brightnessOverlay.color = c;
    }

    // ==================== FULLSCREEN ====================
    public void ToggleFullScreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
        //Debug.Log("Fullscreen = " + isFullscreen);
    }

    // ==================== RESOLUTION ====================
    private void PopulateResolutions()
    {
        uniqueResolutions.Clear();
        resolutionDropdown.ClearOptions();

        HashSet<string> seen = new HashSet<string>();
        foreach (Resolution res in Screen.resolutions)
        {
            string key = res.width + "x" + res.height;
            if (!seen.Contains(key))
            {
                seen.Add(key);
                uniqueResolutions.Add(res);
                resolutionDropdown.options.Add(new Dropdown.OptionData(key));
            }
        }
    }

    private int FindCurrentResolutionIndex()
    {
        for (int i = 0; i < uniqueResolutions.Count; i++)
        {
            if (uniqueResolutions[i].width == Screen.width && uniqueResolutions[i].height == Screen.height)
                return i;
        }
        return 0;
    }

    public void SetResolution(int index)
    {
        if (index < 0 || index >= uniqueResolutions.Count) return;

        Resolution res = uniqueResolutions[index];
        FullScreenMode mode = Screen.fullScreenMode;
        Screen.SetResolution(res.width, res.height, mode);
    }
}
