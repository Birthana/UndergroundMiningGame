using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public AudioSource collisionSound;
    float h = 0.0f;
    float v = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        collisionSound.PlayDelayed(1);
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        collisionSound.loop = true;
        if (h == 0 && v == 0 || h != Input.GetAxis("Horizontal") || v != Input.GetAxis("Vertical"))
        {
            collisionSound.loop = false;
            collisionSound.Stop();
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        collisionSound.loop = false;
        collisionSound.Stop();
    }
}
