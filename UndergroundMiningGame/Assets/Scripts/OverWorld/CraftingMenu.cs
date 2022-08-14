using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingMenu : MonoBehaviour
{
    public Animator anim;
    public Sprite[] toolSprites;
    public Item[] toolItems;
    public GameObject smithingBar;
    public GameObject selectedTool;
    public GameObject nextUpgradeTool;
    public GameObject selectedToolButton;
    public GameObject selectedGemButton;
    public GameObject selector;
    public TextMeshProUGUI craftingRatio;

    public bool isCrafting;
    public int selectedToolSprite;
    public float nextToolCount;
    public int upgradeDifficulty;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        selector = GameObject.FindGameObjectWithTag("Selector");
        selectedTool.SetActive(false);
        nextUpgradeTool.SetActive(false);
        selectedGemButton.SetActive(false);

        if (PlayerManager.instance.continuing)
        {
            isCrafting = PlayerManager.instance.playerData.isCrafting;
            selectedToolSprite = PlayerManager.instance.playerData.selectedToolSprite;
            nextToolCount = PlayerManager.instance.playerData.nextToolCount;
            upgradeDifficulty = PlayerManager.instance.playerData.upgradeDifficulty;
            if (isCrafting)
            {
                selectedToolButton.SetActive(false);
                selectedGemButton.SetActive(true);
                selectedTool.SetActive(true);
                selectedTool.GetComponent<Image>().sprite = toolSprites[selectedToolSprite];
                if (selectedToolSprite != toolSprites.Length/2 - 1 || selectedToolSprite != toolSprites.Length - 1)
                {
                    nextUpgradeTool.SetActive(true);
                    nextUpgradeTool.GetComponent<Image>().sprite = toolSprites[selectedToolSprite + 1];
                    smithingBar.GetComponent<RectTransform>().localScale = new Vector3(1, nextToolCount / (25 + ((upgradeDifficulty / 2) * 25)), 0);
                    smithingBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                    craftingRatio.text = nextToolCount + "/" + (25 + ((upgradeDifficulty / 2) * 25));
                }
            }
        }
        else
        {
            isCrafting = false;
            selectedToolSprite = 0;
            nextToolCount = 0;
            upgradeDifficulty = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !PlayerManager.instance.isPaused)
        {
            if (anim.GetBool("IsOpen"))
            {
                anim.SetBool("IsOpen", false);
            }
            else
            {
                anim.SetBool("IsOpen", true);
            }
        }
    }

    public void Save()
    {
        PlayerManager.instance.playerData.SetCrafting(isCrafting, selectedToolSprite, nextToolCount, upgradeDifficulty);
    }

    public void SelectTool()
    {
        selectedToolSprite = CheckToolName(selector.GetComponent<Selector>().GetItemName());
        if (selectedToolSprite < toolSprites.Length && !(selectedToolSprite == toolSprites.Length / 2 - 1 || selectedToolSprite == toolSprites.Length - 1))
        {
            isCrafting = true;
            selectedToolButton.SetActive(false);
            selectedGemButton.SetActive(true);
            selectedTool.SetActive(true);
            selectedTool.GetComponent<Image>().sprite = toolSprites[selectedToolSprite];
            if (!(selectedToolSprite == toolSprites.Length / 2 - 1 || selectedToolSprite == toolSprites.Length - 1))
            {
                nextUpgradeTool.SetActive(true);
                nextUpgradeTool.GetComponent<Image>().sprite = toolSprites[selectedToolSprite + 1];
                smithingBar.GetComponent<RectTransform>().localScale = new Vector3(1, 0, 0);
                craftingRatio.text = nextToolCount + "/" + (25 + ((upgradeDifficulty / 2) * 25));
            }
            Save();
        }
    }

    public int CheckToolName(string selectorItemName)
    {
        int result = toolSprites.Length;
        if (selectorItemName != null)
        {
            for (int i = 0; i < toolSprites.Length; i++)
            {
                if (selectorItemName.Equals(toolSprites[i].name))
                {
                    result = i;
                }
            }
        }
        return result;
    }

    public void SelectGem()
    {
        if (CheckToolName(selector.GetComponent<Selector>().GetItemName()) >= toolSprites.Length && selector.GetComponent<Selector>().GetItemName() != null)
        {
            float total = (25 + ((upgradeDifficulty/2) * 25));
            
            GemItem tempGemItem = (GemItem)Inventory.instance.GetItemFromSpriteName(selector.GetComponent<Selector>().GetItemName());
            if (tempGemItem.grade.Equals(GemItem.GemGrade.smallBasic)) 
            {
                nextToolCount += (tempGemItem.count * 1);
            }
            else if (tempGemItem.grade.Equals(GemItem.GemGrade.mediumBasic))
            {
                nextToolCount += (tempGemItem.count * 4);
            }
            else if (tempGemItem.grade.Equals(GemItem.GemGrade.largeBasic))
            {
                nextToolCount += (tempGemItem.count * 9);
            }
            smithingBar.GetComponent<RectTransform>().localScale = new Vector3(1, nextToolCount / total, 0);
            smithingBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            craftingRatio.text = nextToolCount + "/" + total;
            tempGemItem.count = 0;
            Inventory.instance.inventory.Remove(tempGemItem);
            Inventory.instance.DisplayInventory();
            if (nextToolCount >= total)
            {
                isCrafting = false;
                nextToolCount = 0;
                upgradeDifficulty++;
                string temp = nextUpgradeTool.GetComponent<Image>().sprite.name;
                if (temp.Equals("tools_9") || temp.Equals("tools_10") || temp.Equals("tools_11") ||
                    temp.Equals("tools_12") || temp.Equals("tools_13") || temp.Equals("tools_14") ||
                    temp.Equals("tools_15") || temp.Equals("tools_16") || temp.Equals("tools_17"))
                {
                    PlayerManager.instance.currentHammerToolSprite = nextUpgradeTool.GetComponent<Image>().sprite;
                    PlayerManager.instance.playerData.SetItem(Inventory.instance.GetItemIndex(Inventory.instance.GetItemFromSpriteName(selectedTool.GetComponent<Image>().sprite.name)), 0);
                    Inventory.instance.inventory.Remove(Inventory.instance.GetItemFromSpriteName(selectedTool.GetComponent<Image>().sprite.name));
                    Inventory.instance.inventory.Insert(0, GetItemFromSpriteName(nextUpgradeTool.GetComponent<Image>().sprite.name));
                    Inventory.instance.DisplayInventory();
                }
                else
                {
                    PlayerManager.instance.playerData.SetItem(Inventory.instance.GetItemIndex(Inventory.instance.GetItemFromSpriteName(selectedTool.GetComponent<Image>().sprite.name)), 0);
                    Inventory.instance.inventory.Remove(Inventory.instance.GetItemFromSpriteName(selectedTool.GetComponent<Image>().sprite.name));
                    Inventory.instance.inventory.Insert(1, GetItemFromSpriteName(nextUpgradeTool.GetComponent<Image>().sprite.name));
                    Inventory.instance.DisplayInventory();
                }
                //Inventory.instance.AddItem(GetItemFromSpriteName(nextUpgradeTool.GetComponent<Image>().sprite.name));
                smithingBar.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 0);
                smithingBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                craftingRatio.text = "";
                selectedTool.SetActive(false);
                nextUpgradeTool.SetActive(false);
                selectedToolButton.SetActive(true);
                selectedGemButton.SetActive(false);
            }
            Save();
        }
    }

    public Item GetItemFromSpriteName(string spriteName)
    {
        Item itemFromSprite = toolItems[0];
        foreach (Item tempItem in toolItems)
        {
            if (tempItem.image.name.Equals(spriteName))
            {
                itemFromSprite = tempItem;
            }
        }
        return itemFromSprite;
    }
}
