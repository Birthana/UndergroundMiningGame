using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Merchant : MonoBehaviour
{
    //public string name;
    public Item itemToSell;
    public int moneyAmount;
    public bool buy;
    public bool unlock;
    private Dialogue dialogue, dialogueNo, dialoguePass, dialogueFail, dialogueSell,dialogueUnlock;

    void Start()
    {
        dialogue = new Dialogue(name, new[] { "Do you want to buy " + itemToSell.itemName + " for $" + moneyAmount });
        dialogueNo = new Dialogue(name, new[] { "Goodbye" });
        dialoguePass = new Dialogue(name, new[] { "Pleasure doing business." });
        dialogueFail = new Dialogue(name, new[] { "Not enough money." });
        dialogueSell = new Dialogue(name, new[] { "Wanna sell your gems?" });
        dialogueUnlock = new Dialogue(name, new[] { "Want to Unlock a new Area for $" + moneyAmount });
    }

    public void YesBuy()
    {
        if(int.Parse(Inventory.instance.moneyAmount.text) >= moneyAmount)
        {
            DialogueSystem.instance.yesnoButtons.SetActive(false);
            DialogueSystem.instance.StartDialogue(dialoguePass);
            Inventory.instance.AddItem(itemToSell);
            Inventory.instance.moneyAmount.text = (int.Parse(Inventory.instance.moneyAmount.text) - moneyAmount).ToString();
        }
        else
        {
            DialogueSystem.instance.yesnoButtons.SetActive(false);
            DialogueSystem.instance.StartDialogue(dialogueFail);
        }

    }

    public void No()
    {
        DialogueSystem.instance.yesnoButtons.SetActive(false);
        DialogueSystem.instance.StartDialogue(dialogueNo);
    }

    public void YesSell()
    {
        Item[] temp = Inventory.instance.inventory.ToArray();
        foreach (var tempItem in temp)
        {
            if (tempItem.GetType().Equals(System.Type.GetType("GemItem")))
            {
                GemItem tempGem = (GemItem)tempItem;
                Inventory.instance.moneyAmount.text = (int.Parse(Inventory.instance.moneyAmount.text) + 1 * tempGem.count * (Mathf.Pow((int)tempGem.grade + 1, 3))).ToString();
                tempGem.count = 0;
                Inventory.instance.inventory.Remove(tempItem);
            }
        }
        Inventory.instance.page = 1;
        Inventory.instance.DisplayInventory();
        DialogueSystem.instance.yesnoButtons.SetActive(false);
        DialogueSystem.instance.StartDialogue(dialoguePass);
    }

    public void YesUnlock()
    {
        if (int.Parse(Inventory.instance.moneyAmount.text) >= moneyAmount)
        {
            ZoneManager.instance.UnlockZone(1);
            Inventory.instance.moneyAmount.text = (int.Parse(Inventory.instance.moneyAmount.text) - moneyAmount).ToString();
            DialogueSystem.instance.yesnoButtons.SetActive(false);
            DialogueSystem.instance.StartDialogue(dialoguePass);
        }
        else
        {
            DialogueSystem.instance.yesnoButtons.SetActive(false);
            DialogueSystem.instance.StartDialogue(dialogueFail);
        }
    }

    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (!DialogueSystem.instance.isOpen)
        {
            InteractTooltipManager.instance.Appear(this.gameObject.transform.position);
        }
        if (collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E))
        {
            InteractTooltipManager.instance.Disappear();
            if (!DialogueSystem.instance.isOpen)
            {
                collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
                DialogueSystem.instance.yesButton.onClick.RemoveAllListeners();
                DialogueSystem.instance.noButton.onClick.RemoveAllListeners();
                if (buy)
                {
                    DialogueSystem.instance.yesButton.onClick.AddListener(YesBuy);
                    DialogueSystem.instance.noButton.onClick.AddListener(No);
                    DialogueSystem.instance.StartMerchantDialogue(dialogue);
                }
                else if (unlock)
                {
                    DialogueSystem.instance.yesButton.onClick.AddListener(YesUnlock);
                    DialogueSystem.instance.noButton.onClick.AddListener(No);
                    DialogueSystem.instance.StartMerchantDialogue(dialogueUnlock);
                }
                else
                {
                    DialogueSystem.instance.yesButton.onClick.AddListener(YesSell);
                    DialogueSystem.instance.noButton.onClick.AddListener(No);
                    DialogueSystem.instance.StartMerchantDialogue(dialogueSell);
                }

            }
            else if (!DialogueSystem.instance.yesnoButtons.activeInHierarchy)
            {
                DialogueSystem.instance.DisplayNextSentence();
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        InteractTooltipManager.instance.Disappear();
    }
}
