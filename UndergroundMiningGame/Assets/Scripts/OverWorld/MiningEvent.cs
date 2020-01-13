using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class MiningEvent : MonoBehaviour
{
    public Tilemap wallLayer;

    public void OnCollisionStay2D(UnityEngine.Collision2D collision)
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        Vector3 temp = new Vector3(collision.transform.position.x, collision.transform.position.y + 1, collision.transform.position.z);
        Vector3Int tilePosition = tilemap.WorldToCell(temp);
        Tile tile = (Tile)tilemap.GetTile(tilePosition);
        if (tilemap.HasTile(tilePosition))
        {
            if (tile.sprite != null)
            {
                if (tile.sprite.Equals(GetComponent<RandomMiningEvent>().placeholders))
                {
                    InteractTooltipManager.instance.Appear(collision.gameObject.transform.position + new Vector3(0, 1.0f, 0));
                }
                else
                {
                    InteractTooltipManager.instance.Disappear();
                }
            }
            else
            {
                InteractTooltipManager.instance.Disappear();
            }
        }
        else
        {
            InteractTooltipManager.instance.Disappear();
        }
        if (collision.gameObject.tag.Equals("Player") && Input.GetKeyDown(KeyCode.E))
        {
            InteractTooltipManager.instance.Disappear();
            if (tile.sprite != null)
            {
                if (tile.sprite.Equals(GetComponent<RandomMiningEvent>().placeholders))
                {
                    StartCoroutine(EnterMiningGame(tilemap, tilePosition));
                }
            }
            
        }
    }

    IEnumerator EnterMiningGame(Tilemap tilemap, Vector3Int tilePosition)
    {
        TransitionsManager.instance.Open();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(2);
        Tile tempTile = (Tile)wallLayer.GetTile(tilePosition);
        Tile wallTile = ScriptableObject.CreateInstance<Tile>();
        wallTile.sprite = tempTile.sprite;
        tilemap.SetTile(tilePosition, wallTile);
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        InteractTooltipManager.instance.Disappear();
    }
}
