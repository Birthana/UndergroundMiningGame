﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    public int currentPosition;

    public void SetPosition(int newPosition)
    {
        currentPosition = newPosition;
        this.transform.SetParent(GameObject.Find("Slot (" + newPosition + ")").transform);
        this.transform.SetSiblingIndex(0);
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
    }

    public string GetItemName()
    {
        string result = null;
        Sprite item = this.transform.parent.GetChild(1).gameObject.GetComponent<Image>().sprite;
        if(item != null)
        {
            result = item.name;
        }
        return result;
    }
}
