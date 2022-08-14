using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Camera.main.GetComponent<Camera>().orthographicSize = PlayerPrefs.GetInt("CameraSize");
        if (Input.GetKeyDown(KeyCode.M) && !PlayerManager.instance.isPaused)
        {
            if (PlayerPrefs.GetInt("CameraSize") < 4)
            {
                PlayerPrefs.SetInt("CameraSize", PlayerPrefs.GetInt("CameraSize") + 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.N) && !PlayerManager.instance.isPaused)
        {
            if (PlayerPrefs.GetInt("CameraSize") > 2)
            {
                PlayerPrefs.SetInt("CameraSize", PlayerPrefs.GetInt("CameraSize") - 1);
            }
        }
    }
}
