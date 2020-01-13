using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTooltipManager : MonoBehaviour
{
    public static InteractTooltipManager instance = null;
    public GameObject tooltipPanel;
    public bool isVisible;
    public Vector3 interactablePosition;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            tooltipPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isVisible)
        {
            tooltipPanel.SetActive(true);
            tooltipPanel.transform.position = Camera.main.WorldToScreenPoint(interactablePosition);
            tooltipPanel.GetComponent<Animator>().SetBool("IsOpen", true);
        }
        else
        {
            tooltipPanel.SetActive(false);
            tooltipPanel.GetComponent<Animator>().SetBool("IsOpen", false);
        }
    }

    public void Appear(Vector3 position)
    {
        isVisible = true;
        interactablePosition = position;
    }

    public void Disappear()
    {
        isVisible = false;
    }
}
