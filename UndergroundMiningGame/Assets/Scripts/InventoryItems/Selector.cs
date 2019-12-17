using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{

    public void SetPosition(int newPosition)
    {
        this.transform.SetParent(GameObject.Find("Slot (" + newPosition + ")").transform);
        this.transform.SetSiblingIndex(0);
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
    }

    public string GetItemName()
    {
        Sprite item = this.transform.parent.GetChild(1).gameObject.GetComponent<Image>().sprite;
        return item.name;
    }
}
