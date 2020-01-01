using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageShield : MonoBehaviour
{
    public TextMeshPro protectValueText;
    public int protectValue;
    public GameObject damagePrefab;

    private void Start()
    {
        protectValueText = this.GetComponentInChildren<TextMeshPro>();
        protectValueText.text = "" + protectValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            Destroy(collision.gameObject);
            protectValue -= int.Parse(collision.gameObject.GetComponent<TextMeshPro>().text);
            protectValueText.text = "" + protectValue;
            if (protectValue == 0)
            {
                this.gameObject.SetActive(false);
            }
            else if(protectValue < 0)
            {
                GameObject overflowDamage = Instantiate(damagePrefab, this.GetComponentInChildren<RectTransform>().position, Quaternion.identity);
                overflowDamage.GetComponent<TextMeshPro>().text = "" + Mathf.Abs(protectValue);
                overflowDamage.GetComponent<Damage>().speed = 500f;
                this.gameObject.SetActive(false);
            }
        }
    }

    public void SetProtectValue(int value)
    {
        protectValue = value;
        protectValueText.text = "" + protectValue;
    }
}
