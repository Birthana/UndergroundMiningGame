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

    public Tilemap viewCheck;
    public GameObject limitedView;

    public AnimationClip[] testClips;
    
    void Start()
    {
        this.transform.position = new Vector3(PlayerManager.instance.playerData.x, PlayerManager.instance.playerData.y, 0.0f);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerSounds = GameObject.FindGameObjectWithTag("PlayerSounds").GetComponent<AudioSource>();
        tileMap = GameObject.FindGameObjectWithTag("Floor").GetComponent<Tilemap>();
        viewCheck = GameObject.FindGameObjectWithTag("ViewCheck").GetComponent<Tilemap>();
        limitedView = GameObject.FindGameObjectWithTag("LimitedView");
        limitedView.SetActive(false);
        //PlayerAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        CheckView();

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

    public void CheckView()
    {
        Vector3 playerPosition = this.GetComponent<RectTransform>().transform.position;
        Vector3Int tilePosition = viewCheck.WorldToCell(playerPosition);
        if (viewCheck.HasTile(tilePosition))
        {
            Tile playerTile = (Tile)viewCheck.GetTile(tilePosition);
            currentSprite = playerTile.sprite;
            string tileSprite = playerTile.sprite.name;
            if (tileSprite.Equals("general_spritesheet2_19") || tileSprite.Equals("general_spritesheet2_14") ||
                tileSprite.Equals("general_spritesheet2_15") || tileSprite.Equals("general_spritesheet2_16") ||
                tileSprite.Equals("general_spritesheet2_27"))
            {
                limitedView.SetActive(true);
            }
            else
            {
                if (limitedView.activeInHierarchy)
                {
                    limitedView.SetActive(false);
                }
            }
        }
    }

    public void BunnyWaifuAnimation()
    {
        AnimatorOverrideController temp = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = temp;
        temp["Choochoo_Idle_Left"] = testClips[0];
        temp["Choochoo_Idle_Up"] = testClips[1];
        temp["Choochoo_Idle_Down"] = testClips[2];
        temp["Choochoo_Idle_Right"] = testClips[3];
        temp["Choochoo_Move_Left"] = testClips[4];
        temp["Choochoo_Move_Up"] = testClips[5];
        temp["Choochoo_Move_Down"] = testClips[6];
        temp["Choochoo_Move_Right"] = testClips[7];
    }

    public void ChooChooAnimation()
    {
        AnimatorOverrideController temp = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = temp;
        temp["Choochoo_Idle_Left"] = testClips[8];
        temp["Choochoo_Idle_Up"] = testClips[9];
        temp["Choochoo_Idle_Down"] = testClips[10];
        temp["Choochoo_Idle_Right"] = testClips[11];
        temp["Choochoo_Move_Left"] = testClips[12];
        temp["Choochoo_Move_Up"] = testClips[13];
        temp["Choochoo_Move_Down"] = testClips[14];
        temp["Choochoo_Move_Right"] = testClips[15];
    }

    public void GenericMinerAnimation()
    {
        AnimatorOverrideController temp = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = temp;
        temp["Choochoo_Idle_Left"] = testClips[16];
        temp["Choochoo_Idle_Up"] = testClips[17];
        temp["Choochoo_Idle_Down"] = testClips[18];
        temp["Choochoo_Idle_Right"] = testClips[19];
        temp["Choochoo_Move_Left"] = testClips[20];
        temp["Choochoo_Move_Up"] = testClips[21];
        temp["Choochoo_Move_Down"] = testClips[22];
        temp["Choochoo_Move_Right"] = testClips[23];
    }
}
