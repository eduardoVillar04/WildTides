using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuSettings : MonoBehaviour
{

    public AudioMixer audioMixer;

    public TMPro.TMP_Dropdown m_ResolutionDropdown;
    public TMPro.TMP_Dropdown m_GraphicDropdown;

    public GameObject m_SensitivitySlider;
    public GameObject m_VolumeSlider;
    public GameObject m_FullScreenToggle;

    Resolution[] resolutions;

    void Start()
    {
        if (SingletonOptions.m_Instance.m_InitialResolutionIndex == 0)
        {
            resolutions = Screen.resolutions;
            SingletonOptions.m_Instance.m_InitialResolutionIndex = resolutions.Length;
            SingletonOptions.m_Instance.m_ResolutionIndex = resolutions.Length;
        }

        resolutions = Screen.resolutions;
        m_ResolutionDropdown.ClearOptions();
        m_FullScreenToggle.GetComponent<Toggle>().isOn = SingletonOptions.m_Instance.m_IsFullScreen;
        m_GraphicDropdown.value = SingletonOptions.m_Instance.m_QualityIndex;
        m_SensitivitySlider.GetComponent<Slider>().value = SingletonOptions.m_Instance.m_SensitivityValue;
        audioMixer.GetFloat("volume", out float Volume);
        m_VolumeSlider.GetComponent<Slider>().value = Mathf.Pow(10,Volume/20);

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + "   " + ((uint)(resolutions[i].refreshRateRatio.value + 0.3f)) + " Hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        m_ResolutionDropdown.AddOptions(options);
        m_ResolutionDropdown.value = SingletonOptions.m_Instance.m_ResolutionIndex;
        m_ResolutionDropdown.RefreshShownValue();

    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        SingletonOptions.m_Instance.m_ResolutionIndex = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
    }

    public void SetSensitivity(float sensitivity)
    {
        SingletonOptions.m_Instance.m_SensitivityValue = sensitivity;
    }

    public void SetQuality (int qualityIndex)
    {
        SingletonOptions.m_Instance.m_QualityIndex = qualityIndex;
        switch(qualityIndex)
        {
            case 0:
                QualitySettings.SetQualityLevel(1); break;
            case 1:
                QualitySettings.SetQualityLevel(2); break;
            case 2:
                QualitySettings.SetQualityLevel(3); break;
            case 3:
                QualitySettings.SetQualityLevel(5); break;
        }
    }

    public void SetFullScreen ( bool isFullScreen)
    {
        SingletonOptions.m_Instance.m_IsFullScreen = isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}


