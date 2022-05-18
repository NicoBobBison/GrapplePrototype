using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractText : MonoBehaviour
{
    private TMP_Text m_text;
    private Transform player;
    private Transform thisPos;
    [SerializeField] Dialogue dialogue;

    Coroutine currentInteractText;

    // Start is called before the first frame update
    void Start()
    {
        m_text = GetComponentInChildren<TMP_Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        thisPos = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(thisPos.position, player.position) < 1)
        {
            if(currentInteractText == null)
            {
                currentInteractText = StartCoroutine(DisplayInteractText());
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                StopCoroutine(currentInteractText);
                m_text.text = "";

                CheckIfSpecialDialogue();
                if (!DialogueManager.Instance.inDialogue && dialogue != null)
                {
                    // Start new dialogue
                    DialogueManager.Instance.currentSource = this.gameObject;
                    DialogueManager.Instance.ResetSentenceIndex();
                    DialogueManager.Instance.inDialogue = true;
                    DialogueManager.Instance.currentType = StartCoroutine(DialogueManager.Instance.Type(dialogue.lines));
                }
                else
                {
                    // Continue to next sentence
                    DialogueManager.Instance.NextSentence();
                }
            }
        }
        else
        {
            m_text.text = "";
            if(currentInteractText != null)
            {
                StopCoroutine(currentInteractText);
                currentInteractText = null;
            }
            if (gameObject == DialogueManager.Instance.currentSource)
            {
                DialogueManager.Instance.inDialogue = false;
                if (DialogueManager.Instance.currentType != null)
                    DialogueManager.Instance.StopCoroutine(DialogueManager.Instance.currentType);
            } 
        }
    }

    IEnumerator DisplayInteractText()
    {
        foreach(char letter in DialogueManager.Instance.interactDisplayText.ToCharArray())
        {
            m_text.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
    void CheckIfSpecialDialogue()
    {
        if (gameObject.name == "UnlockGrapple")
        {
            PlayerPrefs.SetInt("unlockedGrapple", 1);
        }
    }
}