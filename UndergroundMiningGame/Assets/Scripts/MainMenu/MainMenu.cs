using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator playButton;
    public Animator creditsButton;
    public Animator quitButton;

    public Animator creditsText;
    public Animator backButton;

    void Start()
    {
        playButton.SetBool("IsOpen", true);
        creditsButton.SetBool("IsOpen", true);
        quitButton.SetBool("IsOpen", true);
        creditsText.SetBool("IsOpen", false);
        backButton.SetBool("IsOpen", false);
    }

    public void Play()
    {
        SceneManager.LoadScene("Overworld");
    }
    public void Credits()
    {
        playButton.SetBool("IsOpen", false);
        creditsButton.SetBool("IsOpen", false);
        quitButton.SetBool("IsOpen", false);
        creditsText.SetBool("IsOpen", true);
        backButton.SetBool("IsOpen", true);
    }

    public void Back()
    {
        playButton.SetBool("IsOpen", true);
        creditsButton.SetBool("IsOpen", true);
        quitButton.SetBool("IsOpen", true);
        creditsText.SetBool("IsOpen", false);
        backButton.SetBool("IsOpen", false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
