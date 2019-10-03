using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningEvent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Mining Game Start");
    }
}
