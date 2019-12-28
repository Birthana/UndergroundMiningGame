using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;

public class Boss_GameManager : MonoBehaviour
{
    public Sprite bossSprite;
    public int bossCurrentHealth;
    public int bossMaxHealth;
    public string bossName;
    public TextMeshProUGUI bossHealthRatio;
    public GameObject bossHealthBar;
    public TextMeshProUGUI playerHealthRatio;
    public GameObject playerHealthBar;
    public int currentPlayerHealth;
    public int rounds;
    public GameObject newMatchGrid;
    public int smallGemCount = 0;
    public int mediumGemCount = 0;
    public int largeGemCount = 0;
    public TextMeshProUGUI currentRound;
    public TextMeshProUGUI damageModifier;
    public GameObject playerAttack;
    public GameObject enemyAttack;
    public bool isAttacking;
    public AudioClip backgroundMusic;
    public ParticleSystem hitParticle;

    public int x;
    public int y;
    public Tilemap[] tilemap;
    public Tilemap gemMap;
    public Camera cam;
    public Sprite[] smallGemSprites;
    public Sprite[] mediumGemSprites;
    public Sprite[] largeGemSprites;
    public int clicks = 0;
    public int max;
    public Gem[] gemArray;
    public TextMeshProUGUI clickCountRatio;
    public GameObject healthBar;
    public Item[] smallGemItems;
    public Item[] mediumGemItems;
    public Item[] largeGemItems;
    public GameObject selector;
    public GameObject player;
    public GameObject overworld;
    public AudioSource hammerSound;
    public AudioSource pickSound;
    void Start()
    {
        cam = Camera.main;
        gemArray = GenerateGems();
        //playerHealthBar.GetComponent<MiningWallHealthBar>().SetPercentage(9f, 10f);
        selector = GameObject.FindGameObjectWithTag("Selector");
        player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
        overworld = GameObject.FindGameObjectWithTag("Tilemap");
        overworld.SetActive(false);
        damageModifier.gameObject.SetActive(false);
        playerHealthRatio.text = PlayerManager.instance.maxHealth + "/" + PlayerManager.instance.maxHealth;
        bossHealthRatio.text = bossMaxHealth + "/" + bossMaxHealth;
        playerAttack.SetActive(false);
        enemyAttack.SetActive(false);
        SoundManager.instance.PlayBackground(backgroundMusic);
    }

    public Gem[] GenerateGems()
    {
        Gem[] spawnGems = new Gem[Random.Range(8, 16)];
        for (int i = 0; i < spawnGems.Length; i++)
        {
            bool creatingGem = true;
            while (creatingGem)
            {
                Vector3Int rngPosition = new Vector3Int(Random.Range(0, x), Random.Range(0, y), 0);
                Tile rngTile = (Tile)gemMap.GetTile(rngPosition);
                int size = Random.Range(0, 10);
                if (size == 0 || size == 1 || size == 2 || size == 3 || size == 4 || size == 5 || size == 6)
                {
                    if (rngTile == null)
                    {
                        creatingGem = false;
                        int rngSmallGem = Random.Range(0, smallGemSprites.Length);
                        Tile newTile = ScriptableObject.CreateInstance<Tile>();
                        newTile.sprite = smallGemSprites[rngSmallGem];
                        gemMap.SetTile(rngPosition, newTile);
                        spawnGems[i] = new Gem(rngSmallGem, 1, rngPosition);
                    }
                }
                else if (size == 7 || size == 8)
                {
                    if (rngPosition.x != x - 1 && rngPosition.y != y - 1)
                    {
                        Tile rngTile1 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(1, 0, 0));
                        Tile rngTile2 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(0, 1, 0));
                        Tile rngTile3 = (Tile)gemMap.GetTile(rngPosition + new Vector3Int(1, 1, 0));
                        if (rngTile == null && rngTile1 == null
                            && rngTile2 == null && rngTile3 == null)
                        {
                            creatingGem = false;
                            int rngMediumGem = Random.Range(0, mediumGemSprites.Length / 4);
                            Tile bottomLeft = ScriptableObject.CreateInstance<Tile>();
                            bottomLeft.sprite = mediumGemSprites[rngMediumGem * 4];
                            Tile bottomRight = ScriptableObject.CreateInstance<Tile>();
                            bottomRight.sprite = mediumGemSprites[rngMediumGem * 4 + 1];
                            Tile topLeft = ScriptableObject.CreateInstance<Tile>();
                            topLeft.sprite = mediumGemSprites[rngMediumGem * 4 + 2];
                            Tile topRight = ScriptableObject.CreateInstance<Tile>();
                            topRight.sprite = mediumGemSprites[rngMediumGem * 4 + 3];
                            gemMap.SetTile(rngPosition, bottomLeft);
                            gemMap.SetTile(rngPosition + new Vector3Int(1, 0, 0), bottomRight);
                            gemMap.SetTile(rngPosition + new Vector3Int(0, 1, 0), topLeft);
                            gemMap.SetTile(rngPosition + new Vector3Int(1, 1, 0), topRight);
                            spawnGems[i] = new Gem(rngMediumGem, 2, rngPosition);
                        }
                    }
                }
                else if (size == 9)
                {
                    if (rngPosition.x != x - 1 && rngPosition.x != x - 2
                        && rngPosition.y != y - 1 && rngPosition.y != y - 2)
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
                            int rngLargeGem = Random.Range(0, largeGemSprites.Length / 9);
                            Tile bottomLeft = ScriptableObject.CreateInstance<Tile>();
                            bottomLeft.sprite = largeGemSprites[rngLargeGem * 9];
                            Tile bottomMiddle = ScriptableObject.CreateInstance<Tile>();
                            bottomMiddle.sprite = largeGemSprites[rngLargeGem * 9 + 1];
                            Tile bottomRight = ScriptableObject.CreateInstance<Tile>();
                            bottomRight.sprite = largeGemSprites[rngLargeGem * 9 + 2];
                            Tile centerLeft = ScriptableObject.CreateInstance<Tile>();
                            centerLeft.sprite = largeGemSprites[rngLargeGem * 9 + 3];
                            Tile centerMiddle = ScriptableObject.CreateInstance<Tile>();
                            centerMiddle.sprite = largeGemSprites[rngLargeGem * 9 + 4];
                            Tile centerRight = ScriptableObject.CreateInstance<Tile>();
                            centerRight.sprite = largeGemSprites[rngLargeGem * 9 + 5];
                            Tile topLeft = ScriptableObject.CreateInstance<Tile>();
                            topLeft.sprite = largeGemSprites[rngLargeGem * 9 + 6];
                            Tile topMiddle = ScriptableObject.CreateInstance<Tile>();
                            topMiddle.sprite = largeGemSprites[rngLargeGem * 9 + 7];
                            Tile topRight = ScriptableObject.CreateInstance<Tile>();
                            topRight.sprite = largeGemSprites[rngLargeGem * 9 + 8];
                            gemMap.SetTile(rngPosition, bottomLeft);
                            gemMap.SetTile(rngPosition + new Vector3Int(1, 0, 0), bottomMiddle);
                            gemMap.SetTile(rngPosition + new Vector3Int(0, 1, 0), centerLeft);
                            gemMap.SetTile(rngPosition + new Vector3Int(1, 1, 0), centerMiddle);
                            gemMap.SetTile(rngPosition + new Vector3Int(0, 2, 0), topLeft);
                            gemMap.SetTile(rngPosition + new Vector3Int(1, 2, 0), topMiddle);
                            gemMap.SetTile(rngPosition + new Vector3Int(2, 0, 0), bottomRight);
                            gemMap.SetTile(rngPosition + new Vector3Int(2, 1, 0), centerRight);
                            gemMap.SetTile(rngPosition + new Vector3Int(2, 2, 0), topRight);
                            spawnGems[i] = new Gem(rngLargeGem, 3, rngPosition);
                        }
                    }
                }
            }
        }
        return spawnGems;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && (max - clicks) > 0 && !isAttacking)
        {
            string toolSpriteName = selector.GetComponent<Selector>().GetItemName();
            Vector3Int tilePosition = ScreenToTilePosition(Input.mousePosition);
            if (tilemap[0].GetTile(tilePosition) != null || tilemap[1].GetTile(tilePosition) != null)
            {
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
        }

        
        if (clicks >= max)//end game
        {
            foreach (var Gem in gemArray)
            {
                if (Gem != null)
                {
                    if (Gem.getSize() == 1)
                    {
                        if (!tilemap[0].HasTile(Gem.getPosition()) && !tilemap[1].HasTile(Gem.getPosition()))
                        {
                            gemMap.SetTile(Gem.getPosition(), null);
                            smallGemCount++;
                        }
                    }
                    else if (Gem.getSize() == 2)
                    {
                        if (!(tilemap[0].HasTile(Gem.getPosition())
                            || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(1, 0, 0))
                            || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(0, 1, 0))
                            || tilemap[0].HasTile(Gem.getPosition() + new Vector3Int(1, 1, 0))
                            || tilemap[1].HasTile(Gem.getPosition())
                            || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(1, 0, 0))
                            || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(0, 1, 0))
                            || tilemap[1].HasTile(Gem.getPosition() + new Vector3Int(1, 1, 0))
                            ))
                        {
                            gemMap.SetTile(Gem.getPosition(), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(1, 0, 0), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(0, 1, 0), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(1, 1, 0), null);
                            mediumGemCount++;
                        }
                    }
                    else if (Gem.getSize() == 3)
                    {
                        if (!(tilemap[0].HasTile(Gem.getPosition())
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
                            gemMap.SetTile(Gem.getPosition(), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(0, 1, 0), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(0, 2, 0), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(1, 0, 0), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(1, 1, 0), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(1, 2, 0), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(2, 0, 0), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(2, 1, 0), null);
                            gemMap.SetTile(Gem.getPosition() + new Vector3Int(2, 2, 0), null);
                            largeGemCount++;
                        }
                    }
                }
            }
            Attacks();
        }
    }

    public void Attacks()
    {
        if (!isAttacking)
        {
            StartCoroutine(Attacking());
        }
    }

    IEnumerator Attacking()
    {
        isAttacking = true;
        
        int damage = (smallGemCount + (mediumGemCount * 6) + (largeGemCount * 13));
        bossCurrentHealth += damage;
        playerAttack.SetActive(true);
        playerAttack.GetComponent<SpriteRenderer>().sprite = PlayerManager.instance.currentHammerToolSprite;
        playerAttack.GetComponent<Animator>().SetBool("IsOpen", true);
        yield return new WaitForSeconds(0.5f);
        SoundManager.instance.PlaySound(6);
        yield return new WaitForSeconds(1.0f);
        playerAttack.GetComponent<Animator>().SetBool("IsOpen", false);
        playerAttack.SetActive(false);

        this.GetComponent<Animator>().SetBool("IsOpen", true);
        SoundManager.instance.PlaySound(3);
        hitParticle.Play();
        yield return new WaitForSeconds(0.5f);
        this.GetComponent<Animator>().SetBool("IsOpen", false);
        hitParticle.Stop();

        damageModifier.gameObject.SetActive(true);
        damageModifier.text = "" + (int.Parse(damageModifier.text) + damage);
        bossHealthRatio.text = (bossMaxHealth - bossCurrentHealth) + "/" + bossMaxHealth;
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            bossHealthBar.GetComponent<MiningWallHealthBar>().SetPercentage(bossMaxHealth - bossCurrentHealth, bossMaxHealth);
        }
        else
        {
            bossHealthBar.GetComponent<MiningWallHealthBar>().SetPercentage(0, bossMaxHealth);
        }

        yield return new WaitForSeconds(0.1f);

        rounds++;
        currentRound.text = "Round " + (rounds + 1);
        if (rounds < 3)
        {
            RoundReset();
        }
        else
        {
            MatchReset();
        }

        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            currentPlayerHealth += 10;
            enemyAttack.SetActive(true);
            SoundManager.instance.PlaySound(1);
            yield return new WaitForSeconds(0.5f);
            enemyAttack.GetComponent<Animator>().SetBool("IsOpen", true);
            yield return new WaitForSeconds(1.0f);
            enemyAttack.GetComponent<Animator>().SetBool("IsOpen", false);
            enemyAttack.SetActive(false);
            playerHealthRatio.text = (PlayerManager.instance.maxHealth - currentPlayerHealth) + "/" + PlayerManager.instance.maxHealth;
            playerHealthBar.GetComponent<MiningWallHealthBar>().SetPercentage(PlayerManager.instance.maxHealth - currentPlayerHealth, PlayerManager.instance.maxHealth);
        }
        else
        {
            player.SetActive(true);
            overworld.SetActive(true);
            ZoneManager.instance.UnlockZone();
            SoundManager.instance.backgroundPlayer.Stop();
            SoundManager.instance.PlaySound(4);
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(1);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            player.SetActive(true);
            overworld.SetActive(true);
            SoundManager.instance.backgroundPlayer.Stop();
            SoundManager.instance.PlaySound(5);
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(1);
        }
        isAttacking = false;
    }

    public void RoundReset()
    {
        clicks = 0;
        healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(max, max);
        clickCountRatio.text = max + "/" + max;
        smallGemCount = 0;
        mediumGemCount = 0;
        largeGemCount = 0;
    }

    public void MatchReset()
    {
        RoundReset();
        rounds = 0;
        currentRound.text = "Round " + (rounds + 1);
        damageModifier.gameObject.SetActive(false);
        damageModifier.text = "0";
        Destroy(tilemap[0].transform.parent.gameObject);
        Vector3 gridposition = new Vector3(0, 0.5f, 0);
        GameObject newGrid = Instantiate(newMatchGrid, gridposition, Quaternion.identity);
        tilemap[0] = newGrid.transform.GetChild(3).gameObject.GetComponent<Tilemap>();
        tilemap[1] = newGrid.transform.GetChild(2).gameObject.GetComponent<Tilemap>();
        gemMap = newGrid.transform.GetChild(1).gameObject.GetComponent<Tilemap>();
        gemArray = GenerateGems();
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
            if ((max - clicks) > 0)
            {
                clickCountRatio.text = (max - clicks) + " / " + max;
                healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(max - clicks, max);
            }
            else
            {
                clickCountRatio.text = 0 + " / " + max;
                healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(0, max);
            }
            
        }
        else
        {
            clicks += 1;
            if ((max - clicks) > 0)
            {
                clickCountRatio.text = (max - clicks) + " / " + max;
                healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(max - clicks, max);
            }
            else
            {
                clickCountRatio.text = 0 + " / " + max;
                healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(0, max);
            }
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
        }
        else if (tilemap[1].HasTile(tilePosition))
        {
            tilemap[1].SetTile(tilePosition, null);
        }
    }
}
