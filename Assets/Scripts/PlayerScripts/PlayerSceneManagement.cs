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
        if (SceneManager.GetActiveScene().name == "Cave1")
        {
            if (transform.position.x > 19.775f && !camEffects.transitioning)
            {
                //Debug.Log("Change scene");
                //camEffects.PlaySceneTransition("Cave2");
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
