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
    private int index;
    [SerializeField] float typeSpeed = 0.1f;
    public bool inDialogue = false;
    public bool isTyping = false;
    public string[] currentDialogue;


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
        TextBox.enabled = false;
        TextBox_TMP.text = "";
    }
    private void Update()
    {
        if (!inDialogue)
        {
            TextBox.enabled = false;
            TextBox_TMP.text = "";
        }
    }
    public IEnumerator Type(string[] dialogue)
    {
        if (inDialogue)
        {
            isTyping = true;
            TextBox.enabled = true;
            currentDialogue = dialogue;
            foreach (char letter in dialogue[index].ToCharArray())
            {
                TextBox_TMP.text += letter;
                yield return new WaitForSeconds(typeSpeed);
            }
            //Debug.Log(dialogue[0]);
            isTyping = false;
        }
    }
    public void NextSentence()
    {
        if (!isTyping)
        {
            if (index < currentDialogue.Length - 1)
            {
                index++;
                TextBox_TMP.text = "";
                StartCoroutine(Type(currentDialogue));
            }
            else
            {
                TextBox.enabled = false;
                TextBox_TMP.text = "";
                index = 0;
                inDialogue = false;
            }
        }
    }
}
