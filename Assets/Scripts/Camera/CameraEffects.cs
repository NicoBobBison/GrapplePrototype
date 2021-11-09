using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraEffects : MonoBehaviour
{
    public Image dimmer;
    public bool transitioning = false;
    PlayerSceneManagement psm;
    private void Start()
    {
        dimmer = GameObject.Find("Dimmer").GetComponent<Image>();
        Color tempColor = dimmer.color;
        tempColor.a = 1;
        dimmer.color = tempColor;
        psm = GameObject.Find("Player").GetComponent<PlayerSceneManagement>();
    }
    
    public void PlaySceneTransition(string nextScene)
    {
        if(dimmer.color.a < 0.1f)
        {
            StartCoroutine(DimCam(nextScene));
        }
        else
        {
            StartCoroutine(BrightenCam());
        }
    }

    public void PlaySceneTransition()
    {
        StartCoroutine(BrightenCam());
    }

    public IEnumerator DimCam(string nextScene)
    {
        transitioning = true;
        Color tempColor = dimmer.color;
        tempColor.a = 0;
        dimmer.color = tempColor;
        while (dimmer.color.a < 1)
        {
            yield return new WaitForSeconds(0.005f);
            //Debug.Log(tempColor.a);
            tempColor.a += 0.01f;
            dimmer.color = tempColor;
        }
        psm.ChangeScene(nextScene);
        transitioning = false;
    }
    public IEnumerator BrightenCam()
    {
        transitioning = true;
        Color tempColor = dimmer.color;
        tempColor.a = 1;
        dimmer.color = tempColor;
        while (dimmer.color.a > 0)
        {
            yield return new WaitForSeconds(0.005f);
            tempColor.a -= 0.01f;
            dimmer.color = tempColor;
        }
        transitioning = false;
    }
}
