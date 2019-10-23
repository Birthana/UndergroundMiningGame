using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GemItem.asset", menuName = "Item/Gem")]
public class GemItem : Item
{
    public enum GemGrade
    {
        smallBasic, mediumBasic, largeBasic
    }

    public GemGrade grade;

    public int count;
}
