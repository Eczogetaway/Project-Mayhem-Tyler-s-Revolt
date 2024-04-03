using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

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
        InitializeVSyncToggle();
        InitializeFPSDropdown();
        InitializeQualitySettings();
        InitializeFullscreenToggle();

        // Загрузить сохраненное состояние VSync и инициализировать переключатель VSync
        int savedVSyncSetting = PlayerPrefs.GetInt("VSyncSetting", QualitySettings.vSyncCount);
        vSyncToggle.isOn = savedVSyncSetting > 0;
        

        // Загрузить сохраненный индекс FPS и инициализировать выпадающий список FPS
        int savedFPSIndex = PlayerPrefs.GetInt("FPSLimitIndex", Application.targetFrameRate);
        fpsDropdown.value = savedFPSIndex;
        
    }
    void Awake()
    {
        // Загрузить сохраненный индекс FPS
        int savedFPSIndex = PlayerPrefs.GetInt("FPSLimitIndex", Application.targetFrameRate);

        // Установить ограничение FPS
        SetFPSLimit(savedFPSIndex);

        // Загрузить сохраненное состояние VSync
        int savedVSyncSetting = PlayerPrefs.GetInt("VSyncSetting", QualitySettings.vSyncCount);
        SetVSync(savedVSyncSetting > 0);
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
        // Загрузить сохраненное состояние VSync
        int savedVSyncSetting = PlayerPrefs.GetInt("VSyncSetting", QualitySettings.vSyncCount);
        vSyncToggle.isOn = savedVSyncSetting > 0;
        vSyncToggle.onValueChanged.AddListener(delegate
        {
            SetVSync(vSyncToggle.isOn);
            PlayerPrefs.Save(); // Сохранить изменения на диск
        });
    }


    private void InitializeFPSDropdown()
    {
        fpsDropdown.ClearOptions();
        fpsDropdown.AddOptions(new List<string> { "30", "60", "75", "90", "120", "144", "165", "Unlock" });

        // Загрузить сохраненный индекс FPS
        int savedFPSIndex = PlayerPrefs.GetInt("FPSLimitIndex", Application.targetFrameRate);
        fpsDropdown.value = savedFPSIndex;
        fpsDropdown.RefreshShownValue();
        fpsDropdown.onValueChanged.AddListener(delegate { SetFPSLimit(fpsDropdown.value); });
        SetFPSLimit(savedFPSIndex);
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
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void SetVSync(bool vSyncOn)
    {
        QualitySettings.vSyncCount = vSyncOn ? 1 : 0;

        // Сохранить состояние VSync
        PlayerPrefs.SetInt("VSyncSetting", vSyncOn ? 1 : 0);
        PlayerPrefs.Save(); // Сохранить изменения на диск
    }

    public void SetFPSLimit(int fpsIndex)
    {
        int fps = fpsIndex switch
        {
            0 => 30,
            1 => 60,
            2 => 75,
            3 => 90,
            4 => 120,
            5 => 144,
            6 => 165,
            7 => -1, // Разблокировать FPS
            _ => -1, // По умолчанию разблокировать, если индекс вне диапазона
        };

        if (fps == -1)
        {
            // Разблокировать FPS
            Application.targetFrameRate = -1;
        }
        else
        {
            // Установить ограничение FPS
            Application.targetFrameRate = fps;
        }

        // Сохранить индекс FPS
        PlayerPrefs.SetInt("FPSLimitIndex", fpsIndex);
        PlayerPrefs.Save(); // Сохранить изменения на диск
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
