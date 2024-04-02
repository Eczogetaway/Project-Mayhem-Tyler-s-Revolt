using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Dropdown fpsDropdown; // Dropdown for FPS limit 
    public Toggle fullscreenToggle;
    public Toggle vSyncToggle;

    private List<Resolution> resolutions = new List<Resolution>();

    void Start()
    {
        InitializeResolutions();
        InitializeQualitySettings();
        InitializeFullscreenToggle();
        InitializeVSyncToggle();
        InitializeFPSDropdown();
    }

    private void InitializeResolutions()
    {
        AddResolution(640, 480, "4:3");
        AddResolution(800, 600, "4:3");
        AddResolution(1024, 768, "4:3");
        AddResolution(1152, 864, "4:3");
        AddResolution(1280, 720, "16:9");
        AddResolution(1280, 800, "16:10");
        AddResolution(1280, 960, "4:3");
        AddResolution(1366, 768, "16:9");
        AddResolution(1400, 1050, "4:3");
        AddResolution(1440, 900, "16:10");
        AddResolution(1600, 900, "16:9");
        AddResolution(1680, 1050, "16:10");
        AddResolution(1920, 1080, "16:9");
        AddResolution(1920, 1200, "16:10");
        AddResolution(2560, 1440, "16:9");
        AddResolution(2560, 1600, "16:10");
        AddResolution(2880, 1620, "16:10");
        AddResolution(2880, 1800, "16:10");
        AddResolution(3200, 1800, "16:9");
        AddResolution(3840, 2160, "16:9");
        AddResolution(7680, 4320, "16:9");

        resolutions.Sort((a, b) => a.width != b.width ? a.width.CompareTo(b.width) : a.height.CompareTo(b.height));

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        foreach (var res in resolutions)
        {
            string option = $"{res.width} x {res.height} ({res.aspectRatio})";
            options.Add(option);
            if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = resolutions.IndexOf(res);
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    private void InitializeQualitySettings()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string> { "Low", "Medium", "High", "Ultra" });
        int savedQualityIndex = PlayerPrefs.GetInt("QualitySettingIndex", QualitySettings.GetQualityLevel());
        qualityDropdown.value = savedQualityIndex;
        qualityDropdown.RefreshShownValue();
        qualityDropdown.onValueChanged.AddListener(delegate { SetQuality(qualityDropdown.value); });
    }

    private void InitializeFullscreenToggle()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    private void InitializeVSyncToggle()
    {
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
        vSyncToggle.onValueChanged.AddListener(SetVSync);
    }

    private void InitializeFPSDropdown()
    {
        fpsDropdown.ClearOptions();
        fpsDropdown.AddOptions(new List<string> { "30", "60", "75", "90", "120", "144", "165", "Unlock" });
        fpsDropdown.onValueChanged.AddListener(SetFPSLimit);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualitySettingIndex", qualityIndex);
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreenMode =
isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void SetVSync(bool isVSync)
    {
        QualitySettings.vSyncCount = isVSync ? 1 : 0;
    }

    public void SetFPSLimit(int fpsIndex)
    {
        int fpsLimit = fpsIndex switch
        {
            0 => 30,
            1 => 60,
            2 => 75,
            3 => 90,
            4 => 120,
            5 => 144,
            6 => 165,
            _ => -1,
        };

        if (fpsLimit == -1)
        {
            Application.targetFrameRate = -1; // Unlock FPS 
        }
        else
        {
            Application.targetFrameRate = fpsLimit;
        }
    }

    private void AddResolution(int width, int height, string aspectRatio)
    {
        resolutions.Add(new Resolution { width = width, height = height, aspectRatio = aspectRatio });
    }
}

public class Resolution
{
    public int width;
    public int height;
    public string aspectRatio;
}
