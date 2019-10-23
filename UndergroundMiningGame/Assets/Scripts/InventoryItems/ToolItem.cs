using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ToolItem.asset", menuName = "Item/Tool")]
public class ToolItem: Item
{
    public enum ToolType
    {
        hammer, pick
    }

    public enum ToolGrade
    {
        basic, notBasic, iAmMadeOfMoney
    }

    public ToolType type;
    public ToolGrade grade;
}
