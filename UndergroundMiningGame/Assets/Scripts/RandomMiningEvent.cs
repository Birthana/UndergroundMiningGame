using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMiningEvent : MonoBehaviour
{
    public Tilemap tilemap;
    public Sprite placeholder;

    // Start is called before the first frame update
    void Start()
    {
        RandomMiningEventSpawn();
    }

    public void RandomMiningEventSpawn()
    {
        tilemap = GetComponent<Tilemap>();
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, position.z);
            if (tilemap.HasTile(tilePosition))
            {
                int rngInt = Random.Range(0, 10);
                if (rngInt == 0)
                {
                    Tile newTile = ScriptableObject.CreateInstance<Tile>();
                    newTile.sprite = placeholder;
                    tilemap.SetTile(tilePosition, newTile);
                    //Debug.Log("Replaced Tile at " + tilePosition);
                }
                else
                {
                    //Debug.Log("Kept Tile at " + tilePosition);
                }

            }
        }
    }
}
