using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneMusic : MonoBehaviour
{
    public AudioSource overworld;
    public GameObject player;
    public Tilemap tileMap;
    public Sprite currentSprite;
    public AudioClip[] overworldThemes;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        tileMap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
        overworld = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = player.GetComponent<RectTransform>().transform.position;
        Vector3Int tilePosition = tileMap.WorldToCell(playerPosition);
        AudioClip tempClip = overworldThemes[0];
        if (tileMap.HasTile(tilePosition))
        {
            Tile playerTile = (Tile)tileMap.GetTile(tilePosition);
            currentSprite = playerTile.sprite;
            string tileSprite = playerTile.sprite.name;
            if (tileSprite.Equals("general_spritesheet2_19"))
            {
                tempClip = overworldThemes[0];
            }else if (tileSprite.Equals("redrock_spritesheet_24"))
            {
                tempClip = overworldThemes[1];
            }
            else if (tileSprite.Equals("ice_spritesheet2_20"))
            {
                tempClip = overworldThemes[2];
            }
            else if (tileSprite.Equals("magma_spritesheet_22"))
            {
                tempClip = overworldThemes[3];
            }
            else if (tileSprite.Equals("water_spritesheet_23"))
            {
                tempClip = overworldThemes[4];
            }
            else if (tileSprite.Equals("desert_spritesheet_27"))
            {
                tempClip = overworldThemes[5];
            }
            else if (tileSprite.Equals("mushroom_spritesheet_20"))
            {
                tempClip = overworldThemes[6];
            }
        }
        if (!overworld.clip.Equals(tempClip))
        {
            overworld.clip = tempClip;
            overworld.loop = true;
            overworld.Play();
        }
    }
}
