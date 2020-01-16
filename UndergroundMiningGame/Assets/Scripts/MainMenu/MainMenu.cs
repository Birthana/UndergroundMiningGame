using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator playButton;
    public Animator creditsButton;
    public Animator quitButton;
    public Animator titleText;

    public Animator creditsText;
    public Animator backButton;

    public Animator newGameButton;
    public Animator continueButton;

    void Start()
    {
        playButton.SetBool("IsOpen", true);
        creditsButton.SetBool("IsOpen", true);
        quitButton.SetBool("IsOpen", true);
        titleText.SetBool("IsOpen", true);
        creditsText.SetBool("IsOpen", false);
        backButton.SetBool("IsOpen", false);
        newGameButton.SetBool("IsOpen", false);
        continueButton.SetBool("IsOpen", false);
    }

    public void Play()
    {
        playButton.SetBool("IsOpen", false);
        creditsButton.SetBool("IsOpen", false);
        quitButton.SetBool("IsOpen", false);
        titleText.SetBool("IsOpen", true);
        creditsText.SetBool("IsOpen", false);
        backButton.SetBool("IsOpen", true);
        newGameButton.SetBool("IsOpen", true);
        if (DataAccess.HasSaveFile())
        {
            continueButton.SetBool("IsOpen", true);
        }
    }

    public void NewGame()
    {
        StartCoroutine(OpenTransition(false));
    }

    public void Continue()
    {
        StartCoroutine(OpenTransition(true));
    }

    IEnumerator OpenTransition(bool continuing)
    {
        PlayerManager.instance.continuing = continuing;
        PlayerManager.instance.SetPlayerData();
        TransitionsManager.instance.Open();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        playButton.SetBool("IsOpen", false);
        creditsButton.SetBool("IsOpen", false);
        quitButton.SetBool("IsOpen", false);
        titleText.SetBool("IsOpen", false);
        creditsText.SetBool("IsOpen", true);
        backButton.SetBool("IsOpen", true);
        newGameButton.SetBool("IsOpen", false);
        continueButton.SetBool("IsOpen", false);
    }

    public void Back()
    {
        playButton.SetBool("IsOpen", true);
        creditsButton.SetBool("IsOpen", true);
        quitButton.SetBool("IsOpen", true);
        titleText.SetBool("IsOpen", true);
        creditsText.SetBool("IsOpen", false);
        backButton.SetBool("IsOpen", false);
        newGameButton.SetBool("IsOpen", false);
        continueButton.SetBool("IsOpen", false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
