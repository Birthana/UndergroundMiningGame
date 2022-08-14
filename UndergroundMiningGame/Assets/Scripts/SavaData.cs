using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SavaData
{
    public float x;
    public float y;
    public int[] items;
    public int moneyAmount;
    public bool[] blockades;
    public bool isCrafting;
    public int selectedToolSprite;
    public float nextToolCount;
    public int upgradeDifficulty;

    public SavaData()
    {
        x = 7.0f;
        y = -2.0f;
        items = new int[73];
        moneyAmount = 0;
        blockades = new bool[14];
        isCrafting = false;
        selectedToolSprite = 0;
        nextToolCount = 0.0f;
        upgradeDifficulty = 0;
    }

    public void SetPosition(Vector3 playerPosition)
    {
        x = playerPosition.x;
        y = playerPosition.y;
    }

    public void SetItem(int index, int count)
    {
        items[index] = count;
    }

    public void SetMoney(int money)
    {
        moneyAmount = money;
    }

    public void SetBlockades(bool[] blockadesCleared)
    {
        blockades = blockadesCleared;
    }

    public void SetCrafting(bool c, int toolSpriteIndex, float craftingProgress, int craftingDifficulty)
    {
        isCrafting = c;
        selectedToolSprite = toolSpriteIndex;
        nextToolCount = craftingProgress;
        upgradeDifficulty = craftingDifficulty;
    }

    public void ResetItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = 0;
        }
    }
}
