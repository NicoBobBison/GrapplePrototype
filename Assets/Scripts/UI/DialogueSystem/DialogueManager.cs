using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    [SerializeField] Image TextBox;
    [SerializeField] TMP_Text TextBox_TMP;
    private int sentenceIndex;
    [SerializeField] float typeSpeed = 0.1f;
    public bool inDialogue = false;
    bool isTyping = false;
    public string[] currentDialogue;
    public Coroutine currentType;
    int pointInSentence = 0;
    public GameObject currentSource;
    bool inMainMenu = true;
    public string interactDisplayText { get; private set; }

    public Vector2 bottomTextLocation = new Vector2(0, -420);
    public Vector2 topTextLocation = new Vector2(0, 420);


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        interactDisplayText = "Press E";

        inMainMenu = SceneManagement.instance.GetScene().name.Equals("MainMenu");
        GetDialogueReferences();
        if (!inMainMenu)
        {
            TextBox.enabled = false;
            TextBox_TMP.text = "";
        }
    }
    private void Update()
    {
        if (SceneManagement.instance.GetScene().name.Equals("MainMenu"))
        {
            inMainMenu = true;
        }
        else
        {
            inMainMenu = false;
        }
        if (!inDialogue && !inMainMenu)
        {
            if (TextBox == null)
                GetDialogueReferences();
            TextBox.enabled = false;
            TextBox_TMP.text = "";
        }
    }
    public IEnumerator Type(string[] dialogue)
    {
        if (inDialogue)
        {
            pointInSentence = 0;
            isTyping = true;
            
            int camHeight = Camera.main.pixelHeight;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float playerYToScreenSpace = Camera.main.WorldToScreenPoint(player.transform.position).y;
            if(playerYToScreenSpace > camHeight / 2)
            {
                TextBox.rectTransform.anchoredPosition = bottomTextLocation;
            }
            else
            {
                TextBox.rectTransform.anchoredPosition = topTextLocation;
            }

            TextBox.enabled = true;
            currentDialogue = dialogue;
            foreach (char letter in dialogue[sentenceIndex].ToCharArray())
            {
                if (inDialogue)
                {
                    pointInSentence++;
                    TextBox_TMP.text += letter;
                    AudioManager.instance.Play("DialogueBeep");
                    yield return new WaitForSeconds(typeSpeed);
                }
            }
            isTyping = false;
        }
    }
    public void NextSentence()
    {
        if (isTyping)
        {
            StopCoroutine(currentType);
            TextBox_TMP.text += currentDialogue[sentenceIndex].Substring(pointInSentence);
            isTyping = false;
        }
        else
        {
            if (sentenceIndex < currentDialogue.Length - 1)
            {
                sentenceIndex++;
                TextBox_TMP.text = "";
                currentType = StartCoroutine(Type(currentDialogue));
            }
            else
            {
                TextBox.enabled = false;
                TextBox_TMP.text = "";
                sentenceIndex = 0;
                inDialogue = false;
            }
        }
    }
    public void ResetSentenceIndex()
    {
        sentenceIndex = 0;
    }
    public void GetDialogueReferences()
    {
        if (inMainMenu)
            return;
        TextBox = GameObject.Find("TextBox").GetComponent<Image>();
        TextBox_TMP = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
    }
}
