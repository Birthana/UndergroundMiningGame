using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Title : MonoBehaviour
{
    public TMP_Text title;
    public string[] titles = new string[18];
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        title = GameObject.FindGameObjectWithTag("Title").GetComponent<TMP_Text>();
        titles[0] = "Untitled Mining Game";
        titles[1] = "Underground Game";
        titles[2] = "Capitialism Simulator";
        titles[3] = "Mine of Duty: Modern Rockfare";
        titles[4] = "Undertale 3";
        titles[5] = "The Age of Capitialism";
        titles[6] = "Black Lung Disease: \nA Love Story";
        titles[7] = "Minecraft Superior";
        titles[8] = "PokeMine";
        titles[9] = "Proletariat vs. Bourgeoise";
        titles[10] = "Gold Rush: Instant Death Mode";
        titles[11] = "Exploitation of Natural Resources!";
        titles[12] = "Colonialism";
        titles[13] = "The Prospective Prospector";
        titles[14] = "Seizing the Mines of Production";
        titles[15] = "Sanctions of Ore";
        titles[16] = "I Can't Believe It's Not Pokemon";
        titles[17] = "The Life of a CPP \nAfter Dropping Out";
        StartCoroutine(RandomTitle());
    }
    
    IEnumerator RandomTitle()
    {
        while (true)
        {
            int rng = Random.Range(0, 18);
            title.text = titles[rng];
            yield return new WaitForSeconds(speed);
        }
    }
}
