using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    CameraEffects camEffects;
    TextMeshProUGUI fastestTime;
    TextMeshProUGUI completedRuns;
    [SerializeField]
    TextMeshProUGUI resetButtonText;
    [SerializeField]
    GameObject leaderboard;

    bool shouldResetScore = false;
    void Start()
    {
        shouldResetScore = false;
        Cursor.visible = true;
        fastestTime = GameObject.Find("FastestTime").GetComponent<TextMeshProUGUI>();
        completedRuns = GameObject.Find("CompletedRuns").GetComponent<TextMeshProUGUI>();
        if(PlayerPrefs.HasKey("CompletedRuns") && PlayerPrefs.GetInt("CompletedRuns") > 0)
        {
            UpdateTimeAndRuns();
        }
        else
        {
            fastestTime.text = "";
            completedRuns.text = "";
        }
        camEffects = Camera.main.GetComponent<CameraEffects>();
        DisplayLeaderboards();
    }

   

    public void NewGame()
    {
        Cursor.visible = false;
        PlayerPrefs.SetInt("coins", 0);
        PlayerPrefs.SetInt("SaveExists", 1);
        Timer.ResetTimer();
        Timer.SetTimePaused(false);
        PlayerPrefs.SetInt("unlockedGrapple", 0);
        Coin.collectedCoins.Clear();
        camEffects.PlaySceneTransition("Lab1");
        AudioManager.instance.FadeOut("TitleTheme");
    }
    public void ContinueGame()
    {
        if (PlayerPrefs.GetInt("SaveExists") == 1)
        {
            Cursor.visible = false;
            Timer.SetTimePaused(false);
            camEffects.PlaySceneTransition(PlayerPrefs.GetString("currentScene"));
            AudioManager.instance.FadeOut("TitleTheme");
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ResetButton()
    {
        ConfirmReset();
    }
    void ConfirmReset()
    {
        if (!shouldResetScore)
        {
            resetButtonText.text = "Click again to confirm";
            shouldResetScore = true;
        }
        else
        {
            resetButtonText.text = "Reset high score";
            ResetScore();
            shouldResetScore = false;
        }
    }
    void ResetScore()
    {
        PlayerPrefs.SetFloat("FastestTime", -1);
        UpdateTimeAndRuns();
    }
    private void DisplayLeaderboards()
    {
        if(PlayerPrefs.GetInt("CompletedRuns") == 0)
        {
            leaderboard.SetActive(false);
        }
        else
        {
            leaderboard.SetActive(true);
        }
    }
    void UpdateTimeAndRuns()
    {
        if (PlayerPrefs.HasKey("FastestTime") && PlayerPrefs.GetFloat("FastestTime") != -1)
            fastestTime.text = "Fastest time: " + PlayerPrefs.GetFloat("FastestTime");
        else
            fastestTime.text = "Fastest time: No runs completed";
        completedRuns.text = "Completed runs: " + PlayerPrefs.GetInt("CompletedRuns");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            PlayerPrefs.SetFloat("FastestTime", 12);
            UpdateTimeAndRuns();
        }
    }
}
