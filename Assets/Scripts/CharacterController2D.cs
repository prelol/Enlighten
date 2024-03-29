﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    enum Movement { none, walking, jumping, falling }

    [Header("Movement")]
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpStrength = 500f;
    [SerializeField] float jumpWaitTime = 1f;

    [Space]
    [Header("Physics")]
    [SerializeField] Transform playerFeet;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask crystalLayer;

    [Space]
    [Header("Animation")]
    [SerializeField] Animator anim;
    [SerializeField] Animation startAnim;

    [Space]
    [Header("Controls")]
    [SerializeField] Joystick joystick;

    [Space]
    [Header("Tilemap")]
    [SerializeField] Tilemap gemTilemap;
    [SerializeField] TileBase caveTile;
    [Tooltip("The crystal to spawn after the gem block is interacted with.")]
    [SerializeField] List<GameObject> crystalToSpawn = new List<GameObject>();

    Rigidbody2D rb2d;
    SpriteRenderer sr;

    bool isGrounded = true;
    bool canJump = true;
    bool canMove = false;
    bool canBreakGemBlock = false;

    [HideInInspector] public bool jumpImgBtnPressed = false;

    float horzMovement;

    public bool jumpPressed;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        /*while(canMove != true)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SpawnAnimation"))
            {
                print("Now can move");
                canMove = true;
            }
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        if (StartAnimationPlaying()) canMove = false;
        else canMove = true;

        MovementInput();
        Animate();
    }

    bool StartAnimationPlaying()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SpawnAnimation"))
        {
            return true;
        }
        return false;
    }

    void MovementInput()
    {
        //horzMovement = Input.GetAxisRaw("Horizontal");
        if(joystick.Horizontal > 0.2f)
        {
            horzMovement = 1;
        } else if(joystick.Horizontal < -0.2f)
        {
            horzMovement = -1;
        } else
        {
            horzMovement = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump && isGrounded)
        {
            int layerMask = 1 << 10;
            RaycastHit2D topHit = Physics2D.Raycast(transform.position, Vector3.up, 15f, layerMask);
            RaycastHit2D bottomHit = Physics2D.Raycast(transform.position, Vector3.down, 15f, layerMask);

            if (gemTilemap != null && topHit.collider != null)
            {
                if (gemTilemap.name == topHit.collider.name)
                {
                    canBreakGemBlock = true;
                }
            }

            if (gemTilemap != null && bottomHit.collider != null)
            {
                if (gemTilemap.name == bottomHit.collider.name)
                {
                    canBreakGemBlock = true;
                }
            }

            canJump = false;
            StartCoroutine("JumpCooldown");
            rb2d.AddForce(Vector2.up * jumpStrength);
        } else if (jumpImgBtnPressed && canJump && isGrounded)
        {
            int layerMask = 1 << 10;
            RaycastHit2D topHit = Physics2D.Raycast(transform.position, Vector3.up, 15f, layerMask);
            RaycastHit2D bottomHit = Physics2D.Raycast(transform.position, Vector3.down, 15f, layerMask);

            if (gemTilemap != null && topHit.collider != null)
            {
                if (gemTilemap.name == topHit.collider.name)
                {
                    canBreakGemBlock = true;
                }
            }

            if (gemTilemap != null && bottomHit.collider != null)
            {
                if (gemTilemap.name == bottomHit.collider.name)
                {
                    canBreakGemBlock = true;
                }
            }

            //jumpPressed = false;
            jumpImgBtnPressed = false;
            canJump = false;
            StartCoroutine("JumpCooldown");
            rb2d.AddForce(Vector2.up * jumpStrength);
        }
    }

    void Animate()
    {
        if(horzMovement != 0)
        {
            if (horzMovement > 0) sr.flipX = false;
            else sr.flipX = true;

            if (isGrounded)
                anim.SetFloat("Movement", (int)Movement.walking);
            else
            {
                if(rb2d.velocity.y > 0)
                {
                    anim.SetFloat("Movement", (int)Movement.jumping);
                } else
                {
                    anim.SetFloat("Movement", (int)Movement.falling);
                }
            }
        } else
        {
            if(!isGrounded)
            {
                if(rb2d.velocity.y > 0)
                {
                    //print("Jumping");
                    anim.SetFloat("Movement", (int)Movement.jumping);
                    return;
                } else
                {
                    //print("Falling");
                    anim.SetFloat("Movement", (int)Movement.falling);
                    return;
                }
            }
            anim.SetFloat("Movement", (int)Movement.none);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.CompareTag("Spike"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        Vector3 hitPos = Vector3.zero;
        if (canBreakGemBlock)
        {
            foreach (ContactPoint2D hitPoint in col.contacts)
            {
                hitPos.x = hitPoint.point.x - .5f * hitPoint.normal.x;
                hitPos.y = hitPoint.point.y - .5f * hitPoint.normal.y;
                if (gemTilemap.GetTile(gemTilemap.WorldToCell(hitPos)).name != caveTile.name)
                {
                    gemTilemap.SetTile(gemTilemap.WorldToCell(hitPos), caveTile);
                    GameObject crystal = Instantiate(crystalToSpawn[Random.Range(0, crystalToSpawn.Capacity)], hitPos, Quaternion.identity);
                    crystal.GetComponent<Floater>().canFloat = false;
                }
            }

            canBreakGemBlock = false;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(playerFeet.position, .2f, groundLayer) || Physics2D.OverlapCircle(playerFeet.position, .2f, crystalLayer);
        rb2d.velocity = new Vector2(horzMovement * speed, rb2d.velocity.y);
    }

    public void JumpPressed()
    {
        //print("Jump Pressed!");
        if (isGrounded) jumpPressed = true;
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpWaitTime);
        canJump = true;
    }
}