using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Play()
    {
        SceneManager.LoadScene("Overworld");
    }

    void Credits()
    {

    }

    void Quit()
    {
        Application.Quit();
    }
}
