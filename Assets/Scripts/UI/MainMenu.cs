using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    CameraEffects camEffects;
    void Start()
    {
        camEffects = Camera.main.GetComponent<CameraEffects>();
    }

    public void NewGame()
    {
        Timer.ResetTimer();
        PlayerPrefs.SetInt("unlockedGrapple", 0);
        Coin.collectedCoins.Clear();
        camEffects.PlaySceneTransition("Lab1");
        AudioManager.instance.FadeOut("TitleTheme", 1);
    }
    public void ContinueGame()
    {
        Timer.SetTimePaused(false);
        camEffects.PlaySceneTransition(PlayerPrefs.GetString("currentScene"));
        AudioManager.instance.FadeOut("TitleTheme", 1);
    }
}
