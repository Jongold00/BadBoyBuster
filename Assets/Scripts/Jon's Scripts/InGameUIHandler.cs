using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class InGameUIHandler : MonoBehaviour
{
    static InGameUIHandler instance;

    [SerializeField]
    GameObject[] pauseMenuLayers;
    [SerializeField]
    GameObject pauseTab;

    [SerializeField]
    TMP_Dropdown resolutionChooser;
    [SerializeField]
    Toggle fullscreen;

    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    Slider sfxSlider;

    [SerializeField]
    TextMeshProUGUI gameOverScoreText;


    AudioHandler audioHandler; 

    private bool paused = false;
    private bool gameOver = false;
    // Start is called before the first frame update

    public static InGameUIHandler GetInstance()
    {
        if (!instance)
        {
            instance = GameObject.FindObjectOfType<InGameUIHandler>();
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
    /// <summary>
    /// STUFF TO DO WITH PAUSE MENU NAVIGATION
    /// toggles layers on the pause menu and changes the timescale


    public void OnGamePause()
    {
        if (gameOver)
        {
            return;
        }
        if (paused)
        {
            Time.timeScale = 1f;
            ResetPauseMenu();
        }

        else
        {
            Time.timeScale = 0f;
            pauseTab.SetActive(true);

        }

        paused = !paused;
    }

    public void OnLayerToggle(int layerIndex)
    {
        if (gameOver)
        {
            return;
        }

        for (int i = 0; i < pauseMenuLayers.Length; i++)
        {
            if (i == layerIndex)
            {
                pauseMenuLayers[i].SetActive(true);
            }
            else
            {
                pauseMenuLayers[i].SetActive(false);
            }
        }
    }
    public void ResetPauseMenu()
    {
        if (gameOver)
        {
            return;
        }

        OnLayerToggle(0);
        pauseTab.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// STUFF TO DO WITH VIDEO TAB ON PAUSE MENU
    /// </summary>


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


    public void GameOver()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        pauseTab.SetActive(true);
        OnLayerToggle(pauseMenuLayers.Length - 1);
        gameOver = true;
        gameOverScoreText.text = "Your Score:\n" + FindObjectOfType<ScoreHandler>().GetScore().ToString();

    }

    public void OnClickPlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
        Destroy(gameObject);
    }

    public void OnClickMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        Destroy(gameObject);

    }
}
