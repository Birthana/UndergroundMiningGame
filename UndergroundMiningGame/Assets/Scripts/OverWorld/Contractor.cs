using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contractor : MonoBehaviour
{
    public GameObject possibleMiningEvents;
    public bool cooldown = true;

    void Start()
    {
        possibleMiningEvents = GameObject.FindGameObjectWithTag("MiningEvents");
    }

    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (cooldown)
        {
            StartCoroutine(Cooldown(collision));
        }
    }

    IEnumerator Cooldown(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E))
        {
            possibleMiningEvents.GetComponent<RandomMiningEvent>().RandomMiningEventSpawn();
            cooldown = false;
            yield return new WaitForSeconds(5.0f);
            cooldown = true;
        }
    }
}
