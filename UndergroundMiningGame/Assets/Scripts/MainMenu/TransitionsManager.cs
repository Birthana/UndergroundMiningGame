using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TransitionsManager : MonoBehaviour
{
    public static TransitionsManager instance = null;
    public GameObject transitionPanel;
    public GameObject bossTransitionPanel;
    public Sprite bossSprite;
    public string bossName;
    public ParticleSystem bossParticle;
    public Sprite[] playerSprites;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (PlayerPrefs.HasKey("Character"))
            {
                if (PlayerPrefs.GetString("Character").Equals("Choochoo"))
                {
                    bossTransitionPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = playerSprites[0];
                }else if (PlayerPrefs.GetString("Character").Equals("BunnyWaifu"))
                {
                    bossTransitionPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = playerSprites[1];
                }else if (PlayerPrefs.GetString("Character").Equals("GenericMiner"))
                {
                    bossTransitionPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = playerSprites[2];
                }
            }
            else
            {
                bossTransitionPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = playerSprites[0];
            }
            transitionPanel.SetActive(false);
            bossTransitionPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void Open()
    {
        StartCoroutine(Transition());
    }

    public void BossOpen()
    {
        StartCoroutine(BossTransition());
    }

    IEnumerator BossTransition()
    {
        GameObject playerUI = GameObject.FindGameObjectWithTag("PlayerUI");
        if (playerUI != null)
        {
            playerUI.SetActive(false);
        }
        bossTransitionPanel.SetActive(true);
        bossTransitionPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = bossSprite;
        bossTransitionPanel.transform.GetChild(0).gameObject.GetComponentInChildren<TextMeshProUGUI>().text = bossName;
        if (PlayerPrefs.GetString("Character").Equals("Choochoo"))
        {
            bossTransitionPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = playerSprites[0];
        }
        else if (PlayerPrefs.GetString("Character").Equals("BunnyWaifu"))
        {
            bossTransitionPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = playerSprites[1];
        }
        else if (PlayerPrefs.GetString("Character").Equals("GenericMiner"))
        {
            bossTransitionPanel.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = playerSprites[2];
        }
        bossTransitionPanel.GetComponent<Animator>().SetBool("IsOpen", true);
        yield return new WaitForSeconds(1.0f);
        bossParticle.Play();
        yield return new WaitForSeconds(1.9f);
        if (playerUI != null){
            playerUI.SetActive(true);
        }
        yield return new WaitForSeconds(0.1f);
        bossParticle.Stop();
        bossTransitionPanel.GetComponent<Animator>().SetBool("IsOpen", false);
        yield return new WaitForSeconds(0.5f);
        bossTransitionPanel.SetActive(false);
    }

    IEnumerator Transition()
    {
        GameObject playerUI = GameObject.FindGameObjectWithTag("PlayerUI");
        if (playerUI != null)
        {
            playerUI.SetActive(false);
        }
        transitionPanel.SetActive(true);
        transitionPanel.GetComponent<Animator>().SetBool("IsOpen", true);
        yield return new WaitForSeconds(1.0f);
        transitionPanel.GetComponent<Animator>().SetBool("IsOpen", false);
        if (playerUI != null)
        {
            playerUI.SetActive(true);
        }
        yield return new WaitForSeconds(1.0f);
        transitionPanel.SetActive(false);
    }
}
