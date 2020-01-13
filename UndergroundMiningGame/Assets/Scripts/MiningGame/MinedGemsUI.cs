using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinedGemsUI : MonoBehaviour
{
    public static MinedGemsUI instance = null;
    public List<Item> itemsMined;
    public GameObject minedGemsPanel;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            itemsMined = new List<Item>();
            minedGemsPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Display()
    {
        if (itemsMined.Count > 0)
        {
            StartCoroutine(Showcase());
        }
    }

    IEnumerator Showcase()
    {
        minedGemsPanel.SetActive(true);
        foreach (Item gemItem in itemsMined)
        {
            GameObject minedItemImage = new GameObject();
            Image newImage = minedItemImage.AddComponent<Image>();
            newImage.sprite = gemItem.image;
            minedItemImage.transform.SetParent(minedGemsPanel.transform.GetChild(0));
            SoundManager.instance.PlaySound(12);
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(1.0f);
        minedGemsPanel.SetActive(false);
        for (int i = 0; i < minedGemsPanel.transform.GetChild(0).childCount; i++)
        {
            Destroy(minedGemsPanel.transform.GetChild(0).GetChild(i).gameObject);
        }
        itemsMined.Clear();
    }

    public void AddMinedItem(Item minedItem)
    {
        itemsMined.Add(minedItem);
    }
}
