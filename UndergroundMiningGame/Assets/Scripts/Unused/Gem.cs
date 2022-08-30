using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem
{
    public enum Size { SMALL, MEDIUM, LARGE };
    private Size size_;
    public int gemPosition;
    private Vector3Int position;

    public int size;

    public static Size GetRandomWeightedSize(float smallPercent, float mediumPercent, float LargePercent)
    {
        float rngNumber = UnityEngine.Random.value;
        if (rngNumber < LargePercent)
        {
            return Size.LARGE;
        }
        else if (rngNumber - LargePercent < mediumPercent)
        {
            return Size.MEDIUM;
        }
        else
        {
            return Size.SMALL;
        }
    }

    #region Function: Constructors
    public Gem(int newGemPosition, int newSize, Vector3Int newPosition)
    {
        gemPosition = newGemPosition;
        size = newSize;
        position = newPosition;
    }
    public Gem(Size newSize, int newGemPosition, Vector3Int newPosition)
    {
        size_ = newSize;
        gemPosition = newGemPosition;
        position = newPosition;
    }
    #endregion

    #region Function: Setters & Getters
    public int getGemPosition()
    {
        return gemPosition;
    }

    public Size GetSize()
    {
        return size_;
    }

    public Vector3Int getPosition()
    {
        return position;
    }
    #endregion
}
