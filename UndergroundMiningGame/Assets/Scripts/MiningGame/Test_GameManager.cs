using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Test_GameManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap gemMap;
    public Camera cam;
    public Sprite gemSprite;
    public Sprite[] gemSprite2x2 = new Sprite[4];
    public Sprite[] gemSprite3x3 = new Sprite[9];

    public int clicks = 0;
    public int max = 15;
    public Gem[] gemArray;
    int points = 0;

    public TextMeshProUGUI clickCountRatio;
    public GameObject healthBar;

    void Start()
    {
        
        tilemap = GetComponent<Tilemap>();
        cam = Camera.main;
        gemArray = GenerateGems();
    }

    public Gem[] GenerateGems()
    {
        int x = 16;
        int y = 8;
        Gem[] spawnGems = new Gem[Random.Range(1, 6)];
        for (int i = 0; i < spawnGems.Length; i++)
        {
            bool creatingGem = true;
            while (creatingGem)
            {
                Vector3Int rngPosition = new Vector3Int(Random.Range(0, x), Random.Range(0, y), 0);
                Tile rngTile = (Tile) gemMap.GetTile(rngPosition);
                int size = Random.Range(1, 4);
                if (size == 1)
                {
                    if (rngTile == null)
                    {
                        creatingGem = false;
                        Tile newTile = ScriptableObject.CreateInstance<Tile>();
                        newTile.sprite = gemSprite;
                        gemMap.SetTile(rngPosition, newTile);
                        spawnGems[i] = new Gem(size, rngPosition);
                    }
                }else if (size == 2)
                {
                    if (rngPosition.x != x-1 && rngPosition.y != y-1)
                    {
                        Tile rngTile1 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(1, 0, 0));
                        Tile rngTile2 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(0, 1, 0));
                        Tile rngTile3 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(1, 1, 0));
                        if (rngTile == null && rngTile1 == null
                            && rngTile2 == null && rngTile3 == null)
                        {
                            creatingGem = false;
                            Tile bottomLeft = ScriptableObject.CreateInstance<Tile>();
                            bottomLeft.sprite = gemSprite2x2[0];
                            Tile bottomRight = ScriptableObject.CreateInstance<Tile>();
                            bottomRight.sprite = gemSprite2x2[1];
                            Tile topLeft = ScriptableObject.CreateInstance<Tile>();
                            topLeft.sprite = gemSprite2x2[2];
                            Tile topRight = ScriptableObject.CreateInstance<Tile>();
                            topRight.sprite = gemSprite2x2[3];
                            gemMap.SetTile(rngPosition, bottomLeft);
                            gemMap.SetTile(rngPosition + new Vector3Int(1, 0, 0), bottomRight);
                            gemMap.SetTile(rngPosition + new Vector3Int(0, 1, 0), topLeft);
                            gemMap.SetTile(rngPosition + new Vector3Int(1, 1, 0), topRight);
                            spawnGems[i] = new Gem(size, rngPosition);
                        }
                    }
                }
                else if (size == 3)
                {
                    if (rngPosition.x != x-1 && rngPosition.x != x-2
                        && rngPosition.y != y-1 && rngPosition.y != y-2)
                    {
                        Tile rngTile1 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(1, 0, 0));
                        Tile rngTile2 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(0, 1, 0));
                        Tile rngTile3 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(1, 1, 0));
                        Tile rngTile4 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(0, 2, 0));
                        Tile rngTile5 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(1, 2, 0));
                        Tile rngTile6 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(2, 0, 0));
                        Tile rngTile7 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(2, 1, 0));
                        Tile rngTile8 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(2, 2, 0));
                        if (rngTile == null && rngTile1 == null && rngTile2 == null && rngTile3 == null
                            && rngTile4 == null && rngTile5 == null && rngTile6 == null && rngTile7 == null
                            && rngTile8 == null)
                        {
                            creatingGem = false;
                            Tile bottomLeft = ScriptableObject.CreateInstance<Tile>();
                            bottomLeft.sprite = gemSprite3x3[0];
                            Tile bottomMiddle = ScriptableObject.CreateInstance<Tile>();
                            bottomMiddle.sprite = gemSprite3x3[1];
                            Tile bottomRight = ScriptableObject.CreateInstance<Tile>();
                            bottomRight.sprite = gemSprite3x3[2];
                            Tile centerLeft = ScriptableObject.CreateInstance<Tile>();
                            centerLeft.sprite = gemSprite3x3[3];
                            Tile centerMiddle = ScriptableObject.CreateInstance<Tile>();
                            centerMiddle.sprite = gemSprite3x3[4];
                            Tile centerRight = ScriptableObject.CreateInstance<Tile>();
                            centerRight.sprite = gemSprite3x3[5];
                            Tile topLeft = ScriptableObject.CreateInstance<Tile>();
                            topLeft.sprite = gemSprite3x3[6];
                            Tile topMiddle = ScriptableObject.CreateInstance<Tile>();
                            topMiddle.sprite = gemSprite3x3[7];
                            Tile topRight = ScriptableObject.CreateInstance<Tile>();
                            topRight.sprite = gemSprite3x3[8];
                            gemMap.SetTile(rngPosition, bottomLeft);
                            gemMap.SetTile(rngPosition + new Vector3Int(1, 0, 0), bottomMiddle);
                            gemMap.SetTile(rngPosition + new Vector3Int(0, 1, 0), centerLeft);
                            gemMap.SetTile(rngPosition + new Vector3Int(1, 1, 0), centerMiddle);
                            gemMap.SetTile(rngPosition + new Vector3Int(0, 2, 0), topLeft);
                            gemMap.SetTile(rngPosition + new Vector3Int(1, 2, 0), topMiddle);
                            gemMap.SetTile(rngPosition + new Vector3Int(2, 0, 0), bottomRight);
                            gemMap.SetTile(rngPosition + new Vector3Int(2, 1, 0), centerRight);
                            gemMap.SetTile(rngPosition + new Vector3Int(2, 2, 0), topRight);
                            spawnGems[i] = new Gem(size, rngPosition);
                        }
                    }
                }
            }
        }
        return spawnGems;
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 clickedPosition = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector3Int tilePosition = tilemap.WorldToCell(clickedPosition);
            if (tilemap.HasTile(tilePosition))
            {
                //Debug.Log(tilePosition);
                clicks++;
                clickCountRatio.text = (max - clicks) + " / " + max;
                healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(max - clicks, max);
                tilemap.SetTile(tilePosition, null);
            }
        }

        if (clicks >= max)//end game
        {
            foreach (var Gem in gemArray)
            {
                if (Gem.getSize() == 1)
                {
                    points += tilemap.HasTile(Gem.getPosition()) ? 0 : 1;
                }
                else if (Gem.getSize() == 2)
                {
                    points += (tilemap.HasTile(Gem.getPosition())
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(1, 0, 0))
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(0, 1, 0))
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(1, 1, 0))
                        ) ? 0 : 4;
                }
                else if (Gem.getSize() == 3)
                {
                    points += (tilemap.HasTile(Gem.getPosition())
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(0, 1, 0))
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(0, 2, 0))
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(1, 0, 0))
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(1, 1, 0))
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(1, 2, 0))
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(2, 0, 0))
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(2, 1, 0))
                        || tilemap.HasTile(Gem.getPosition() + new Vector3Int(2, 2, 0))
                        ) ? 0 : 9;
                }
            }

            Debug.Log(points);
            //END
            SceneManager.LoadScene(1);
        }
    }

}
