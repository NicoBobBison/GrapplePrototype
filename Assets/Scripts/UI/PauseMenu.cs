using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    CameraEffects camEffects;
    void Start()
    {
        camEffects = Camera.main.GetComponent<CameraEffects>();
    }
    public void Resume()
    {
        SceneManagement.instance.ResumeGame();
    }
    public void ToMainMenu()
    {
        PlayerPrefs.SetString("currentScene", SceneManager.GetActiveScene().name);
        AudioManager.instance.StartCoroutine(AudioManager.instance.FadeOut("MainTheme", 1));
        camEffects.PlaySceneTransition("MainMenu");
    }
    public void toSettings()
    {
        SceneManagement.instance.inSettings = true;
        camEffects.EnableSettings();
    }
}
