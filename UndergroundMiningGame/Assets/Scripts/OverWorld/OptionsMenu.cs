using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionMenu;
    public GameObject muteButton;
    public AnimationClip[] optionsAnimations;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Muted"))
        {
            PlayerPrefs.SetFloat("Muted", 1.0f);
        }
        else
        {
            if (PlayerPrefs.GetFloat("Muted") == 0.0f)
            {
                AudioListener.volume = 0.0f;
                muteButton.GetComponent<TextMeshProUGUI>().text = "YES";
            }
            else
            {
                AudioListener.volume = 1.0f;
                muteButton.GetComponent<TextMeshProUGUI>().text = "NO";
            }
        }

        if (!PlayerPrefs.HasKey("Character"))
        {
            SelectChooChoo();
        }
        else
        {
            if (PlayerPrefs.GetString("Character").Equals("Choochoo"))
            {
                SelectChooChoo();
            }else if (PlayerPrefs.GetString("Character").Equals("BunnyWaifu"))
            {
                SelectBunnyWaifu();
            }else if (PlayerPrefs.GetString("Character").Equals("GenericMiner"))
            {
                SelectGenericMiner();
            }
        }
        optionMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !PlayerManager.instance.isPaused && !PlayerManager.instance.animationPlaying)
        {
            StartCoroutine(OpenOptionsMenu());
        }
    }

    IEnumerator OpenOptionsMenu()
    {
        PlayerManager.instance.isPaused = true;
        optionMenu.SetActive(true);
        optionMenu.GetComponent<Animator>().SetBool("IsOpen", true);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        StartCoroutine(CloseOptionsMenu());
    }

    IEnumerator CloseOptionsMenu()
    {
        Time.timeScale = 1.0f;
        optionMenu.GetComponent<Animator>().SetBool("IsOpen", false);
        yield return new WaitForSeconds(0.5f);
        PlayerManager.instance.isPaused = false;
        optionMenu.SetActive(false);
    }

    public void Mute()
    {
        if (PlayerPrefs.GetFloat("Muted") == 0.0f)
        {
            PlayerPrefs.SetFloat("Muted", 1.0f);
            AudioListener.volume = 1.0f;
            muteButton.GetComponent<TextMeshProUGUI>().text = "NO";
        }
        else
        {
            PlayerPrefs.SetFloat("Muted", 0.0f);
            AudioListener.volume = 0.0f;
            muteButton.GetComponent<TextMeshProUGUI>().text = "YES";
        }
    }

    public void SelectBunnyWaifu()
    {
        PlayerPrefs.SetString("Character", "BunnyWaifu");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().BunnyWaifuAnimation();
        AnimatorOverrideController temp = new AnimatorOverrideController(optionMenu.GetComponent<Animator>().runtimeAnimatorController);
        temp["OptionsMenu_Idle"] = optionsAnimations[3];
        temp["OptionsMenu_Open"] = optionsAnimations[4];
        temp["OptionsMenu_Close"] = optionsAnimations[5];
    }

    public void SelectChooChoo()
    {
        PlayerPrefs.SetString("Character", "Choochoo");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().ChooChooAnimation();
        AnimatorOverrideController temp = new AnimatorOverrideController(optionMenu.GetComponent<Animator>().runtimeAnimatorController);
        temp["OptionsMenu_Idle"] = optionsAnimations[0];
        temp["OptionsMenu_Open"] = optionsAnimations[1];
        temp["OptionsMenu_Close"] = optionsAnimations[2];
    }

    public void SelectGenericMiner()
    {
        PlayerPrefs.SetString("Character", "GenericMiner");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().GenericMinerAnimation();
        AnimatorOverrideController temp = new AnimatorOverrideController(optionMenu.GetComponent<Animator>().runtimeAnimatorController);
        temp["OptionsMenu_Idle"] = optionsAnimations[6];
        temp["OptionsMenu_Open"] = optionsAnimations[7];
        temp["OptionsMenu_Close"] = optionsAnimations[8];
    }
}
