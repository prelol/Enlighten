using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    enum Movement { none, walking, jumping, falling}

    [Header("Movement")]
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpStrength = 500f;
    [SerializeField] float jumpWaitTime = 1f;

    [Space]
    [Header("Physics")]
    [SerializeField] Transform playerFeet;
    [SerializeField] LayerMask groundLayer;

    [Space]
    [Header("Animation")]
    [SerializeField] Animator anim;
    [SerializeField] Animation startAnim;

    [Space]
    [Header("Controls")]
    [SerializeField] Joystick joystick;

    Rigidbody2D rb2d;
    SpriteRenderer sr;

    bool isGrounded = true;
    bool canJump = true;
    bool canMove = false;

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
            canJump = false;
            StartCoroutine("JumpCooldown");
            rb2d.AddForce(Vector2.up * jumpStrength);
        } else if(jumpPressed && canJump && isGrounded)
        {
            jumpPressed = false;
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
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(playerFeet.position, .2f, groundLayer);
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
