using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    CameraEffects camEffects;
    TextMeshProUGUI fastestTime;
    TextMeshProUGUI completedRuns;
    void Start()
    {
        Cursor.visible = true;
        fastestTime = GameObject.Find("FastestTime").GetComponent<TextMeshProUGUI>();
        completedRuns = GameObject.Find("CompletedRuns").GetComponent<TextMeshProUGUI>();
        if(PlayerPrefs.HasKey("CompletedRuns") && PlayerPrefs.GetInt("CompletedRuns") > 0)
        {
            fastestTime.text = "Fastest time: " + PlayerPrefs.GetFloat("FastestTime");
            completedRuns.text = "Completed runs: " + PlayerPrefs.GetInt("CompletedRuns");
        }
        else
        {
            fastestTime.text = "";
            completedRuns.text = "";
        }
        camEffects = Camera.main.GetComponent<CameraEffects>();
    }

    public void NewGame()
    {
        Cursor.visible = false;
        PlayerPrefs.SetInt("SaveExists", 1);
        Timer.ResetTimer();
        Timer.SetTimePaused(false);
        PlayerPrefs.SetInt("unlockedGrapple", 0);
        Coin.collectedCoins.Clear();
        camEffects.PlaySceneTransition("Lab1");
        AudioManager.instance.FadeOut("TitleTheme", 1);
    }
    public void ContinueGame()
    {
        if (PlayerPrefs.GetInt("SaveExists") == 1)
        {
            Cursor.visible = false;
            Timer.SetTimePaused(false);
            camEffects.PlaySceneTransition(PlayerPrefs.GetString("currentScene"));
            AudioManager.instance.FadeOut("TitleTheme", 1);
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
