using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    static GameObject inventory;

    void Awake()
    {
        DontDestroyOnLoad(this.transform.gameObject);

        if (inventory == null)
        {
            inventory = this.transform.gameObject;
        }else
        {
            Destroy(this.gameObject);
        }
    }
}
