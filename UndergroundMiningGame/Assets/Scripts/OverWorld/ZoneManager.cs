using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager instance = null;
    public int count;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            count = 1;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void UnlockZone()
    {
        Destroy(GameObject.Find("Blockade" + count));
        count++;
    }


}
