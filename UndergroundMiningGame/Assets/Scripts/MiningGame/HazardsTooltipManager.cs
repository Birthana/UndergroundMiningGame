using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HazardsTooltipManager : MonoBehaviour
{
    public static HazardsTooltipManager instance = null;
    public GameObject tooltipPanel;
    public bool isVisible;
    public Vector3 hazardPosition;
    public Sprite hazardSprite;
    public string tooltipText;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            tooltipPanel = GameObject.FindGameObjectWithTag("HazardTooltipUI");
            tooltipPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (isVisible)
        {
            tooltipPanel.SetActive(true);
            tooltipPanel.transform.position = hazardPosition + new Vector3(3.0f, 1.0f, 0);
            tooltipPanel.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = hazardSprite;
            tooltipPanel.GetComponentInChildren<TextMeshProUGUI>().text = tooltipText;
        }
        else
        {
            tooltipPanel.SetActive(false);
        }
    }

    public void Appear(Vector3 position, Sprite sprite, string text)
    {
        isVisible = true;
        hazardPosition = position;
        hazardSprite = sprite;
        tooltipText = text;
    }

    public void Disappear()
    {
        isVisible = false;
    }
}
