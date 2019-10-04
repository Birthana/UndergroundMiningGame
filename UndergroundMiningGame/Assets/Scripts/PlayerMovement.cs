using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float speed = 2.0f;
    public Animator animator;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        animator.SetFloat("horizontal", h);
        animator.SetFloat("vertical", v);

        Vector2 tempVect = new Vector2(h, v);
        animator.SetFloat("speed", tempVect.sqrMagnitude);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + tempVect * speed);
    }
}
