using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsManager : MonoBehaviour {

    public AudioMixer mainMixer;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    private List<Resolution> resolutions = new List<Resolution>();

    private void Start()
    {
        FindResolutions();
        FindQuality();

    }

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log(Screen.fullScreen);
    }

    public void ChangeResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ChangeQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    void FindResolutions()
    {
        foreach (Resolution r in Screen.resolutions)
        {
            resolutions.Add(r);
        }
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        foreach (Resolution res in resolutions)
        {
            string s = res.width + " x " + res.height;
            options.Add(s);

            if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = resolutions.IndexOf(res);
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    void FindQuality()
    {
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();
    }
}
