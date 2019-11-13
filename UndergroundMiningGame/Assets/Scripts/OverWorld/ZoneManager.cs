using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public GameObject[] blockades;

    // Start is called before the first frame update
    void Start()
    {
        //UnlockZone(0);
        //UnlockZone(1);
        //UnlockZone(2);
    }

    public void UnlockZone(int index)
    {
        Destroy(blockades[index]);
    }


}
