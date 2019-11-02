using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    public GameObject[] inventorySlots;

    void Start()
    {
        inventorySlots = GameObject.FindGameObjectsWithTag("InventorySlot");
    }

    public void SetPosition(int newPosition)
    {
        this.transform.SetParent(inventorySlots[newPosition].transform);
        this.transform.SetSiblingIndex(0);
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
    }

    public string GetItemName()
    {
        Sprite item = this.transform.parent.GetChild(1).gameObject.GetComponent<Image>().sprite;
        return item.name;
    }
}
