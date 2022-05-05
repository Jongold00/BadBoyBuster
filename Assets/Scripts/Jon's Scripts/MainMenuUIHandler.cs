using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUIHandler : MonoBehaviour
{
    static MainMenuUIHandler instance;

    [SerializeField]
    GameObject[] mainMenuLayers;

    [SerializeField]
    TMP_Dropdown resolutionChooser;
    [SerializeField]
    Toggle fullscreen;

    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    Slider sfxSlider;





    AudioHandler audioHandler;

    private bool paused = false;
    // Start is called before the first frame update

    public static MainMenuUIHandler GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType<MainMenuUIHandler>();
        }
        return instance;
    }

    public void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }


    public void Start()
    {
        if (!audioHandler)
        {
            audioHandler = AudioHandler.GetInstance();
        }
        musicSlider.onValueChanged.AddListener(delegate { OnMusicSliderChange(musicSlider.value); });
        sfxSlider.onValueChanged.AddListener(delegate { OnSFXVolumeValue(sfxSlider.value); });

    }

    public void OnLayerToggle(int layerIndex)
    {
        for (int i = 0; i < mainMenuLayers.Length; i++)
        {
            if (i == layerIndex)
            {
                mainMenuLayers[i].SetActive(true);
            }
            else
            {
                mainMenuLayers[i].SetActive(false);
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SaveAndApplyVideoSettings()
    {
        bool turnOnFullscreen = fullscreen.isOn;
        switch (resolutionChooser.value)
        {
            case 0:
                Screen.SetResolution(1366, 768, turnOnFullscreen);
                break;
            case 1:
                Screen.SetResolution(1600, 900, turnOnFullscreen);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, turnOnFullscreen);
                break;
            case 3:
                Screen.SetResolution(1920, 1200, turnOnFullscreen);
                break;
            case 4:
                Screen.SetResolution(2560, 1440, turnOnFullscreen);
                break;
            case 5:
                Screen.SetResolution(2560, 1600, turnOnFullscreen);
                break;
            case 6:
                Screen.SetResolution(3840, 2160, turnOnFullscreen);
                break;
        }
    }

    /// <summary>
    /// STUFF TO DO WITH AUDIO TAB ON PAUSE MENU
    /// </summary>
    public void OnMusicSliderChange(float value)
    {
        audioHandler.setMusicVolume(value);
    }

    public void OnSFXVolumeValue(float value)
    {

        audioHandler.setSFXVolume(value);
    }
}
