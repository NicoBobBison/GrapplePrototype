using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CameraEffects : MonoBehaviour
{
    public Image dimmer;
    TextMeshProUGUI pauseText;
    Image resumeButton;
    Image exitButton;
    TextMeshProUGUI resumeButtonText;
    TextMeshProUGUI exitButtonText;
    public bool transitioning = false;
    public SceneManagement sm { get; private set; }
    private void Awake()
    {
        GetDimmer();
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu")) {
            GetPauseText();
            DisablePauseText();
            sm = GameObject.Find("Player").GetComponent<SceneManagement>();
        }
        Color tempColor = dimmer.color;
        tempColor.a = 1;
        dimmer.color = tempColor;
    }
    private void Start()
    {
        StartCoroutine(BrightenCam());
    }

    public void PlaySceneTransition(string nextScene)
    {
        SceneManagement.instance.GetSceneReferences();

        if (dimmer == null)
            GetDimmer();
        
        if(dimmer.color.a < 0.5f)
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

    IEnumerator DimCam(string nextScene)
    {
        transitioning = true;
        Color tempColor = dimmer.color;
        tempColor.a = dimmer.color.a;
        dimmer.color = tempColor;
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            PlayerControls pc = GameObject.Find("Player").GetComponent<PlayerControls>();
            pc.StateMachine.ChangeState(pc.SceneTransState);
        }
        
        while (dimmer.color.a < 1)
        {
            yield return new WaitForSeconds(0.005f);
            tempColor.a += 0.02f;
            dimmer.color = tempColor;

        }
        SceneManagement.instance.ChangeScene(nextScene);
        transitioning = false;
    }
    IEnumerator BrightenCam()
    {
        GetDimmer();
        transitioning = true;
        Color tempColor = dimmer.color;
        tempColor.a = 1;
        dimmer.color = tempColor;
        while (dimmer.color.a > 0)
        {
            yield return new WaitForSeconds(0.005f);
            tempColor.a -= 0.02f;
            dimmer.color = tempColor;
        }
        transitioning = false;
    }
    public void SetDimmerLevel(float level)
    {
        GetDimmer();
        Color temp = dimmer.color;
        temp.a = level;
        dimmer.color = temp;
    }
    void GetDimmer()
    {
        dimmer = GameObject.Find("Dimmer").GetComponent<Image>();
    }
    void GetPauseText()
    {
        pauseText = GameObject.Find("PauseText").GetComponent<TextMeshProUGUI>();
        resumeButton = GameObject.Find("ResumeButton").GetComponent<Image>();
        resumeButtonText = GameObject.Find("ResumeButtonText").GetComponent<TextMeshProUGUI>();
        exitButton = GameObject.Find("ExitButton").GetComponent<Image>();
        exitButtonText = GameObject.Find("ExitButtonText").GetComponent<TextMeshProUGUI>();
    }
    public void EnablePauseText()
    {
        GetPauseText();
        pauseText.enabled = true;
        resumeButton.enabled = true;
        resumeButtonText.enabled = true;
        exitButton.enabled = true;
        exitButtonText.enabled = true;
    }
    public void DisablePauseText()
    {
        GetPauseText();
        pauseText.enabled = false;
        resumeButton.enabled = false;
        resumeButtonText.enabled = false;
        exitButton.enabled = false;
        exitButtonText.enabled = false;
    }
}
