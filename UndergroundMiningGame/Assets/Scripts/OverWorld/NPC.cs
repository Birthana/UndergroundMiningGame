using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        StartCoroutine(DialogueDisplay(collision));
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        InteractTooltipManager.instance.Disappear();
    }

    IEnumerator DialogueDisplay(UnityEngine.Collision2D collision)
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
                DialogueSystem.instance.StartDialogue(dialogue);
            }
            else
            {
                DialogueSystem.instance.DisplayNextSentence();
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
