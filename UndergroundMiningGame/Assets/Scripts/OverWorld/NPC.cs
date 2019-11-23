using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GameObject.Find("DialogueManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        
        StartCoroutine(DialogueDisplay(collision));
    }

    IEnumerator DialogueDisplay(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().enabled)
            {
                collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
                dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialogue, false);
            }
            else
            {
                dialogueManager.GetComponent<DialogueManager>().DisplayNextSentence(false);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
