﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class MiningEvent : MonoBehaviour
{
    public Tilemap wallLayer;

    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Tilemap tilemap = GetComponent<Tilemap>();
            Vector3 temp = new Vector3(collision.transform.position.x, collision.transform.position.y + 1, collision.transform.position.z);
            Vector3Int tilePosition = tilemap.WorldToCell(temp);
            Tile tile = (Tile) tilemap.GetTile(tilePosition);
            //if (tile.sprite.Equals(wall))
            if (tile.sprite != null)
            {
                if (tile.sprite.Equals(GetComponent<RandomMiningEvent>().placeholders))
                {
                    //
                    //Debug.Log("Mining Game Start");
                    SceneManager.LoadScene(2);
                    //LoadingScreenManager.instance.LoadLevel(3);
                    Tile tempTile = (Tile)wallLayer.GetTile(tilePosition);
                    Tile wallTile = ScriptableObject.CreateInstance<Tile>();
                    wallTile.sprite = tempTile.sprite;
                    tilemap.SetTile(tilePosition, wallTile);
                }
            }
            
        }
    }
}
