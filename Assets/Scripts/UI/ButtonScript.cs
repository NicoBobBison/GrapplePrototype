using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{

    // Deprecated

    SceneManagement sm;
    void Start()
    {
        sm = GameObject.Find("SceneManager").GetComponent<SceneManagement>();
    }
    public void New()
    {
        sm.NewGame();
    }
    public void Continue()
    {
        sm.ContinueGame();
    }
    
}
