using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem
{
    public int gemPosition;
    private int size; //1, 2, or 3
    Vector3Int position;

    public Gem(int newGemPosition, int newSize, Vector3Int newPosition)
    {
        gemPosition = newGemPosition;
        size = newSize;
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
