using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public float speed = 2.0f;
    public Animator animator;

    public AudioSource playerSounds;
    public Tilemap tileMap;
    public Sprite currentSprite;
    public AudioClip[] walkingSounds;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSounds = GameObject.FindGameObjectWithTag("PlayerSounds").GetComponent<AudioSource>();
        tileMap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = this.GetComponent<RectTransform>().transform.position;
        Vector3Int tilePosition = tileMap.WorldToCell(playerPosition);
        AudioClip tempClip = walkingSounds[0];
        if (tileMap.HasTile(tilePosition))
        {
            Tile playerTile = (Tile)tileMap.GetTile(tilePosition);
            currentSprite = playerTile.sprite;
            string tileSprite = playerTile.sprite.name;
            if (tileSprite.Equals("general_spritesheet2_19"))
            {
                tempClip = walkingSounds[0];
            }
            else if (tileSprite.Equals("magma_spritesheet_22"))
            {
                tempClip = walkingSounds[1];
            }
            else if (tileSprite.Equals("redrock_spritesheet_24"))
            {
                tempClip = walkingSounds[1];
            }
            else if (tileSprite.Equals("ice_spritesheet2_20"))
            {
                tempClip = walkingSounds[2];
            }
            else if (tileSprite.Equals("water_spritesheet_23"))
            {
                tempClip = walkingSounds[5];
            }
            else if (tileSprite.Equals("desert_spritesheet_27"))
            {
                tempClip = walkingSounds[4];
            }
            else if (tileSprite.Equals("mushroom_spritesheet_20"))
            {
                tempClip = walkingSounds[3];
            }
            else if (tileSprite.Equals("plant_spritesheet_27"))
            {
                tempClip = walkingSounds[3];
            }
        }
        if (!playerSounds.clip.Equals(tempClip))
        {
            playerSounds.clip = tempClip;
            playerSounds.loop = true;
            playerSounds.Play();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h == 0 && v == 0)
        {
            playerSounds.loop = false;
        }
        else
        {
            playerSounds.loop = true;
        }

        animator.SetFloat("horizontal", h);
        animator.SetFloat("vertical", v);

        Vector2 tempVect = new Vector2(h, v);
        animator.SetFloat("speed", tempVect.sqrMagnitude);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + tempVect * speed);
    }
}
