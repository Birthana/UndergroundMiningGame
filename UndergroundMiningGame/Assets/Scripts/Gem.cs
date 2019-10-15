using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem
{
    private int size; //1, 2, or 3
    Vector3Int position;
    //potential value field?
    public Gem(int newSize, Vector3Int newPosition)
    {
        size = newSize;
        position = newPosition;
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
