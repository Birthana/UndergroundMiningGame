using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMiningEvent : MonoBehaviour
{
    public string bossName;
    public int moneyAmount;
    private Dialogue dialogueFail;
    private Dialogue dialogueNo;
    private Dialogue dialogue;

    private void Start()
    {
        dialogueFail = new Dialogue(bossName, new[] { "Not enough money." });
        dialogueNo = new Dialogue(bossName, new[] { "Blockade still stands." });
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
        DialogueSystem.instance.yesnoButtons.SetActive(false);
        DialogueSystem.instance.EndDialogue();
        yield return new WaitForSeconds(1.0f);
        Inventory.instance.moneyAmount.text = (int.Parse(Inventory.instance.moneyAmount.text) - moneyAmount).ToString();
        PlayerManager.instance.maxHealth = moneyAmount;
        SceneManager.LoadScene(3);
    }

    public void No()
    {
        DialogueSystem.instance.yesnoButtons.SetActive(false);
        DialogueSystem.instance.StartDialogue(dialogueNo);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
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
}
