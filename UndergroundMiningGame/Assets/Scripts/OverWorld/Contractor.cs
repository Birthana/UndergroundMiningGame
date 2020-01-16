using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contractor : MonoBehaviour
{
    public GameObject possibleMiningEvents;
    public bool cooldown;

    void Start()
    {
        possibleMiningEvents = GameObject.FindGameObjectWithTag("MiningEvents");
    }

    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E) & !cooldown)
        {
            cooldown = true;
            Save();
            possibleMiningEvents.GetComponent<RandomMiningEvent>().RandomMiningEventSpawn();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        cooldown = false;
    }

    public void Save()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerManager.instance.playerData.SetPosition(player.transform.position);
        Inventory.instance.Save();
        ZoneManager.instance.Save();
        DataAccess.Save(PlayerManager.instance.playerData);
    }
}
