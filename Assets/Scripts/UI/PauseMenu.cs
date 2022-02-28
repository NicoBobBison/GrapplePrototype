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
        camEffects.PlaySceneTransition("MainMenu");
    }
}
