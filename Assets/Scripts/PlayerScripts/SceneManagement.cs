using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public CameraEffects camEffects;
    public bool inSceneTransition = false;
    public string previousScene;
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
        GetSceneReferences();
        
    }

    void Update()
    {
        //CheckIfShouldTransition();
        if (Input.GetKeyDown(KeyCode.R))
        {
            camEffects.PlaySceneTransition(SceneManager.GetActiveScene().name);
        }
    }

    /*
     * DEPRECATED
     * 
     * void CheckIfShouldTransition()
    {
        if (!camEffects.transitioning)
        {
            if (SceneManager.GetActiveScene().name == "Cave1")
            {
                if (transform.position.x > 19.775)
                {
                    //Debug.Log("Change scene");
                    //camEffects.PlaySceneTransition("Cave2");
                }
            }
            if (SceneManager.GetActiveScene().name == "CaveRoom1")
            {
                if (transform.position.x > 4.75)
                {
                    camEffects.PlaySceneTransition("CaveRoom2");
                }
            }
            if (SceneManager.GetActiveScene().name == "CaveRoom2")
            {
                if (transform.position.x < -5.25)
                {
                    camEffects.PlaySceneTransition("CaveRoom1");
                }
            }
        }
    }*/
    public void ChangeScene(string scene)
    {
        if(scene != SceneManager.GetActiveScene().name)
        {
            instance.previousScene = SceneManager.GetActiveScene().name;
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
    
}
