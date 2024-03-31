using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour
{
    public Slider resolutionSlider;
    public Slider qualitySlider;
    public Toggle fullscreenToggle;

    List<Resolution> resolutions = new List<Resolution>();
    Coroutine resolutionChangeCoroutine;

    void Start()
    {
        // Setup resolution settings
        resolutions.Add(new Resolution { width = 720, height = 480 });
        resolutions.Add(new Resolution { width = 960, height = 540 });
        resolutions.Add(new Resolution { width = 1280, height = 720 });
        resolutions.Add(new Resolution { width = 1366, height = 768 });
        resolutions.Add(new Resolution { width = 1600, height = 900 });
        resolutions.Add(new Resolution { width = 1920, height = 1080 });
        resolutions.Add(new Resolution { width = 2560, height = 1440 });
        resolutions.Add(new Resolution { width = 3200, height = 1800 });
        resolutions.Add(new Resolution { width = 3840, height = 2160 });

        resolutionSlider.minValue = 0;
        resolutionSlider.maxValue = resolutions.Count - 1;
        resolutionSlider.wholeNumbers = true;
        resolutionSlider.onValueChanged.AddListener(StartResolutionChange);

        // Setup quality settings
        qualitySlider.minValue = 0;
        qualitySlider.maxValue = QualitySettings.names.Length - 1;
        qualitySlider.wholeNumbers = true;
        qualitySlider.onValueChanged.AddListener(SetQuality);

        // Setup fullscreen settings
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    void StartResolutionChange(float resolutionIndex)
    {
        if (resolutionChangeCoroutine != null)
        {
            StopCoroutine(resolutionChangeCoroutine);
        }
        resolutionChangeCoroutine = StartCoroutine(ChangeResolution((int)resolutionIndex));
    }

    IEnumerator ChangeResolution(int resolutionIndex)
    {
        Resolution targetResolution = resolutions[resolutionIndex];
        float changeTime = 1f; // Change this to control the speed of the resolution change
        float startTime = Time.time;

        Resolution startResolution = new Resolution { width = Screen.width, height = Screen.height };

        while (Time.time - startTime < changeTime)
        {
            float t = (Time.time - startTime) / changeTime;
            int width = (int)Mathf.Lerp(startResolution.width, targetResolution.width, t);
            int height = (int)Mathf.Lerp(startResolution.height, targetResolution.height, t);
            Screen.SetResolution(width, height, Screen.fullScreen);
            yield return null;
        }

        Screen.SetResolution(targetResolution.width, targetResolution.height, Screen.fullScreen);
    }

    void SetQuality(float qualityIndex)
    {
        QualitySettings.SetQualityLevel((int)qualityIndex);
    }

    void SetFullscreen(bool isFullscreen)
    {
        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
}
