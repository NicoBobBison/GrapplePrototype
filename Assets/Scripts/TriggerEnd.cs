using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerEnd : MonoBehaviour
{
    GameObject player;
    TextMeshProUGUI finalTimeDisplay;
    bool endedGame = false;
    float finalTime;
    void Start()
    {
        player = GameObject.Find("Player");
        finalTimeDisplay = GameObject.Find("FinalTimeDisplay").GetComponent<TextMeshProUGUI>();
        AudioManager.instance.FadeOut("MainTheme");
        Timer.SetTimePaused(true);
        finalTimeDisplay.text = "";
    }
    void Update()
    {
        if(player.transform.position.y >= transform.position.y && !endedGame)
        {
            StartCoroutine(EndGame());
        }
    }
    IEnumerator EndGame()
    {
        endedGame = true;
        finalTime = Timer.GetRoundedTime();
        finalTimeDisplay.text = "Time: " + finalTime.ToString() + "\nCoins: " + PlayerPrefs.GetInt("coins");
        for(int i = 0; i < 6;  i++)
        {
            finalTimeDisplay.enabled = !finalTimeDisplay.enabled;
            yield return new WaitForSeconds(0.8f);
        }
        yield return new WaitForSeconds(5);
        PlayerPrefs.SetInt("SaveExists", 0);

        SetRecords();

        SceneManagement.instance.ToMainMenu();
    }

    void SetRecords()
    {
        if (PlayerPrefs.HasKey("CompletedRuns"))
        {
            PlayerPrefs.SetInt("CompletedRuns", PlayerPrefs.GetInt("CompletedRuns") + 1);
        }
        else
        {
            PlayerPrefs.SetInt("CompletedRuns", 1);
        }

        if (PlayerPrefs.HasKey("FastestTime"))
        {
            if (finalTime < PlayerPrefs.GetFloat("FastestTime"))
            {
                PlayerPrefs.SetFloat("FastestTime", finalTime);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("FastestTime", finalTime);
        }
    }
}
