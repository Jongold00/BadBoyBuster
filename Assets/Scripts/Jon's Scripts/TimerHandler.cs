using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TimerHandler : MonoBehaviour
{
    public float startingTime = 300.0f; // all in seconds
    public float timeRemaining;

    [SerializeField]
    TextMeshProUGUI timerLabel;

    [SerializeField]
    Image timerFill;

    [SerializeField]
    GameObject lowTimeGO;

    [SerializeField]
    GameObject themeStartGO;

    private bool gameOver = false;

    private void Start()
    {
        timeRemaining = startingTime;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        timerFill.fillAmount = timeRemaining / startingTime;
        timerLabel.text = ConvertToMinutesAndSeconds(timeRemaining);
        if (timeRemaining <= 0 && !gameOver)
        {
            gameOver = true;
            GameOver();
        }

        if (timeRemaining <= 35)
        {
            TriggerLowTime();
        }
    }

    string ConvertToMinutesAndSeconds(float time)
    {
        int mins = Mathf.FloorToInt(Mathf.FloorToInt(time) / 60);
        int tenSeconds = Mathf.FloorToInt(time % 60) / 10;
        int seconds = Mathf.FloorToInt(time % 60) % 10;
        return mins.ToString() + ":" + tenSeconds.ToString() + seconds.ToString();
    }

    void GameOver()
    {
        FindObjectOfType<InGameUIHandler>().GameOver();
    }

    void TriggerLowTime()
    {
        themeStartGO.SetActive(false);
        lowTimeGO.SetActive(true);
    }
}
