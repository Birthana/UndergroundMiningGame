using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    public Image[] image;

    public Item placeHolder;

    public Animator inventoryAnim;
    public Animator moneyAnim;

    // Start is called before the first frame update
    void Start()
    {
        image = this.GetComponentsInChildren<Image>();
        TextMeshProUGUI[] text = this.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var temp in text)
        {
            if (!(temp.text.Equals("<") || temp.text.Equals(">")))
            {
                temp.text = "";
            }
        }
        AddItem(placeHolder);
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
    }

    public void AddItem(Item item)
    {
        inventory.Add(item);
        //index++;
        DisplayInventory();
    }

    public void DisplayInventory()
    {
        int count = 1;
        foreach (var item in inventory)
        {
            while (!image[count].name.Equals("Item"))
            {
                count++;
            }
            if (image[count].name.Equals("Item"))
            {
                image[count].sprite = item.image;
                image[count].color = new Color(255, 255, 255, 255);
                if (item.GetType().Equals(System.Type.GetType("GemItem")))
                {
                    GemItem gemItem = (GemItem)item;
                    image[count].GetComponentInChildren<TextMeshProUGUI>().text = "" + gemItem.count;
                }
                else
                {
                    image[count].GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
                count++;
            }
        }
    }
}
