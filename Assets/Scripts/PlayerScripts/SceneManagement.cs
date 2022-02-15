using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public CameraEffects camEffects;
    public bool inSceneTransition = false;
    public string previousScene;
    public bool gamePaused = false;

    public static SceneManagement instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        /*if (!PlayerPrefs.HasKey("coins"))
        {
            PlayerPrefs.SetInt("coins", 0);
        }*/
        GetSceneReferences();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        //CheckIfShouldTransition();
        if (Input.GetKeyDown(KeyCode.R))
        {
            camEffects.PlaySceneTransition(SceneManager.GetActiveScene().name);
        }
    }
    public void ChangeScene(string scene)
    {
        if(scene != SceneManager.GetActiveScene().name)
        {
            instance.previousScene = SceneManager.GetActiveScene().name;
        }
        if (Time.timeSinceLevelLoad < 0.03f)
        {
            Debug.LogWarning("Attempting to reload scene too quickly. Make sure the player's spawn and the area around it is unobstructed.");
        }
        SceneManager.LoadSceneAsync(scene);
    }
    
    
    public void GetSceneReferences()
    {
        camEffects = GameObject.Find("Main Camera").GetComponent<CameraEffects>();
        camEffects.PlaySceneTransition();
    }

    public GameObject FindSpawnPoint()
    {
        GameObject[] respawns = GameObject.FindGameObjectsWithTag("LevelExit");
        foreach(GameObject respawn in respawns)
        {
            if(respawn.name.Equals(previousScene))
            {
                return respawn;
            }
        }
        Debug.LogWarning("Couldn't find respawn that matched scene name");
        return null;
    }
    public void ToMainMenu()
    {

    }
    
    void PauseGame()
    {
        Time.timeScale = 0;
        gamePaused = true;
        camEffects.SetDimmerLevel(0.4f);
        camEffects.EnablePauseText();
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
        gamePaused = false;
        camEffects.SetDimmerLevel(0);
        camEffects.DisablePauseText();
    }
}
