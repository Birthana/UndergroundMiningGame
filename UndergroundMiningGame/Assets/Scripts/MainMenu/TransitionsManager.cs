using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionsManager : MonoBehaviour
{
    public static TransitionsManager instance = null;
    public GameObject transitionPanel;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transitionPanel.SetActive(false);
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

    IEnumerator Transition()
    {
        transitionPanel.SetActive(true);
        transitionPanel.GetComponent<Animator>().SetBool("IsOpen", true);
        yield return new WaitForSeconds(1.0f);
        transitionPanel.GetComponent<Animator>().SetBool("IsOpen", false);
        yield return new WaitForSeconds(1.0f);
        transitionPanel.SetActive(false);
    }
}
