using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Damage : MonoBehaviour
{
    public GameObject target;
    public bool isPausing;
    public float speed;
    public float stoppingDistance;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Pausing());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(this.transform.position, target.transform.position) > stoppingDistance)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            if (this.GetComponent<TextMeshPro>().text.Equals("x2"))
            {
                target.GetComponent<TextMeshProUGUI>().text = "" + (int.Parse(target.GetComponent<TextMeshProUGUI>().text) + int.Parse(target.GetComponent<TextMeshProUGUI>().text));
                Destroy(gameObject);
            }
            else
            {
                target.GetComponent<TextMeshProUGUI>().text = "" + (int.Parse(target.GetComponent<TextMeshProUGUI>().text) + int.Parse(this.GetComponent<TextMeshPro>().text));
                Destroy(gameObject);
            }
        }
    }

    IEnumerator Pausing()
    {
        target = GameObject.FindGameObjectWithTag("DamageModifier");
        yield return new WaitForSeconds(0.3f);
    }
}
