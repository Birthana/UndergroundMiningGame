using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Boss.asset", menuName = "Boss/Boss")]
public class Boss : ScriptableObject
{
    public Sprite bossSprite;
    [TextArea(2,3)]
    public string bossName;
    public int bossMaxHealth;
    public int bossDamage;
}
