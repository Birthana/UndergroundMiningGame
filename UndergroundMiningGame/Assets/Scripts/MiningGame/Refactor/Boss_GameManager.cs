using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;

public class Boss_GameManager : MonoBehaviour
{
    //Need seperate class to display boss ui.
    //Boss_GameManager should only manage the game, nothing else.
    
    public Boss bossInfo;
    public SpriteRenderer bossSprite;
    public int bossCurrentHealth;
    public int bossMaxHealth;
    public TextMeshProUGUI bossName;
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
    public bool isAttacking;
    public AudioClip backgroundMusic;
    public GameObject damageShield;
    public GameObject attackName;

    public GameObject[] enemyAttack;
    public ParticleSystem[] particles;
    public GameObject damagePrefab;
    public GameObject explosionPrefab;
    public Sprite[] baseAttackSprites;
    public Sprite[] hazardSprite;
    public GameObject[] minions;
    public Sprite[] minionSprites;
    public int minionCount;
    public int maxHazardCount;
    public bool gameEnding;

    public int x;
    public int y;
    public Tilemap[] tilemap;
    public Tilemap gemMap;
    public Tilemap hazards;
    public Camera cam;
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
    public List<Item> bossLoot;
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
        bossInfo = PlayerManager.instance.bossToFight;
        bossSprite = this.GetComponent<SpriteRenderer>();
        bossSprite.sprite = bossInfo.bossSprite;
        if (bossSprite.sprite.name.Equals("cactus_merchant"))
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        bossMaxHealth = bossInfo.bossMaxHealth;
        bossName.text = bossInfo.bossName;
        bossHealthRatio.text = bossInfo.bossMaxHealth + "/" + bossInfo.bossMaxHealth;
        playerAttack.SetActive(false);
        foreach (GameObject attack in enemyAttack)
        {
            attack.SetActive(false);
        }
        SoundManager.instance.PlayBackground(backgroundMusic, true);
        damageShield.SetActive(false);
        attackName.SetActive(false);
        bossLoot = new List<Item>();
        foreach (GameObject minion in minions)
        {
            minion.SetActive(false);
        }
        maxHazardCount = 0;
    }

    public Gem[] GenerateGems()
    {
        return GetComponent<GemSpawner>().SpawnGems();
    }

    // Update is called once per frame
    void Update()
    {
        if (selector != null)
        {
            selector = GameObject.FindGameObjectWithTag("Selector");
        }

        if (!gameEnding)
        {
            CheckHazardsTooltip();
        }

        if (Input.GetMouseButtonDown(0) && (max - clicks) > 0 && !isAttacking)
        {
            Vector3Int tilePosition = ScreenToTilePosition(Input.mousePosition);
            Tile hazardTile = (Tile)hazards.GetTile(tilePosition);
            if (hazardTile == null)
            {
                MineTopTile();
            }
            else if(hazardTile.sprite.Equals(hazardSprite[0]) || hazardTile.sprite.Equals(hazardSprite[6]))
            {
                SoundManager.instance.PlaySound(8);
            }else if (hazardTile.sprite.Equals(hazardSprite[1]) || hazardTile.sprite.Equals(hazardSprite[2]) || hazardTile.sprite.Equals(hazardSprite[4]))
            {
                maxHazardCount--;
                hazards.SetTile(tilePosition, null);
                currentPlayerHealth += 5;
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
                {
                    StartCoroutine(EndBossFight(false, 1));
                }
                MineTopTile();
            }
            else if (hazardTile.sprite.Equals(hazardSprite[3]) || hazardTile.sprite.Equals(hazardSprite[5]) || 
                hazardTile.sprite.Equals(hazardSprite[7]) || hazardTile.sprite.Equals(hazardSprite[8]))
            {
                MineTopTile();
            }
        }

        
        if (clicks >= max && !isAttacking)//end game
        {
            damageModifier.gameObject.SetActive(true);
            bool tenMod = false;
            foreach (var Gem in gemArray)
            {
                if (Gem != null)
                {
                    if (Gem.GetSize() == Gem.Size.SMALL)
                    {
                        if (!tilemap[0].HasTile(Gem.getPosition()) && !tilemap[1].HasTile(Gem.getPosition()))
                        {
                            gemMap.SetTile(Gem.getPosition(), null);
                            smallGemCount++;
                            GameObject temp = Instantiate(damagePrefab, gemMap.CellToWorld(Gem.getPosition()) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                            int rng = Random.Range(0, 20);
                            if (rng == 0 && !tenMod)
                            {
                                tenMod = true;
                                temp.GetComponent<TextMeshPro>().text = "5";
                                temp.GetComponent<TextMeshPro>().color = new Color(255, 0, 0, 255);
                            }
                            else
                            {
                                temp.GetComponent<TextMeshPro>().text = "1";
                            }
                            bossLoot.Add(smallGemItems[Gem.getGemPosition()]);
                            MinedGemsUI.instance.AddMinedItem(smallGemItems[Gem.getGemPosition()]);
                        }
                    }
                    else if (Gem.GetSize() == Gem.Size.MEDIUM)
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
                            GameObject temp = Instantiate(damagePrefab, gemMap.CellToWorld(Gem.getPosition()) + new Vector3(1, 1, 0), Quaternion.identity);
                            temp.GetComponent<TextMeshPro>().text = "6";
                            bossLoot.Add(mediumGemItems[Gem.getGemPosition()]);
                            MinedGemsUI.instance.AddMinedItem(mediumGemItems[Gem.getGemPosition()]);
                        }
                    }
                    else if (Gem.GetSize() == Gem.Size.LARGE)
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
                            GameObject temp = Instantiate(damagePrefab, gemMap.CellToWorld(Gem.getPosition()) + new Vector3(1.5f, 1.5f, 0), Quaternion.identity);
                            temp.GetComponent<TextMeshPro>().text = "13";
                            bossLoot.Add(largeGemItems[Gem.getGemPosition()]);
                            MinedGemsUI.instance.AddMinedItem(largeGemItems[Gem.getGemPosition()]);
                        }
                    }
                }
            }
            Attacks();
        }
    }

    public void CheckHazardsTooltip()
    {
        Vector3Int tilePosition = ScreenToTilePosition(Input.mousePosition);
        if (hazards.HasTile(tilePosition))
        {
            Tile hazardTile = (Tile)hazards.GetTile(tilePosition);
            Sprite hoveringHazardSprite = hazardTile.sprite;
            if (hoveringHazardSprite.Equals(hazardSprite[0]))
            {
                HazardsTooltipManager.instance.Appear(hazards.CellToWorld(tilePosition), hoveringHazardSprite, "Cannot be mined.");
            }
            else if (hoveringHazardSprite.Equals(hazardSprite[1]))
            {
                HazardsTooltipManager.instance.Appear(hazards.CellToWorld(tilePosition), hoveringHazardSprite, "Deals 5HP to mine.");
            }
            else if (hoveringHazardSprite.Equals(hazardSprite[2]))
            {
                HazardsTooltipManager.instance.Appear(hazards.CellToWorld(tilePosition), hoveringHazardSprite, "Deals 5HP to mine.");
            }
            else if (hoveringHazardSprite.Equals(hazardSprite[3]))
            {
                HazardsTooltipManager.instance.Appear(hazards.CellToWorld(tilePosition), hoveringHazardSprite, "Extra tile to mine.");
            }
            else if (hoveringHazardSprite.Equals(hazardSprite[4]))
            {
                HazardsTooltipManager.instance.Appear(hazards.CellToWorld(tilePosition), hoveringHazardSprite, "Deals 5HP to mine.");
            }
            else if (hoveringHazardSprite.Equals(hazardSprite[5]))
            {
                HazardsTooltipManager.instance.Appear(hazards.CellToWorld(tilePosition), hoveringHazardSprite, "Extra tile to mine.");
            }
            else if (hoveringHazardSprite.Equals(hazardSprite[6]))
            {
                HazardsTooltipManager.instance.Appear(hazards.CellToWorld(tilePosition), hoveringHazardSprite, "Cannot be mined.");
            }
            else if (hoveringHazardSprite.Equals(hazardSprite[7]))
            {
                HazardsTooltipManager.instance.Appear(hazards.CellToWorld(tilePosition), hoveringHazardSprite, "Tile that moves when mined.");
            }
            else if (hoveringHazardSprite.Equals(hazardSprite[8]))
            {
                HazardsTooltipManager.instance.Appear(hazards.CellToWorld(tilePosition), hoveringHazardSprite, "Extra tile to mine.");
            }
        }
        else
        {
            HazardsTooltipManager.instance.Disappear();
        }
    }

    public void MineTopTile()
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
                    RemoveTile(tilePosition);
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

            foreach (GameObject minion in minions)
            {
                if (minion.activeInHierarchy)
                {
                    int particleAttack = 0;
                    minion.GetComponentInChildren<TextMeshPro>().text = "" + (int.Parse(minion.GetComponentInChildren<TextMeshPro>().text) - 1);
                    if ((int.Parse(minion.GetComponentInChildren<TextMeshPro>().text) <= 0))
                    {
                        if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("miner_ghost"))
                        {
                            particleAttack = 16;
                            ChangeHazard(hazardSprite[2], hazardSprite[0]);
                        }
                        else if(minion.GetComponent<SpriteRenderer>().sprite.name.Equals("mushroomnpcs_0"))
                        {
                            particleAttack = 20;
                            SpawnHazards(7, 5);
                        }
                        else if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("mushroomnpcs_1"))
                        {
                            particleAttack = 20;
                            ChangeHazard(hazardSprite[7], hazardSprite[4]);
                        }
                        else if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("mushroomnpcs_2"))
                        {
                            particleAttack = 3;
                            if (bossCurrentHealth < bossMaxHealth)
                            {
                                bossCurrentHealth -= 5;
                                bossHealthRatio.text = (bossMaxHealth - bossCurrentHealth) + "/" + bossMaxHealth;
                                bossHealthBar.GetComponent<MiningWallHealthBar>().SetPercentage(bossMaxHealth - bossCurrentHealth, bossMaxHealth, true);
                            }
                        }
                        else if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("icetopus_minions_0"))
                        {
                            particleAttack = 23;
                            SpawnHazards(4, 5);
                        }
                        else if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("icetopus_minions_1"))
                        {
                            particleAttack = 23;
                            ChangeHazard(hazardSprite[4], hazardSprite[8]);
                        }
                        else if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("panda_minions_0"))
                        {
                            particleAttack = 26;
                            ChangeHazard(hazardSprite[8], hazardSprite[0]);
                        }
                        else if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("panda_minions_1"))
                        {
                            particleAttack = 26;
                            SpawnHazards(0, 5);
                        }
                        else if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("mephobiousminions_0"))
                        {
                            particleAttack = 29;
                            int rngHazardChange = Random.Range(0, 6);
                            if (rngHazardChange == 0)
                            {
                                ChangeHazard(hazardSprite[0], hazardSprite[3]);
                            }
                            else if (rngHazardChange == 1)
                            {
                                ChangeHazard(hazardSprite[1], hazardSprite[6]);
                            }
                            else if (rngHazardChange == 2)
                            {
                                ChangeHazard(hazardSprite[2], hazardSprite[0]);
                            }
                            else if (rngHazardChange == 3)
                            {
                                ChangeHazard(hazardSprite[3], hazardSprite[1]);
                            }
                            else if (rngHazardChange == 4)
                            {
                                ChangeHazard(hazardSprite[5], hazardSprite[2]);
                            }
                            else if (rngHazardChange == 5)
                            {
                                ChangeHazard(hazardSprite[6], hazardSprite[5]);
                            }
                        }
                        else if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("mephobiousminions_1"))
                        {
                            particleAttack = 29;
                            SpawnHazards(0, 1);
                            SpawnHazards(1, 1);
                            SpawnHazards(2, 1);
                            SpawnHazards(3, 1);
                            SpawnHazards(5, 1);
                            SpawnHazards(6, 1);
                        }
                        else if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("cactus_minions_0"))
                        {
                            particleAttack = 33;
                            SpawnHazards(0, 5);
                        }
                        else if (minion.GetComponent<SpriteRenderer>().sprite.name.Equals("cactus_minions_1"))
                        {
                            particleAttack = 33;
                            ChangeHazard(hazardSprite[0], hazardSprite[2]);
                        }
                        minion.GetComponentInChildren<TextMeshPro>().text = "10";
                        StartCoroutine(MinionAttack(minion.transform.position, particleAttack));
                        currentPlayerHealth += 15;
                        PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
                        {
                            StartCoroutine(EndBossFight(false, 1));
                        }
                    }
                }
            }
        }
    }

    public void ChangeHazard(Sprite oldHazard, Sprite newHazard)
    {
        foreach (var position in hazards.cellBounds.allPositionsWithin)
        {
            if (hazards.HasTile(position))
            {
                Tile tempTile = (Tile)hazards.GetTile(position);
                if (tempTile.sprite.Equals(oldHazard))
                {
                    int rng = Random.Range(0, 4);
                    if (rng == 0)
                    {
                        Tile newTile = ScriptableObject.CreateInstance<Tile>();
                        newTile.sprite = newHazard;
                        hazards.SetTile(position, newTile);
                    }
                }
            }
        }
    }

    IEnumerator MinionAttack(Vector3 position, int particleIndex)
    {
        particles[particleIndex].transform.position = position;
        particles[particleIndex].Play();
        yield return new WaitForSeconds(1.0f);
        particles[particleIndex].Stop();
    }

    public void Attacks()
    {
        if (!isAttacking)
        {
            if (bossInfo.bossSprite.name.Equals("npc-1_0")) 
            {
                StartCoroutine(ChooChooAttacking());
            }else if (bossInfo.bossSprite.name.Equals("coal 1"))
            {
                StartCoroutine(MagnaAttacking());
            }else if (bossInfo.bossSprite.name.Equals("merchants_1"))
            {
                StartCoroutine(YanconAttacking());
            }else if (bossInfo.bossSprite.name.Equals("merchants_4"))
            {
                StartCoroutine(GemoolAttacking());
            }else if (bossInfo.bossSprite.name.Equals("lavamask"))
            {
                StartCoroutine(LavamaskAttacking());
            }else if (bossInfo.bossSprite.name.Equals("merchants_5"))
            {
                StartCoroutine(AquariusAttacking());
            }else if (bossInfo.bossSprite.name.Equals("merchants_3"))
            {
                StartCoroutine(LockycAttacking());
            }else if (bossInfo.bossSprite.name.Equals("ghostleader"))
            {
                StartCoroutine(RowtanyonAttacking());
            }
            else if (bossInfo.bossSprite.name.Equals("mushroomnpcs_3"))
            {
                StartCoroutine(FungalmindAttacking());
            }
            else if (bossInfo.bossSprite.name.Equals("icetopus"))
            {
                StartCoroutine(IcetopusAttacking());
            }
            else if (bossInfo.bossSprite.name.Equals("merchants_2"))
            {
                StartCoroutine(PanpassAttacking());
            }else if (bossInfo.bossSprite.name.Equals("mephobiousnpc"))
            {
                StartCoroutine(MephobiousAttacking());
            }
            else if (bossInfo.bossSprite.name.Equals("cactus_merchant"))
            {
                StartCoroutine(ErictusAttacking());
            }
        }
    }

    IEnumerator ChooChooAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 3);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("Chucking Yellow Gems", -1));
                yield return new WaitForSeconds(1.5f);
            }
            else if(rng == 1 || rng == 2)
            {
                currentPlayerHealth += (bossInfo.bossDamage/2);
                SetAttackName("Protective Bubble Spell");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[1].SetActive(true);
                enemyAttack[1].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                enemyAttack[1].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[1].SetActive(false);
                particles[1].Play();
                yield return new WaitForSeconds(1.0f);
                particles[1].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                SpawnHazards(0, 13);
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 1));
            yield return new WaitForSeconds(2.0f);
        }
        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 1));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1.0f);
        bossCurrentHealth += int.Parse(damageModifier.text);
        playerAttack.SetActive(true);
        playerAttack.GetComponent<SpriteRenderer>().sprite = PlayerManager.instance.currentHammerToolSprite;
        playerAttack.GetComponent<Animator>().SetBool("IsOpen", true);
        yield return new WaitForSeconds(0.5f);
        SoundManager.instance.PlaySound(6);
        yield return new WaitForSeconds(1.0f);
        playerAttack.GetComponent<Animator>().SetBool("IsOpen", false);
        playerAttack.SetActive(false);

        if (int.Parse(damageModifier.text) > 0)
        {
            this.GetComponent<Animator>().SetBool("IsOpen", true);
            SoundManager.instance.PlaySound(3);
            particles[0].Play();
            yield return new WaitForSeconds(1.0f);
            this.GetComponent<Animator>().SetBool("IsOpen", false);
            particles[0].Stop();

            bossHealthRatio.text = (bossMaxHealth - bossCurrentHealth) + "/" + bossMaxHealth;
            if ((bossMaxHealth - bossCurrentHealth) > 0)
            {
                bossHealthBar.GetComponent<MiningWallHealthBar>().SetPercentage(bossMaxHealth - bossCurrentHealth, bossMaxHealth, true);
            }
            else
            {
                bossHealthBar.GetComponent<MiningWallHealthBar>().SetPercentage(0, bossMaxHealth, true);
            }
        }
        else
        {
            SoundManager.instance.PlaySoundAt(7, 0.5f);
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(0.5f);
    }

    public void RoundCheck()
    {
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
    }

    public void SetAttackName(string currentAttackName)
    {
        attackName.SetActive(true);
        attackName.GetComponentInChildren<TextMeshProUGUI>().text = currentAttackName;
    }

    IEnumerator EndBossFight(bool gameWin, int numberOfBlockade)
    {
        gameEnding = true;
        HazardsTooltipManager.instance = null;
        Destroy(GameObject.FindGameObjectWithTag("HazardTooltipManager"));
        SoundManager.instance.backgroundPlayer.Stop();
        MinedGemsUI.instance.Display();
        yield return new WaitForSeconds(((0.2f * MinedGemsUI.instance.itemsMined.Count) + 1.3f));
        if (gameWin)
        {
            ZoneManager.instance.UnlockZone(numberOfBlockade);
            SoundManager.instance.PlayInBackground(4);
        }
        else
        {
            SoundManager.instance.PlayInBackground(5);
        }
        isAttacking = true;
        foreach (Item temp in bossLoot)
        {
            Inventory.instance.AddItem(temp);
        }
        yield return new WaitForSeconds(1.0f);
        TransitionsManager.instance.Open();
        yield return new WaitForSeconds(1.0f);
        player.SetActive(true);
        overworld.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void SpawnHazards(int index, int numberOfHazards)
    {
        for (int i = 0; i < numberOfHazards; i++)
        {
            if (maxHazardCount < (x * y))
            {
                bool still_looking = true;
                while (still_looking)
                {
                    Vector3Int rngPosition = new Vector3Int(Random.Range(0, x), Random.Range(0, y), 0);
                    if (!hazards.HasTile(rngPosition))
                    {
                        Tile rngTile = ScriptableObject.CreateInstance<Tile>();
                        rngTile.sprite = hazardSprite[index];
                        hazards.SetTile(rngPosition, rngTile);
                        still_looking = false;
                        maxHazardCount++;
                    }
                }
            }
        }
    }

    public void PlayerTakeDamage(float newCurrentHealth)
    {
        playerHealthRatio.text = newCurrentHealth + "/" + PlayerManager.instance.maxHealth;
        playerHealthBar.GetComponent<MiningWallHealthBar>().SetPercentage(newCurrentHealth, PlayerManager.instance.maxHealth, true);
    }

    IEnumerator EnemyBaseAttack(string currentAttackName, int index)
    {
        currentPlayerHealth += bossInfo.bossDamage;
        SetAttackName(currentAttackName);
        SoundManager.instance.PlaySound(1); 
        yield return new WaitForSeconds(0.5f);
        attackName.SetActive(false);
        enemyAttack[0].SetActive(true);
        if (index >= 0)
        {
            enemyAttack[0].GetComponent<SpriteRenderer>().sprite = baseAttackSprites[index];
        }
        enemyAttack[0].GetComponent<Animator>().SetBool("IsOpen", true);
        yield return new WaitForSeconds(1.0f);
        enemyAttack[0].GetComponent<Animator>().SetBool("IsOpen", false);
        enemyAttack[0].SetActive(false);
        PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
    }

    IEnumerator MagnaAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 5);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("Chucking Red Gems", 0));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2 || rng == 3 || rng == 4)
            {
                if (rng == 1 && (bossCurrentHealth > (bossMaxHealth / 2)))
                {
                    SetAttackName("Chewing On Rocks"); 
                    yield return new WaitForSeconds(0.5f);
                    attackName.SetActive(false);
                    enemyAttack[3].SetActive(true);
                    enemyAttack[3].GetComponent<Animator>().SetBool("IsOpen", true);
                    particles[3].Play();
                    yield return new WaitForSeconds(1.0f);
                    attackName.SetActive(false);
                    enemyAttack[3].GetComponent<Animator>().SetBool("IsOpen", false);
                    enemyAttack[3].SetActive(false);
                    particles[3].Stop();
                    bossCurrentHealth -= 15;
                    bossHealthRatio.text = (bossMaxHealth - bossCurrentHealth) + "/" + bossMaxHealth;
                    bossHealthBar.GetComponent<MiningWallHealthBar>().SetPercentage(bossMaxHealth - bossCurrentHealth, bossMaxHealth, true);
                }
                else
                {
                    currentPlayerHealth += (bossInfo.bossDamage / 2);
                    SetAttackName("Explosive Ember Boulders");
                    SoundManager.instance.PlaySound(8);
                    yield return new WaitForSeconds(0.5f);
                    attackName.SetActive(false);
                    enemyAttack[2].SetActive(true);
                    enemyAttack[2].GetComponent<Animator>().SetBool("IsOpen", true);
                    yield return new WaitForSeconds(1.0f);
                    enemyAttack[2].GetComponent<Animator>().SetBool("IsOpen", false);
                    enemyAttack[2].SetActive(false);
                    particles[2].Play();
                    SoundManager.instance.PlaySound(9);
                    yield return new WaitForSeconds(1.0f);
                    particles[2].Stop();
                    PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                    SpawnHazards(1, 20);
                }
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 2));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 2));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator YanconAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("Throwing Orange Gems", 1));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2 || rng == 3)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 3);
                SetAttackName("Calling The Flock");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[5].SetActive(true);
                enemyAttack[5].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(0.3f);
                SoundManager.instance.PlaySound(7);
                yield return new WaitForSeconds(1.0f);
                enemyAttack[5].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[5].SetActive(false);
                SoundManager.instance.PlaySound(10);
                yield return new WaitForSeconds(0.5f);
                particles[5].Play();
                yield return new WaitForSeconds(4.0f);
                particles[5].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                SpawnHazards(2, 15);
            }
            else if (rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 3);
                SetAttackName("Conjure Enchanted Arrows");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[4].SetActive(true);
                enemyAttack[4].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                enemyAttack[4].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[4].SetActive(false);
                particles[4].Play();
                SoundManager.instance.PlaySound(11);
                yield return new WaitForSeconds(3.0f);
                particles[4].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                SpawnHazards(0, 15);
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 3));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 3));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator GemoolAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("Releasing Blue Crystals", 2));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 3);
                SetAttackName("Blinding Drone Phazers");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[6].SetActive(true);
                enemyAttack[6].GetComponent<Animator>().SetBool("IsOpen", true);
                particles[6].Play();
                SoundManager.instance.PlaySound(11);
                yield return new WaitForSeconds(3.0f);
                enemyAttack[6].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[6].SetActive(false);
                particles[6].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (damageShield.activeInHierarchy)
                {
                    SpawnHazards(3, 5);
                }
                else
                {
                    damageShield.SetActive(true);
                    damageShield.GetComponent<DamageShield>().SetProtectValue(10);
                }
            }
            else if (rng == 3 || rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 3);
                SetAttackName("Causing An Avalanche");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[7].SetActive(true);
                enemyAttack[7].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                enemyAttack[7].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[7].SetActive(false);
                particles[7].Play();
                yield return new WaitForSeconds(1.0f);
                SoundManager.instance.PlaySound(9);
                yield return new WaitForSeconds(2.0f);
                particles[7].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                SpawnHazards(3, 10);
                SpawnHazards(4, 10);
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 5));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 5));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator LavamaskAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("Leave Me Alone", 3));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2 || rng == 3)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 3);
                SetAttackName("Heavy Smoke Signal");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[8].SetActive(true);
                enemyAttack[8].GetComponent<Animator>().SetBool("IsOpen", true);
                //SoundManager.instance.PlaySound(11);
                yield return new WaitForSeconds(1.0f);
                particles[8].Play();
                yield return new WaitForSeconds(3.0f);
                enemyAttack[8].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[8].SetActive(false);
                particles[8].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (damageShield.activeInHierarchy)
                {
                    SpawnHazards(5, 5);
                }
                else
                {
                    damageShield.SetActive(true);
                    damageShield.GetComponent<DamageShield>().SetProtectValue(10);
                }
                SpawnHazards(5, 20);
            }
            else if (rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 3);
                SetAttackName("Too Much Smoke Raindance");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[9].SetActive(true);
                enemyAttack[9].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                enemyAttack[9].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[9].SetActive(false);
                particles[9].Play();
                yield return new WaitForSeconds(1.0f);
                //SoundManager.instance.PlaySound(9);
                yield return new WaitForSeconds(2.0f);
                particles[9].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                SpawnHazards(6, 10);
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 8));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 8));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator AquariusAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("No Real Rocks Here", 3));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2 || rng == 3)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 3);
                SetAttackName("Tossing Out Dirty Fish Water");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[10].SetActive(true);
                enemyAttack[10].GetComponent<Animator>().SetBool("IsOpen", true);
                //SoundManager.instance.PlaySound(11);
                yield return new WaitForSeconds(1.0f);
                particles[10].Play();
                yield return new WaitForSeconds(2.0f);
                enemyAttack[10].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[10].SetActive(false);
                particles[10].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (damageShield.activeInHierarchy)
                {
                    SpawnHazards(6, 15);
                }
                else
                {
                    damageShield.SetActive(true);
                    damageShield.GetComponent<DamageShield>().SetProtectValue(10);
                }
                SpawnHazards(6, 7);
            }
            else if (rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 3);
                SetAttackName("Fireball Phazers");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[11].SetActive(true);
                enemyAttack[11].GetComponent<Animator>().SetBool("IsOpen", true);
                particles[11].Play();
                yield return new WaitForSeconds(3.0f);
                enemyAttack[11].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[11].SetActive(false);
                particles[11].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                SpawnHazards(5, 25);
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 7));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 7));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator LockycAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("Back Off", 1));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 4);
                SetAttackName("Eye Beams");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[12].SetActive(true);
                enemyAttack[12].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(0.5f);
                particles[12].Play();
                yield return new WaitForSeconds(3.0f);
                enemyAttack[12].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[12].SetActive(false);
                particles[12].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (damageShield.activeInHierarchy)
                {
                    ExplodeHazards(1);
                    ExplodeHazards(2);
                }
                else
                {
                    ExplodeHazards(1);
                    ExplodeHazards(2);
                    damageShield.SetActive(true);
                    damageShield.GetComponent<DamageShield>().SetProtectValue(10);
                }
            }
            else if (rng == 3 || rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 3);
                SetAttackName("Wild Punch");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[13].SetActive(true);
                int rngRock = Random.Range(0, 2);
                if (rngRock == 0)
                {
                    enemyAttack[13].GetComponent<SpriteRenderer>().sprite = baseAttackSprites[4];
                }
                else if (rngRock == 1)
                {
                    enemyAttack[13].GetComponent<SpriteRenderer>().sprite = baseAttackSprites[5];
                }
                enemyAttack[13].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                particles[15].Play();
                yield return new WaitForSeconds(1.0f);
                enemyAttack[13].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[13].SetActive(false);
                if (rngRock == 0)
                {
                    particles[13].Play();
                }
                else
                {
                    particles[14].Play();
                }
                yield return new WaitForSeconds(2.0f);
                particles[13].Stop();
                particles[14].Stop();
                particles[15].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (rngRock == 0)
                {
                    ExplodeHazards(1);
                    SpawnHazards(1, 5);
                }
                else
                {
                    ExplodeHazards(2);
                    SpawnHazards(2, 5);
                }
                
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 4));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 4));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator RowtanyonAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("Back Off", -1));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2 || rng == 3)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 4);
                SetAttackName("Out of the Mines");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[15].SetActive(true);
                enemyAttack[15].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.5f);
                enemyAttack[15].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[15].SetActive(false);
                particles[14].Play();
                yield return new WaitForSeconds(3.0f);
                particles[14].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (minionCount < minions.Length) 
                {
                    SpawnMinion(minionSprites[0], 10);
                    SpawnHazards(2, 10);
                    if (damageShield.activeInHierarchy)
                    {

                    }
                    else
                    {
                        damageShield.SetActive(true);
                        damageShield.GetComponent<DamageShield>().SetProtectValue(10);
                    }
                }
                else
                {
                    SpawnHazards(2, 20);
                }
            }
            else if (rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 3);
                SetAttackName("Mysterious Locket");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[14].SetActive(true);
                enemyAttack[14].GetComponent<Animator>().SetBool("IsOpen", true);
                particles[17].Play();
                yield return new WaitForSeconds(3.0f);
                enemyAttack[14].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[14].SetActive(false);
                particles[17].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                SpawnHazards(2, 10);
                SpawnHazards(0, 10);
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 13));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 13));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator FungalmindAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("We Will Fight", 6));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2 || rng == 3)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 5);
                SetAttackName("Spewing Spores");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[16].SetActive(true);
                enemyAttack[16].GetComponent<Animator>().SetBool("IsOpen", true);
                particles[18].Play();
                yield return new WaitForSeconds(1.5f);
                enemyAttack[16].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[16].SetActive(false);
                yield return new WaitForSeconds(3.0f);
                particles[18].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (minionCount < minions.Length)
                {
                    SpawnMinion(minionSprites[Random.Range(0, 3) + 1], 10);
                    if (damageShield.activeInHierarchy)
                    {
                        SpawnHazards(7, 15);
                    }
                    else
                    {
                        SpawnHazards(7, 10);
                        damageShield.SetActive(true);
                        damageShield.GetComponent<DamageShield>().SetProtectValue(10);
                    }
                }
                else
                {
                    SpawnHazards(7, 15);
                }
            }
            else if (rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 4);
                SetAttackName("Cold Adaptation");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[17].SetActive(true);
                enemyAttack[17].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                enemyAttack[17].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[17].SetActive(false);
                particles[19].Play();
                yield return new WaitForSeconds(3.0f);
                particles[19].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                SpawnHazards(4, 10);
                SpawnHazards(7, 5);
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 11));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 11));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator IcetopusAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("Tentacle Attack", 2));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2 || rng == 3)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 5);
                SetAttackName("Magic Icicles");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[18].SetActive(true);
                enemyAttack[18].GetComponent<Animator>().SetBool("IsOpen", true);
                particles[21].Play();
                yield return new WaitForSeconds(1.5f);
                enemyAttack[18].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[18].SetActive(false);
                yield return new WaitForSeconds(3.0f);
                particles[21].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (minionCount < minions.Length)
                {
                    SpawnMinion(minionSprites[Random.Range(0, 2) + 4], 10);
                    if (damageShield.activeInHierarchy)
                    {
                        SpawnHazards(4, 15);
                    }
                    else
                    {
                        SpawnHazards(4, 10);
                        damageShield.SetActive(true);
                        damageShield.GetComponent<DamageShield>().SetProtectValue(10);
                    }
                }
                else
                {
                    SpawnHazards(4, 15);
                }
            }
            else if (rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 4);
                SetAttackName("Quick Melting Chuck");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[19].SetActive(true);
                enemyAttack[19].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                enemyAttack[19].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[19].SetActive(false);
                particles[22].Play();
                yield return new WaitForSeconds(3.0f);
                particles[22].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                SpawnHazards(8, 20);
                SpawnHazards(6, 10);
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 6));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 6));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator PanpassAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("Bear Chuck", 7));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2 || rng == 3)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 5);
                SetAttackName("Quick Grow Wall");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[20].SetActive(true);
                enemyAttack[20].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                enemyAttack[20].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[20].SetActive(false);
                particles[24].Play();
                yield return new WaitForSeconds(3.0f);
                particles[24].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (minionCount < minions.Length)
                {
                    SpawnMinion(minionSprites[Random.Range(0, 2) + 6], 10);
                    if (damageShield.activeInHierarchy)
                    {
                        SpawnHazards(8, 25);
                    }
                    else
                    {
                        SpawnHazards(8, 15);
                        damageShield.SetActive(true);
                        damageShield.GetComponent<DamageShield>().SetProtectValue(10);
                    }
                }
                else
                {
                    SpawnHazards(8, 25);
                }
            }
            else if (rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 4);
                SetAttackName("Watch The Magic Flowers");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[21].SetActive(true);
                enemyAttack[21].GetComponent<Animator>().SetBool("IsOpen", true);
                particles[25].Play();
                yield return new WaitForSeconds(1.0f);
                enemyAttack[21].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[21].SetActive(false);
                yield return new WaitForSeconds(3.0f);
                particles[25].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                SpawnHazards(0, 15);
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 9));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 9));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator MephobiousAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("Showtime", 8));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2 || rng == 3)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 5);
                SetAttackName("Next Trick");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[22].SetActive(true);
                enemyAttack[22].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                particles[27].Play();
                yield return new WaitForSeconds(1.0f);
                enemyAttack[22].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[22].SetActive(false);
                yield return new WaitForSeconds(4.0f);
                particles[27].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (damageShield.activeInHierarchy)
                {
                    SpawnHazards(0, 7);
                    SpawnHazards(1, 7);
                    SpawnHazards(2, 7);
                    SpawnHazards(3, 7);
                    SpawnHazards(5, 7);
                    SpawnHazards(6, 7);
                }
                else
                {
                    SpawnHazards(0, 5);
                    SpawnHazards(1, 5);
                    SpawnHazards(2, 5);
                    SpawnHazards(3, 5);
                    SpawnHazards(5, 5);
                    SpawnHazards(6, 5);
                    damageShield.SetActive(true);
                    damageShield.GetComponent<DamageShield>().SetProtectValue(10);
                }
            }
            else if (rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 4);
                SetAttackName("Lovely Assistants");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[22].SetActive(true);
                enemyAttack[22].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                particles[28].Play();
                yield return new WaitForSeconds(1.0f);
                enemyAttack[22].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[22].SetActive(false);
                yield return new WaitForSeconds(1.0f);
                particles[28].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (minionCount < minions.Length)
                {
                    SpawnMinion(minionSprites[Random.Range(0, 2) + 8], 10);
                }
                else
                {
                    if (damageShield.activeInHierarchy)
                    {
                        SpawnHazards(0, 3);
                        SpawnHazards(1, 3);
                        SpawnHazards(2, 3);
                        SpawnHazards(3, 3);
                        SpawnHazards(5, 3);
                        SpawnHazards(6, 3);
                    }
                    else
                    {
                        SpawnHazards(0, 1);
                        SpawnHazards(1, 1);
                        SpawnHazards(2, 1);
                        SpawnHazards(3, 1);
                        SpawnHazards(5, 1);
                        SpawnHazards(6, 1);
                        damageShield.SetActive(true);
                        damageShield.GetComponent<DamageShield>().SetProtectValue(15);
                    }
                }
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 12));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 12));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    IEnumerator ErictusAttacking()
    {
        isAttacking = true;
        StartCoroutine(PlayerAttack());
        yield return new WaitForSeconds(4.0f);
        RoundCheck();
        if ((bossMaxHealth - bossCurrentHealth) > 0)
        {
            int rng = Random.Range(0, 6);
            if (rng == 0)
            {
                StartCoroutine(EnemyBaseAttack("No Window Shopping", 7));
                yield return new WaitForSeconds(1.5f);
            }
            else if (rng == 1 || rng == 2 || rng == 3)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 5);
                SetAttackName("Get 1 Random Thing Free");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[23].SetActive(true);
                enemyAttack[23].GetComponent<Animator>().SetBool("IsOpen", true);
                yield return new WaitForSeconds(1.0f);
                particles[30].Play();
                yield return new WaitForSeconds(3.0f);
                enemyAttack[23].GetComponent<Animator>().SetBool("IsOpen", false);
                enemyAttack[23].SetActive(false);
                particles[30].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (damageShield.activeInHierarchy)
                {
                    SpawnHazards(0, 30);
                }
                else
                {
                    SpawnHazards(0, 20);
                    damageShield.SetActive(true);
                    damageShield.GetComponent<DamageShield>().SetProtectValue(15);
                }
            }
            else if (rng == 4 || rng == 5)
            {
                currentPlayerHealth += (bossInfo.bossDamage / 4);
                SetAttackName("Covering Shifts");
                SoundManager.instance.PlaySound(8);
                yield return new WaitForSeconds(0.5f);
                attackName.SetActive(false);
                enemyAttack[24].SetActive(true);
                yield return new WaitForSeconds(0.5f);
                particles[31].Play();
                particles[32].Play();
                yield return new WaitForSeconds(2.0f);
                enemyAttack[24].SetActive(false);
                particles[31].Stop();
                particles[32].Stop();
                PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                if (minionCount < minions.Length)
                {
                    SpawnMinion(minionSprites[Random.Range(0, 2) + 10], 10);
                }
                else
                {
                    damageShield.SetActive(true);
                    damageShield.GetComponent<DamageShield>().SetProtectValue(10);
                }
            }
        }
        else
        {
            StartCoroutine(EndBossFight(true, 10));
            yield return new WaitForSeconds(2.0f);
        }

        if ((PlayerManager.instance.maxHealth - currentPlayerHealth) <= 0)
        {
            StartCoroutine(EndBossFight(false, 10));
            yield return new WaitForSeconds(2.0f);
        }
        isAttacking = false;
    }

    public void SpawnMinion(Sprite newMinionSprite, int attackTimer)
    {
        bool still_looking = true;
        foreach (GameObject minion in minions)
        {
            if (!minion.activeInHierarchy && still_looking)
            {
                minion.SetActive(true);
                minion.GetComponent<SpriteRenderer>().sprite = newMinionSprite;
                minion.GetComponentInChildren<TextMeshPro>().text = "" + attackTimer;
                still_looking = false;
                minionCount++;
            }
        }
    }

    public void ExplodeHazards(int hazardIndex)
    {
        foreach (var position in hazards.cellBounds.allPositionsWithin)
        {
            Vector3Int tempPosition = new Vector3Int(position.x, position.y, position.z);
            if (hazards.HasTile(tempPosition))
            {
                Tile tempTile = (Tile)hazards.GetTile(tempPosition);
                if (tempTile.sprite.Equals(hazardSprite[hazardIndex]))
                {
                    GameObject explosion = Instantiate(explosionPrefab, hazards.CellToWorld(tempPosition) + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    Destroy(explosion, 0.5f);
                    hazards.SetTile(tempPosition, null);
                    currentPlayerHealth += 10;
                    PlayerTakeDamage(PlayerManager.instance.maxHealth - currentPlayerHealth);
                }
            }
        }
    }

    public void RoundReset()
    {
        clicks = 0;
        healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(max, max, false);
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
        hazards = newGrid.transform.GetChild(4).gameObject.GetComponent<Tilemap>();
        gemArray = GenerateGems();
        maxHazardCount = 0;
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
                healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(max - clicks, max, false);
            }
            else
            {
                clickCountRatio.text = 0 + " / " + max;
                healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(0, max, false);
            }
            
        }
        else
        {
            clicks += 1;
            if ((max - clicks) > 0)
            {
                clickCountRatio.text = (max - clicks) + " / " + max;
                healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(max - clicks, max, false);
            }
            else
            {
                clickCountRatio.text = 0 + " / " + max;
                healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(0, max, false);
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
        if (hazards.HasTile(tilePosition))
        {
            Tile tempTile = (Tile)hazards.GetTile(tilePosition);
            if (tempTile.sprite.Equals(hazardSprite[3]) || tempTile.sprite.Equals(hazardSprite[5]) || 
                tempTile.sprite.Equals(hazardSprite[7]) || tempTile.sprite.Equals(hazardSprite[8]))
            {
                if (tempTile.sprite.Equals(hazardSprite[7]))
                {
                    SpawnHazards(7, 1);
                }
                maxHazardCount--;
                hazards.SetTile(tilePosition, null);
            }
            else
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
        else
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

    public void PassTurn()
    {
        if (!isAttacking)
        {
            clicks = max;
            clickCountRatio.text = 0 + " / " + max;
            healthBar.GetComponent<MiningWallHealthBar>().SetPercentage(0, max, false);
        }
    }
}
