using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningEvent : MonoBehaviour
{
    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Mining Game Start");
        }
    }
}
