using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem
{
    public enum Size { SMALL, MEDIUM, LARGE };
    public Size size_;
    public int gemPosition;
    private int size; //1, 2, or 3
    Vector3Int position;

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


    public int getGemPosition()
    {
        return gemPosition;
    }

    public int getSize()
    {
        return size;
    }

    public Vector3Int getPosition()
    {
        return position;
    }
}
