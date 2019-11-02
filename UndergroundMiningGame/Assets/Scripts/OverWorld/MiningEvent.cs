using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class MiningEvent : MonoBehaviour
{
    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Tilemap tilemap = GetComponent<Tilemap>();
            Vector3 temp = new Vector3(collision.transform.position.x, collision.transform.position.y + 1, collision.transform.position.z);
            Vector3Int tilePosition = tilemap.WorldToCell(temp);
            Tile tile = (Tile) tilemap.GetTile(tilePosition);
            if (tile.sprite.Equals(GetComponent<RandomMiningEvent>().placeholder))
            {
                //tilemap.SetTile(tilePosition, null);
                //Debug.Log("Mining Game Start");
                SceneManager.LoadScene(3);
            }
        }
    }
}
