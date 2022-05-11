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
    Image settingsButton;
    TextMeshProUGUI resumeButtonText;
    TextMeshProUGUI exitButtonText;
    TextMeshProUGUI settingsButtonText;
    TextMeshProUGUI timerText;
    GameObject settings;
    float camShakeDistance = 0.1f;
    float camShakeTime = 0.05f;
    [SerializeField] CameraData cameraData;


    public bool transitioning = false;
    public SceneManagement sm { get; private set; }
    private void Awake()
    {
        
    }
    private void Start()
    {
        GetDimmer();
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            GetPauseText();
            DisablePauseText();
            sm = GameObject.Find("Player").GetComponent<SceneManagement>();
        }
        Color tempColor = dimmer.color;
        tempColor.a = 1;
        dimmer.color = tempColor;
    }

    public void PlaySceneTransition(string nextScene)
    {
        Debug.Log("Transition");
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
        Debug.Log("Dim cam");
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
            yield return new WaitForFixedUpdate();
            if (tempColor.a < 0.95f)
            {
                tempColor.a += 0.05f;
            }
            else
            {
                tempColor.a = 1;
            }
            //Debug.Log("a (dim): " + tempColor.a);
            dimmer.color = tempColor;

        }
        SceneManagement.instance.ChangeScene(nextScene);
        transitioning = false;
    }
    IEnumerator BrightenCam()
    {
        Debug.Log("Brighten cam");
        GetDimmer();
        transitioning = true;
        Color tempColor = dimmer.color;
        tempColor.a = 1;
        dimmer.color = tempColor;
        while (dimmer.color.a > 0)
        {
            yield return new WaitForFixedUpdate();
            if(tempColor.a > 0.05f)
            {
                tempColor.a -= 0.05f;
            }
            else
            {
                tempColor.a = 0;
            }
            //Debug.Log("a (brighten): " + tempColor.a);

            dimmer.color = tempColor;
        }
        transitioning = false;
    }
    public IEnumerator camShake(float horizontal, float vertical)
    {
        if (cameraData != null && cameraData.camShake)
        {
            Debug.Log("Cam shake");

            Vector3 originalPos = transform.position;
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (2 * horizontal * camShakeDistance),
            this.gameObject.transform.position.y + (vertical * camShakeDistance),
            this.gameObject.transform.position.z);
            yield return new WaitForSeconds(camShakeTime);
            this.gameObject.transform.position = originalPos;
            yield return new WaitForSeconds(camShakeTime);
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (0.5f * horizontal * camShakeDistance),
                this.gameObject.transform.position.y + (vertical * camShakeDistance),
                this.gameObject.transform.position.z);
            yield return new WaitForSeconds(camShakeTime);
            this.gameObject.transform.position = originalPos;
        }
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
        settingsButton = GameObject.Find("SettingsButton").GetComponent<Image>();
        settingsButtonText = GameObject.Find("SettingsButtonText").GetComponent<TextMeshProUGUI>();
        settings = GameObject.Find("SettingsMenu");
        timerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();
    }
    public void EnablePauseText()
    {
        GetPauseText();
        pauseText.enabled = true;
        resumeButton.enabled = true;
        resumeButtonText.enabled = true;
        exitButton.enabled = true;
        exitButtonText.enabled = true;
        settingsButton.enabled = true;
        settingsButtonText.enabled = true;
        settings.SetActive(true);
        timerText.enabled = true;
    }
    public void DisablePauseText()
    {
        GetPauseText();
        pauseText.enabled = false;
        resumeButton.enabled = false;
        resumeButtonText.enabled = false;
        exitButton.enabled = false;
        exitButtonText.enabled = false;
        settingsButton.enabled = false;
        settingsButtonText.enabled = false;
        timerText.enabled = false;
        DisableSettings();
    }

    public void DisableSettings()
    {
        CanvasGroup settingsGroup = GameObject.Find("SettingsMenu").GetComponent<CanvasGroup>();

        if (settings != null)
        {
            settingsGroup.alpha = 0;
            settingsGroup.blocksRaycasts = false;
        }
        else
        {
            Debug.LogWarning("Settings reference not found!");
        }
    }
    public void EnableSettings()
    {
        CanvasGroup settingsGroup = GameObject.Find("SettingsMenu").GetComponent<CanvasGroup>();

        if (settings != null)
        {
            settingsGroup.alpha = 1;
            settingsGroup.blocksRaycasts = true;
        }
        else
        {
            Debug.LogWarning("Settings reference not found!");
        }
    }
}
