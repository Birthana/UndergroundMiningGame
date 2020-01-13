using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMiningEvent : MonoBehaviour
{
    public Boss bossInfo;
    public string bossName;
    public int moneyAmount;
    private Dialogue dialogueFail;
    private Dialogue dialogueNo;
    private Dialogue dialogue;
    public bool enterBossFight;

    private void Start()
    {
        dialogueFail = new Dialogue(bossName, new[] { "You place the money on it. Nothing happens. You grab back the money you place." });
        dialogueNo = new Dialogue(bossName, new[] { "The statue still stands." });
        dialogue = new Dialogue(bossName, new[] { "A statue stands in the way. Place $" + moneyAmount + " on it?" });
    }

    public void Yes()
    {
        if (int.Parse(Inventory.instance.moneyAmount.text) >= moneyAmount)
        {
            StartCoroutine(EnterBossMiningEvent());
        }
        else
        {
            DialogueSystem.instance.yesnoButtons.SetActive(false);
            DialogueSystem.instance.StartDialogue(dialogueFail);
        }
    }

    IEnumerator EnterBossMiningEvent()
    {
        enterBossFight = true;
        DialogueSystem.instance.yesnoButtons.SetActive(false);
        DialogueSystem.instance.EndDialogue();
        PlayerManager.instance.bossToFight = bossInfo;
        TransitionsManager.instance.Open();
        yield return new WaitForSeconds(1.0f);
        Inventory.instance.moneyAmount.text = (int.Parse(Inventory.instance.moneyAmount.text) - moneyAmount).ToString();
        PlayerManager.instance.maxHealth = moneyAmount;
        SceneManager.LoadScene(3);
        enterBossFight = false;
    }

    public void No()
    {
        DialogueSystem.instance.yesnoButtons.SetActive(false);
        DialogueSystem.instance.StartDialogue(dialogueNo);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (!DialogueSystem.instance.isOpen && !enterBossFight)
        {
            InteractTooltipManager.instance.Appear(this.gameObject.transform.position);
        }
        if (collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            InteractTooltipManager.instance.Disappear();
            if (!DialogueSystem.instance.isOpen)
            {
                collision.gameObject.GetComponent<PlayerMovement>().enabled = false;
                DialogueSystem.instance.yesButton.onClick.RemoveAllListeners();
                DialogueSystem.instance.noButton.onClick.RemoveAllListeners();
                DialogueSystem.instance.yesButton.onClick.AddListener(Yes);
                DialogueSystem.instance.noButton.onClick.AddListener(No);
                DialogueSystem.instance.StartMerchantDialogue(dialogue);
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
