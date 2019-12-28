using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory instance = null;
    public List<Item> inventory = new List<Item>();
    public Image[] inventorySlot = new Image[18];
    public Image[] image;
    public Item basicHammer;
    public Item basicPick;
    public Animator inventoryAnim;
    public Animator moneyAnim;
    public TextMeshProUGUI moneyAmount;
    public int page;
    public Sprite background;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameObject inventoryUI = GameObject.FindGameObjectWithTag("Inventory");
            image = inventoryUI.GetComponentsInChildren<Image>();
            inventoryAnim = inventoryUI.GetComponent<Animator>();
            TextMeshProUGUI[] text = inventoryUI.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var temp in text)
            {
                if (!(temp.text.Equals("<") || temp.text.Equals(">")))
                {
                    temp.text = "";
                }
            }
            page = 1;
            int count = 1;
            int index = 0;
            for (int i = 0; i < 18; i++)
            {
                while (!image[count].name.Equals("Item"))
                {
                    count++;
                }
                if (image[count].name.Equals("Item"))
                {
                    inventorySlot[index] = image[count];
                    index++;
                    count++;
                }
            }
            AddItem(basicHammer);
            PlayerManager.instance.currentHammerToolSprite = basicHammer.image;
            AddItem(basicPick);
            GameObject moneyUI = GameObject.FindGameObjectWithTag("Money");
            moneyAnim = moneyUI.GetComponent<Animator>();
            moneyAmount = moneyUI.GetComponentInChildren<TextMeshProUGUI>();
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (inventoryAnim.GetBool("IsOpen"))
            {
                inventoryAnim.SetBool("IsOpen", false);
                moneyAnim.SetBool("IsOpen", false);
            }
            else
            {
                inventoryAnim.SetBool("IsOpen", true);
                moneyAnim.SetBool("IsOpen", true);
            }
        }
        float sw = Input.GetAxis("Mouse ScrollWheel");
        GameObject selector = GameObject.FindGameObjectWithTag("Selector");
        if (sw > 0f)
        {
            if (selector.GetComponent<Selector>().currentPosition == 0)
            {
                selector.GetComponent<Selector>().SetPosition(17);
            }
            else
            {
                selector.GetComponent<Selector>().SetPosition(selector.GetComponent<Selector>().currentPosition - 1);
            }
        }
        else if(sw < 0f)
        {
            if (selector.GetComponent<Selector>().currentPosition == 17)
            {
                selector.GetComponent<Selector>().SetPosition(0);
            }
            else
            {
                selector.GetComponent<Selector>().SetPosition(selector.GetComponent<Selector>().currentPosition + 1);
            }
        }
    }

    public void AddItem(Item item)
    {
        if (Contains(item))
        {
            foreach (var inventoryItem in inventory)
            {
                if (inventoryItem.itemName.Equals(item.itemName) && inventoryItem.GetType().Equals(System.Type.GetType("GemItem")))
                {
                    GemItem inventoryGem = (GemItem) inventoryItem;
                    inventoryGem.count++;
                }
            }
            DisplayInventory();
        }
        else
        {
            if (item.GetType().Equals(System.Type.GetType("GemItem")))
            {
                GemItem tempItem = (GemItem)item;
                tempItem.count++;
                item = tempItem;
            }
            inventory.Add(item);
            DisplayInventory();
        }
    }

    public bool Contains(Item item)
    {
        bool result = inventory.Contains(item);
        return result;
    }

    public void DisplayInventory()
    {
        int skipCount = 0;
        if (page > 1)
        {
            skipCount =  18 *  (page - 1);
        }
        int inventoryCount = 0;
        foreach (var item in inventory)
        {
            if(skipCount > 0)
            {
                skipCount--;
                continue;
            }

            if (inventoryCount < 18) {
                inventorySlot[inventoryCount].sprite = item.image;
                inventorySlot[inventoryCount].color = new Color(255, 255, 255, 255);
                if (item.GetType().Equals(System.Type.GetType("GemItem")))
                {
                    GemItem gemItem = (GemItem)item;
                    inventorySlot[inventoryCount].GetComponentInChildren<TextMeshProUGUI>().text = "" + gemItem.count;
                }
                else
                {
                    inventorySlot[inventoryCount].GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
                inventoryCount++;
            }
        }
        while (inventoryCount < 18)
        {
            inventorySlot[inventoryCount].sprite = background;
            inventorySlot[inventoryCount].color = new Color(150, 150, 150, 255);
            inventorySlot[inventoryCount].GetComponentInChildren<TextMeshProUGUI>().text = "";
            inventoryCount++;
        }
    }

    public void NextPage(bool next)
    {
        if (inventory.Count > page * 18 && next)
        {
            page++;
            
        }
        else if (page > 1 && !next)
        {
            page--;

        }
        DisplayInventory();
    }

    public Item GetItemFromSpriteName(string spriteName)
    {
        Item[] inventoryArray = inventory.ToArray();
        Item itemFromSprite = inventoryArray[0];
        foreach (Item tempItem in inventoryArray)
        {
            if (tempItem.image.name.Equals(spriteName))
            {
                itemFromSprite = tempItem;
            }
        }
        return itemFromSprite;
    }
}
