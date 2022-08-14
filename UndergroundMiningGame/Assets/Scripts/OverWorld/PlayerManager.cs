using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;
    public SavaData playerData;
    public int maxHealth;
    public Sprite currentHammerToolSprite;
    public Boss bossToFight;
    public bool continuing;
    public bool isPaused;
    public bool animationPlaying;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetPlayerData()
    {
        if (continuing)
        {
            playerData = DataAccess.Load();
        }
        else
        {
            DataAccess.Delete();
        }
        if (playerData == null)
        {
            playerData = new SavaData();
        }
    }

}
