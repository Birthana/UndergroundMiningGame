using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    static List<string> staticObjects = new List<string>();

    void Awake()
    {
        if (!staticObjects.Contains(this.gameObject.name))
        {
            staticObjects.Add(this.gameObject.name);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
