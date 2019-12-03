using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Test_GameManager : MonoBehaviour
{
    public Tilemap[] tilemap;
    public Tilemap gemMap;
    public Camera cam;
    public Sprite[] smallGemSprites;
    public Sprite[] gemSprite2x2 = new Sprite[4];
    public Sprite[] gemSprite3x3 = new Sprite[9];

    public int clicks = 0;
    public int max = 30;
    public Gem[] gemArray;
    //int points = 0;

    public TextMeshProUGUI clickCountRatio;
    public GameObject healthBar;

    public Inventory inventory;
    public Item[] placeholders;

    GameObject canvas;

    public GameObject selector;

    public GameObject player;
    public GameObject overworld;
    public GameObject blockade;

    public AudioSource hammerSound;
    public AudioSource pickSound;
    void Start()
    {
        //tilemap = GetComponent<Tilemap>();
        cam = Camera.main;
        gemArray = GenerateGems();
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        selector = GameObject.FindGameObjectWithTag("Selector");
        //canvas = GameObject.FindGameObjectWithTag("Inventory");
        //canvas.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
        overworld = GameObject.FindGameObjectWithTag("Tilemap");
        overworld.SetActive(false);
        blockade = GameObject.FindGameObjectWithTag("Blockade");
        blockade.SetActive(false);
    }

    public Gem[] GenerateGems()
    {
        int x = 16;
        int y = 8;
        Gem[] spawnGems = new Gem[Random.Range(2, 7)];
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
                        int rngSmallGem = Random.Range(0, smallGemSprites.Length);
                        Tile newTile = ScriptableObject.CreateInstance<Tile>();
                        newTile.sprite = smallGemSprites[rngSmallGem];
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
        if (clicks >= max)//end game
        {
            //canvas.SetActive(true);
            foreach (var Gem in gemArray)
            {
                if (Gem.getSize() == 1)
                {
                    //points += tilemap.HasTile(Gem.getPosition()) ? 0 : 1;
                    if (!tilemap[0].HasTile(Gem.getPosition()) && !tilemap[1].HasTile(Gem.getPosition()))
                    {
                        Tile smallGem = (Tile) gemMap.GetTile(Gem.getPosition());
                        for (int i = 0; i < placeholders.Length; i++)
                        {
                            if (placeholders[i].image.Equals(smallGem.sprite))
                            {
                                inventory.AddItem(placeholders[i]);
                            }
                        }
                    }
                }
                else if (Gem.getSize() == 2)
                {
                    //points += 
                    if(!(tilemap[0].HasTile(Gem.getPosition())
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(1, 0, 0))
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(0, 1, 0))
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(1, 1, 0))
                        || tilemap[1].HasTile(Gem.getPosition())
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(1, 0, 0))
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(0, 1, 0))
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(1, 1, 0))
                        ))
                    {
                        inventory.AddItem(placeholders[1]);
                    }
                    //? 0 : 4;
                }
                else if (Gem.getSize() == 3)
                {
                    //points += 
                    if(!(tilemap[0].HasTile(Gem.getPosition())
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(0, 1, 0))
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(0, 2, 0))
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(1, 0, 0))
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(1, 1, 0))
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(1, 2, 0))
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(2, 0, 0))
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(2, 1, 0))
                        || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(2, 2, 0))
                        || tilemap[1].HasTile(Gem.getPosition())
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(0, 1, 0))
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(0, 2, 0))
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(1, 0, 0))
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(1, 1, 0))
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(1, 2, 0))
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(2, 0, 0))
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(2, 1, 0))
                        || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(2, 2, 0))
                        ))
                    {
                        inventory.AddItem(placeholders[2]);
                    }
                    //? 0 : 9;
                }
            }

            //Debug.Log(points);
            //END

            player.SetActive(true);
            overworld.SetActive(true);
            blockade.SetActive(true);
            SceneManager.LoadScene(1);
        }
    }

    public void OnMouseDown()
    {
        string toolSpriteName = selector.GetComponent<Selector>().GetItemName();
        Vector3Int tilePosition = ScreenToTilePosition(Input.mousePosition);
        switch (toolSpriteName)
        {
            case "tools_9":
                //Wood Hammer
                BaseTool(true, 2, 0, 15, 5);
                break;
            case "tools_11":
                //Copper Hammer
                BaseTool(true, 3, 0, 10, 0);
                break;
            case "tools_14":
                //Bone Hammer
                RemoveTile(tilePosition);
                BaseTool(true, 2, 0, 10, 0);
                break;
            case "tools_12":
                //Gold Hammer
                BaseTool(true, 2, 0, 30, 20);
                break;
            case "tools_10":
                //Iron Hammer
                BaseTool(true, 4, 3, 10, 0);
                break;
            case "tools_16":
                //Obsidian Hammer
                RemoveTile(tilePosition);
                RemoveTile(tilePosition + new Vector3Int(1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(-1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(0, 1, 0));
                RemoveTile(tilePosition + new Vector3Int(0, -1, 0));
                BaseTool(true, 2, 0, 10, 0);
                break;
            case "tools_17":
                //Magic Hammer
                BaseTool(true, 2, 0, 45, 35);
                break;
            case "tools_13":
                //Steel Hammer
                BaseTool(true, 5, 6, 10, 0);
                break;
            case "tools_15":
                //Diamond Hammer
                RemoveTile(tilePosition);
                RemoveTile(tilePosition + new Vector3Int(1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(-1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(0, 1, 0));
                RemoveTile(tilePosition + new Vector3Int(0, -1, 0));
                RemoveTile(tilePosition + new Vector3Int(1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(-1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(0, 1, 0));
                RemoveTile(tilePosition + new Vector3Int(0, -1, 0));
                BaseTool(true, 2, 0, 10, 0);
                break;
            case "tools_0":
                //Wood Pickaxe
                BaseTool(false, 0, 0, 5, 5);
                break;
            case "tools_2":
                //Copper Pickaxe
                BaseTool(false, 1, 0, 0, 0);
                break;
            case "tools_5":
                //Bone Pickaxe
                BaseTool(false, 0, 0, 0, 0);
                BaseTool(false, 0, 0, 0, 0);
                break;
            case "tools_3":
                //Gold Pickaxe
                BaseTool(false, 0, 0, 10, 10);
                break;
            case "tools_1":
                //Iron Pickaxe
                BaseTool(false, 2, 1, 0, 0);
                break;
            case "tools_7":
                //Obsidian Pickaxe
                RemoveTile(tilePosition);
                RemoveTile(tilePosition + new Vector3Int(1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(-1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(0, 1, 0));
                RemoveTile(tilePosition + new Vector3Int(0, -1, 0));
                BaseTool(false, 0, 0, 0, 0);
                break;
            case "tools_8":
                //Magic Pickaxe
                BaseTool(false, 0, 0, 15, 15);
                break;
            case "tools_4":
                //Steel Pickaxe
                BaseTool(false, 3, 2, 0, 0);
                break;
            case "tools_6":
                //Diamond Pickaxe
                RemoveTile(tilePosition);
                RemoveTile(tilePosition + new Vector3Int(1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(-1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(0, 1, 0));
                RemoveTile(tilePosition + new Vector3Int(0, -1, 0));
                RemoveTile(tilePosition + new Vector3Int(1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(-1, 0, 0));
                RemoveTile(tilePosition + new Vector3Int(0, 1, 0));
                RemoveTile(tilePosition + new Vector3Int(0, -1, 0));
                BaseTool(false, 0, 0, 0, 0);
                break;
        }
    }

    public void BaseTool(bool isHammer, int surroundingAreaA, int surroundingAreaB, int innerPercentage, int outerPercentage)
    {
        if (isHammer)
        {
            hammerSound.time = 0.5f;
            hammerSound.Play();
        }
        else
        {
            pickSound.time = 0.5f;
            pickSound.Play();
        }
        Vector3Int tilePosition = ScreenToTilePosition(Input.mousePosition);
        RemoveTile(tilePosition);
        Vector3Int[] surroundingTilePositions = new Vector3Int[24];
        surroundingTilePositions[0] = tilePosition + new Vector3Int(1, 0, 0);
        surroundingTilePositions[1] = tilePosition + new Vector3Int(0, 1, 0);
        surroundingTilePositions[2] = tilePosition + new Vector3Int(-1, 0, 0);
        surroundingTilePositions[3] = tilePosition + new Vector3Int(0, -1, 0);
        surroundingTilePositions[4] = tilePosition + new Vector3Int(1, 1, 0);
        surroundingTilePositions[5] = tilePosition + new Vector3Int(-1, -1, 0);
        surroundingTilePositions[6] = tilePosition + new Vector3Int(-1, 1, 0);
        surroundingTilePositions[7] = tilePosition + new Vector3Int(1, -1, 0);

        surroundingTilePositions[8] = tilePosition + new Vector3Int(2, 0, 0);
        surroundingTilePositions[9] = tilePosition + new Vector3Int(0, 2, 0);
        surroundingTilePositions[10] = tilePosition + new Vector3Int(-2, 0, 0);
        surroundingTilePositions[11] = tilePosition + new Vector3Int(0, -2, 0);
        surroundingTilePositions[12] = tilePosition + new Vector3Int(2, 2, 0);
        surroundingTilePositions[13] = tilePosition + new Vector3Int(-2, -2, 0);
        surroundingTilePositions[14] = tilePosition + new Vector3Int(-2, 2, 0);
        surroundingTilePositions[15] = tilePosition + new Vector3Int(2, -2, 0);
        surroundingTilePositions[16] = tilePosition + new Vector3Int(1, 2, 0);
        surroundingTilePositions[17] = tilePosition + new Vector3Int(2, 1, 0);
        surroundingTilePositions[18] = tilePosition + new Vector3Int(-1, 2, 0);
        surroundingTilePositions[19] = tilePosition + new Vector3Int(2, -1, 0);
        surroundingTilePositions[20] = tilePosition + new Vector3Int(-2, 1, 0);
        surroundingTilePositions[21] = tilePosition + new Vector3Int(-2, -1, 0);
        surroundingTilePositions[22] = tilePosition + new Vector3Int(1, -2, 0);
        surroundingTilePositions[23] = tilePosition + new Vector3Int(-1, -2, 0);

        List<int> previousRNG = new List<int>();
        for (int i = 0; i < surroundingAreaA; i++)
        {
            bool looking = true;
            while (looking)
            {
                int rng = Random.Range(0, 8);
                if (!previousRNG.Contains(rng))
                {
                    RemoveTile(surroundingTilePositions[rng]);
                    surroundingTilePositions[rng] = new Vector3Int(0, 0, -1);
                    previousRNG.Add(rng);
                    looking = false;
                }
            }
        }

        for (int i = 0; i < surroundingAreaB; i++)
        {
            bool looking = true;
            while (looking)
            {
                int rng = Random.Range(0, 16);
                if (!previousRNG.Contains(rng + 8))
                {
                    RemoveTile(surroundingTilePositions[rng + 8]);
                    surroundingTilePositions[rng + 8] = new Vector3Int(0, 0, -1);
                    previousRNG.Add(rng + 8);
                    looking = false;
                }
            }
        }

        for (int i = 0; i < surroundingTilePositions.Length; i++)
        {
            if (!(surroundingTilePositions[i].Equals(new Vector3Int(0, 0, -1))))
            {
                if (i < 8)
                {
                    ExtraRemoveTile(surroundingTilePositions[i], innerPercentage);
                }
                else
                {
                    ExtraRemoveTile(surroundingTilePositions[i], outerPercentage);
                }
            }
        }

        if (isHammer)
        {
            clicks += 3;
            clickCountRatio.text = (max - clicks) + " / " + max;
            healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(max - clicks, max);
        }
        else
        {
            clicks += 1;
            clickCountRatio.text = (max - clicks) + " / " + max;
            healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(max - clicks, max);
        }
        
    }

    public void ExtraRemoveTile(Vector3Int position, int chance)
    {
        if (tilemap[0].HasTile(position)
                && tilemap[1].HasTile(position))
        {
            int extra = Random.Range(1, 100);
            if (extra <= chance)
            {
                RemoveTile(position);
            }
        }
    }

    public Vector3Int ScreenToTilePosition(Vector3 clickedPosition)
    {
        Vector3 worldPosition = cam.ScreenToWorldPoint(clickedPosition);
        Vector3Int tilePosition = tilemap[0].WorldToCell(worldPosition);
        return tilePosition;
    }

    public void RemoveTile(Vector3Int tilePosition)
    {
        if (tilemap[0].HasTile(tilePosition))
        {
            tilemap[0].SetTile(tilePosition, null);
        }else if (tilemap[1].HasTile(tilePosition))
        {
            tilemap[1].SetTile(tilePosition, null);
        }
    }
}
