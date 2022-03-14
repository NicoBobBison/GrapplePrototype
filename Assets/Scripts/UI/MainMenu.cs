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
        camEffects.PlaySceneTransition("Lab1");
    }
    public void ContinueGame()
    {
        camEffects.PlaySceneTransition(PlayerPrefs.GetString("currentScene"));
        AudioManager.instance.StartCoroutine(AudioManager.instance.FadeOut("TitleTheme", 1));
    }
}
