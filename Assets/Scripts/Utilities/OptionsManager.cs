using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private Slider masterVolumeSlider;

    [Header("Resolution")]
    [SerializeField] private Dropdown resolutionDropdown;

    [Header("Brightness")]
    [SerializeField] private Slider brightnessSlider;

    [Header("Buttons")]
    [SerializeField] private Button quitButton;

    [Header("Window Mode")]
    [SerializeField] private Toggle windowedToggle;

    private List<AudioSource> allAudioSources = new List<AudioSource>();
    private List<Resolution> availableResolutions;

    private void Start()
    {
        SetupVolumeSlider();
        SetupResolutionDropdown();
        SetupQuitButton();
        SetupBrightnessSlider();
        SetupWindowModeToggle();
    }

    // ====================== MASTER VOLUME ======================
    private void SetupVolumeSlider()
    {
        if (masterVolumeSlider == null) return;

        // Sempre carrega o valor salvo (ou usa 1.0 como padrão)
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        masterVolumeSlider.value = savedVolume;

        // Aplica imediatamente em TODOS os áudios da cena
        ApplyVolumeToAllAudioSources(savedVolume);

        // Listener do slider
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
    }
    private void ApplyVolumeToAllAudioSources(float volume)
    {
        // Pega TODOS os AudioSource da cena (incluindo os inativos)
        AudioSource[] allSources = FindObjectsOfType<AudioSource>(true);

        foreach (AudioSource source in allSources)
        {
            if (source != null)
                source.volume = volume;
        }

        Debug.Log($"Volume aplicado em {allSources.Length} AudioSources. Valor: {volume:F2}");
    }
    public void SetMasterVolume(float volume)
    {
       
        // Salva o valor
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();

        // Aplica em todos os áudios
        ApplyVolumeToAllAudioSources(volume);
    }

    // ====================== RESOLUÇÃO ======================
    private void SetupResolutionDropdown()
    {
        if (resolutionDropdown == null) return;

        availableResolutions = new List<Resolution>(Screen.resolutions);
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentIndex = 0;

        for (int i = 0; i < availableResolutions.Count; i++)
        {
            string option = $"{availableResolutions[i].width} x {availableResolutions[i].height}";
            options.Add(option);

            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
    }

    public void ChangeResolution(int index)
    {
        if (index < 0 || index >= availableResolutions.Count) return;

        Resolution res = availableResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        Debug.Log($"Resolução alterada para: {res.width} x {res.height}");
    }
    // ====================== BRIGHTNESS ======================
    private void SetupBrightnessSlider()
    {
        if (brightnessSlider == null) return;

        // Carrega valor salvo (padrão = 1.0)
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 1f);
        brightnessSlider.value = savedBrightness;

        // Aplica imediatamente
        SetBrightness(savedBrightness);

        // Listener do slider
        brightnessSlider.onValueChanged.AddListener(SetBrightness);
    }

    public void SetBrightness(float value)
    {
        // Salva preferência
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();

        // Aplica brilho na tela (usando Gamma / Color Adjustment)
        RenderSettings.ambientLight = new Color(value, value, value, 1f);

        // Alternativa mais moderna (funciona melhor em alguns projetos):
        // Camera.main.GetComponent<UnityEngine.Rendering.Universal.ColorAdjustments>()?.active = true;
        // (Se estiver usando URP, pode ajustar via Post-Processing)

        Debug.Log($"Brightness definido para: {value:F2}");
    }
    // ====================== WINDOW MODE (Fullscreen / Windowed) ======================
    private void SetupWindowModeToggle()
    {
        if (windowedToggle == null) return;

        // Carrega o estado salvo (padrão = Fullscreen)
        bool isWindowed = PlayerPrefs.GetInt("WindowedMode", 0) == 1;
        windowedToggle.isOn = isWindowed;

        // Aplica imediatamente
        SetWindowMode(isWindowed);

        // Listener do Toggle
        windowedToggle.onValueChanged.AddListener(SetWindowMode);
    }

    public void SetWindowMode(bool isWindowed)
    {
        // Salva a preferência
        PlayerPrefs.SetInt("WindowedMode", isWindowed ? 1 : 0);
        PlayerPrefs.Save();

        // Aplica o modo de tela
        Screen.fullScreen = !isWindowed;

        Debug.Log($"Modo de tela alterado: {(isWindowed ? "Windowed" : "Fullscreen")}");
    }
    // ====================== QUIT ======================
    private void SetupQuitButton()
    {
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");
        Application.Quit();
    }

    // Salva volume ao desativar o painel
    private void OnDisable()
    {
        if (masterVolumeSlider != null)
            PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
    }
}
