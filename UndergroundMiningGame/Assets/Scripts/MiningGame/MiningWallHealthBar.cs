using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiningWallHealthBar : MonoBehaviour
{
    float percentage = 1.0f;
    float fullHealth;
    float y;
    public GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = this.transform.GetChild(0).gameObject;
        //fullHealth = healthBar.GetComponent<RectTransform>().anchoredPosition.x;
        y = healthBar.GetComponent<RectTransform>().anchoredPosition.y;
    }

    public void SetPercentage(float currentHealth, float maxHealth)
    {
        percentage = currentHealth / maxHealth;
        healthBar.GetComponent<RectTransform>().localScale = new Vector3(percentage, 1.0f, 1.0f);
        healthBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
    }
}
