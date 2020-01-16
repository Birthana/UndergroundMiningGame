using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager instance = null;
    public bool[] blockadeCleared;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            if (PlayerManager.instance.continuing)
            {
                blockadeCleared = PlayerManager.instance.playerData.blockades;
                for (int i = 0; i < blockadeCleared.Length; i++)
                {
                    if (blockadeCleared[i])
                    {
                        Destroy(GameObject.Find("Blockade" + i));
                    }
                }
            }
            else
            {
                blockadeCleared = new bool[14];
            }
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void UnlockZone(int index)
    {
        Destroy(GameObject.Find("Blockade" + index));
        blockadeCleared[index] = true;
    }

    public void Save()
    {
        PlayerManager.instance.playerData.SetBlockades(blockadeCleared);
    }
}
