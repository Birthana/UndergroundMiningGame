using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject yesnoButtons;
    public Button yesButton;
    public Button noButton;
    public static DialogueSystem instance = null;
    public bool isOpen;
    public bool isTalking;
    public Queue<string> sentences; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            TextMeshProUGUI[] temp = GameObject.FindGameObjectWithTag("Dialogue").GetComponentsInChildren<TextMeshProUGUI>();
            nameText = temp[0];
            dialogueText = temp[1];
            yesnoButtons = GameObject.FindGameObjectWithTag("YesNoButtons");
            yesButton = yesnoButtons.transform.GetChild(0).gameObject.GetComponent<Button>();
            noButton = yesnoButtons.transform.GetChild(1).gameObject.GetComponent<Button>();
            yesnoButtons.SetActive(false);
            isTalking = false;
            sentences = new Queue<string>();
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (!isTalking)
        {
            StartCoroutine(Open(dialogue));
        }
    }

    public void StartMerchantDialogue(Dialogue dialogue)
    {
        if (!isTalking)
        {
            StartCoroutine(MerchantOpen(dialogue));
        }
    }

    IEnumerator Open(Dialogue dialogue)
    {
        isOpen = true;
        isTalking = true;
        nameText.text = "";
        dialogueText.text = "";
        GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Animator>().SetBool("IsOpen", true);
        yield return new WaitForSeconds(1.0f);
        isTalking = false;
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach(var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    IEnumerator MerchantOpen(Dialogue dialogue)
    {
        isOpen = true;
        isTalking = true;
        nameText.text = "";
        dialogueText.text = "";
        GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Animator>().SetBool("IsOpen", true);
        yield return new WaitForSeconds(1.0f);
        isTalking = false;
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextMerchantSentence();
    }

    public void DisplayNextSentence()
    {
        if (!isTalking)
        {
            StartCoroutine(Next());
        }
    }

    public void DisplayNextMerchantSentence()
    {
        if (!isTalking)
        {
            StartCoroutine(MerchantNext());
        }
    }

    IEnumerator Next()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
        }
        else
        {
            isTalking = true;
            dialogueText.text = sentences.Dequeue();
            yield return new WaitForSeconds(0.5f);
            isTalking = false;
        }
    }

    IEnumerator MerchantNext()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
        }
        else
        {
            isTalking = true;
            dialogueText.text = sentences.Dequeue();
            yield return new WaitForSeconds(0.1f);
            yesnoButtons.SetActive(true);
            isTalking = false;
        }
    }

    public void EndDialogue()
    {
        if (!isTalking)
        {
            StartCoroutine(End());
        }
    }

    IEnumerator End()
    {
        isOpen = false;
        isTalking = true;
        GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Animator>().SetBool("IsOpen", false);
        yield return new WaitForSeconds(1.0f);
        isTalking = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
    }
}
