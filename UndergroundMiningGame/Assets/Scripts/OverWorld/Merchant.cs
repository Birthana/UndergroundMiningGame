using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Merchant : MonoBehaviour
{
    public string name;
    public Item itemToSell;
    public int moneyAmount;

    public TMP_Text money;
    public Inventory inventory;
    public GameObject dialogueManager;
    public Animator animator;

    private Dialogue dialogue, dialogueNo, dialoguePass, dialogueFail;

    // FIX ANIMATION MIGHT HAVE TO OVERHAUL DIALOGMANAGER

    // Start is called before the first frame update
    void Start()
    {
        dialogue = new Dialogue(name, new[] { "Do you want to buy " + itemToSell.itemName + " for $" + moneyAmount });
        dialogueNo = new Dialogue(name, new[] { "Goodbye" });
        dialoguePass = new Dialogue(name, new[] { "Pleasure doing business." });
        dialogueFail = new Dialogue(name, new[] { "Not enough money." });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Yes()
    {
        if(int.Parse(money.text) >= moneyAmount)
        {
            animator.SetBool("IsOpen", false);
            dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialoguePass, false);
            inventory.AddItem(itemToSell);
            money.text = (int.Parse(money.text) - moneyAmount).ToString();
        }
        else
        {
            animator.SetBool("IsOpen", false);
            dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialogueFail, false);
        }

    }

    public void No()
    {
        animator.SetBool("IsOpen", false);
        dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialogueNo, false);

    }

    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().enabled)
            {
                collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
                dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialogue, true);
            }
            else if (!animator.GetBool("IsOpen"))
            {
                dialogueManager.GetComponent<DialogueManager>().DisplayNextSentence(false);
            }
        }
    }
}
