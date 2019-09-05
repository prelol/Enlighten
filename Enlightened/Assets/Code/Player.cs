using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class Player : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    float horizMovement;
    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private bool isIdle;
    private bool randomBlinkPlaying;

    bool jumping;

    Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {

        if(!anim.GetBool("Crouch") || !anim.GetBool("Moving"))
        {
            isIdle = true;
        }
        else
        {
            isIdle = false;
        }

        if(isIdle && !randomBlinkPlaying)
        {
            StartCoroutine(randomBlink());
        }

        movement = new Vector2(Input.GetAxisRaw("Horizontal") * speed, 0);

        anim.SetFloat("Speed", Mathf.Abs(movement.x));

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            spriteRenderer.flipX = false;
        }

        //rig.MovePosition(transform.position + (movement * Time.deltaTime));
        if (/*Input.GetAxisRaw("Vertical") < 0 ||*/ Input.GetKeyDown(KeyCode.LeftShift))
        {
            print("Player Is Crouching");
            anim.SetBool("Crouch", true);
        }
        else
        {
            anim.SetBool("Crouch", false);
        }
        if (/*Input.GetAxisRaw("Vertical") > 0 ||*/ Input.GetKeyDown(KeyCode.Space))
        {
            print("Player Is Jumping");
            anim.SetBool("Jump", true);
            jumping = true;
        }

        if (rig.velocity.y < 0) { anim.SetBool("Jump", false); }
    }

    private void FixedUpdate()
    {
        rig.position += (movement * Time.fixedDeltaTime);

        //if (rig.velocity.y < 0) { anim.SetBool("Jump", false); }

        if (jumping)
        {
            rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumping = false;
        }
    }

    public IEnumerator randomBlink()
    {
        randomBlinkPlaying = true;
        yield return new WaitForSeconds(Random.Range(1, 2));
        anim.SetTrigger("Blink");
        if (!isIdle)
            randomBlinkPlaying = false;
        else
            StartCoroutine(randomBlink());

    }
}
