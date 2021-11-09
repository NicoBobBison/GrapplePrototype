using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneManagement : MonoBehaviour
{
    public CameraEffects camEffects;
    public bool inSceneTransition = false;
    void Start()
    {
        camEffects = GameObject.Find("Main Camera").GetComponent<CameraEffects>();
        camEffects.PlaySceneTransition();
    }

    void Update()
    {
        CheckIfShouldTransition();
    }

    void CheckIfShouldTransition()
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
    }
    public void ChangeScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }
    public string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }
}
