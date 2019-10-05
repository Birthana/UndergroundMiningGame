﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Overworld");
    }

    public void Credits()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
