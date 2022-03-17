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

    // Start is called before the first frame update
    void Start()
    {
        m_text = GetComponent<TMP_Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        thisPos = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(thisPos.position, player.position) < 1)
        {
            m_text.text = "Press E to Interact";
            if (Input.GetKeyDown(KeyCode.E))
                if (!DialogueManager.Instance.inDialogue && dialogue != null)
                {
                    DialogueManager.Instance.inDialogue = true;
                    StartCoroutine(DialogueManager.Instance.Type(dialogue.lines));
                }
                else
                {
                    DialogueManager.Instance.NextSentence();
                }

        }
        else
        {
            m_text.text = null;
            DialogueManager.Instance.inDialogue = false;
            StopCoroutine(DialogueManager.Instance.Type(dialogue.lines));
        }
    }
}
