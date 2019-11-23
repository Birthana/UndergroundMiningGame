using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator;
    public Animator animator2;

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI[] temp = GameObject.FindGameObjectWithTag("Dialogue").GetComponentsInChildren<TextMeshProUGUI>();
        nameText = temp[0];
        dialogueText = temp[1];
        animator = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Animator>();
        animator2 = GameObject.FindGameObjectWithTag("YesNoButtons").GetComponent<Animator>();
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, bool isMerchant)
    {
        if (isMerchant)
        {
            animator.SetBool("IsOpen", true);
            animator2.SetBool("IsOpen", true);
        }
        else
        {
            animator.SetBool("IsOpen", true);
        }

        nameText.text = dialogue.name;

        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(isMerchant);
    }

    public void DisplayNextSentence(bool isMerchant)
    {
        if (sentences.Count == 0)
        {
            EndDialogue(isMerchant);
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    public void EndDialogue(bool isMerchant)
    {
        if (isMerchant)
        {
            animator.SetBool("IsOpen", false);
            animator2.SetBool("IsOpen", false);
        }
        else
        {
            animator.SetBool("IsOpen", false);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
    }

}
