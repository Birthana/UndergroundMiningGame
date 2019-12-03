using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public GameObject[] blockades;
    public int count;

    // Start is called before the first frame update
    void Start()
    {
        count = 0;
        //UnlockZone(0);
        //UnlockZone(1);
        //UnlockZone(2);
    }

    public void UnlockZone()
    {
        Destroy(blockades[count]);
        count++;
    }


}
