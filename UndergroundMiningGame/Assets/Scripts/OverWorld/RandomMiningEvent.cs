using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomMiningEvent : MonoBehaviour
{
    public Tilemap tilemap;
    public Sprite[] placeholders;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        RandomMiningEventSpawn();
    }

    public void RandomMiningEventSpawn()
    {
        Reset();
        int count = 0;
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, position.z);
            if (tilemap.HasTile(tilePosition))
            {
                count++;
            }
        }
        int rng = Random.Range(0, count);
        count = 0;
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, position.z);
            if (tilemap.HasTile(tilePosition))
            {
                if (count == rng)
                {
                    Tile newTile = ScriptableObject.CreateInstance<Tile>();
                    newTile.sprite = placeholders[0];
                    tilemap.SetTile(tilePosition, newTile);
                }
                count++;
            }
        }
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, position.z);
            if (tilemap.HasTile(tilePosition))
            {
                int rngInt = Random.Range(0, 10);
                if (rngInt == 0)
                {
                    Tile newTile = ScriptableObject.CreateInstance<Tile>();
                    newTile.sprite = placeholders[0];
                    tilemap.SetTile(tilePosition, newTile);
                }
            }
        }
    }

    public void Reset()
    {
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, position.z);
            if (tilemap.HasTile(tilePosition))
            {
                Tile newTile = ScriptableObject.CreateInstance<Tile>();
                newTile.sprite = placeholders[1];
                tilemap.SetTile(tilePosition, newTile);
            }
        }
    }
}
