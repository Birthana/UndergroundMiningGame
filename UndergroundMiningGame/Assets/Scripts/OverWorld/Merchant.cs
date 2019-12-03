using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Merchant : MonoBehaviour
{
    public string name;
    public Item itemToSell;
    public int moneyAmount;

    public bool buy;
    public bool unlock;

    public TMP_Text money;
    public Inventory inventory;
    public GameObject dialogueManager;
    public Animator animator;

    private Button yes;
    private Button no;

    private Dialogue dialogue, dialogueNo, dialoguePass, dialogueFail, dialogueSell,dialogueUnlock;

    public ZoneManager blockades;

    // FIX ANIMATION MIGHT HAVE TO OVERHAUL DIALOGMANAGER

    // Start is called before the first frame update
    void Start()
    {
        dialogue = new Dialogue(name, new[] { "Do you want to buy " + itemToSell.itemName + " for $" + moneyAmount });
        dialogueNo = new Dialogue(name, new[] { "Goodbye" });
        dialoguePass = new Dialogue(name, new[] { "Pleasure doing business." });
        dialogueFail = new Dialogue(name, new[] { "Not enough money." });

        dialogueSell = new Dialogue(name, new[] { "Wanna sell your gems?" });
        dialogueUnlock = new Dialogue(name, new[] { "Want to Unlock a new Area for $" + moneyAmount });

        money = GameObject.FindGameObjectWithTag("Money").GetComponent<TMP_Text>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager");
        animator = GameObject.FindGameObjectWithTag("YesNoButtons").GetComponent<Animator>();

        yes = GameObject.FindGameObjectWithTag("Yes").GetComponent<Button>();
        no = GameObject.FindGameObjectWithTag("No").GetComponent<Button>();
        blockades = GameObject.FindGameObjectWithTag("ZoneManager").GetComponent<ZoneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void YesBuy()
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

    public void YesSell()
    {
        Item[] temp = inventory.inventory.ToArray();
        foreach (var tempItem in temp)
        {
            if (tempItem.GetType().Equals(System.Type.GetType("GemItem")))
            {
                GemItem tempGem = (GemItem)tempItem;
                money.text = (int.Parse(money.text) + 1 * tempGem.count * (Mathf.Pow((int)tempGem.grade + 1, 3))).ToString();
                tempGem.count = 0;
                inventory.inventory.Remove(tempItem);
            }
        }
        inventory.page = 1;
        inventory.DisplayInventory();
        animator.SetBool("IsOpen", false);
        dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialoguePass, false);
    }

    public void YesUnlock()
    {
        if (int.Parse(money.text) >= moneyAmount)
        {
            blockades.UnlockZone();
            money.text = (int.Parse(money.text) - moneyAmount).ToString();
            animator.SetBool("IsOpen", false);
            dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialoguePass, false);
        }
        else
        {
            animator.SetBool("IsOpen", false);
            dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialogueFail, false);
        }
    }

    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E))
        {
            if (collision.gameObject.GetComponent<PlayerMovement>().enabled)
            {
                collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
                yes.onClick.RemoveAllListeners();
                no.onClick.RemoveAllListeners();
                if (buy)
                {
                    yes.onClick.AddListener(YesBuy);
                    no.onClick.AddListener(No);
                    dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialogue, true);

                }
                else if (unlock)
                {
                    yes.onClick.AddListener(YesUnlock);
                    no.onClick.AddListener(No);
                    dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialogueUnlock, true);

                }
                else
                {
                    yes.onClick.AddListener(YesSell);
                    no.onClick.AddListener(No);
                    dialogueManager.GetComponent<DialogueManager>().StartDialogue(dialogueSell, true);

                }

            }
            else if (!animator.GetBool("IsOpen"))
            {
                dialogueManager.GetComponent<DialogueManager>().DisplayNextSentence(false);
            }
        }
    }
}
