using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public float jumpForce;
    private Rigidbody2D rig;
    private Animator anim;

    private bool isIdle;
    private bool randomBlinkPlaying;
    // Start is called before the first frame update
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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

        Vector3 movement = new Vector2(Input.GetAxisRaw("Horizontal") * speed, 0);
        rig.MovePosition(transform.position + (movement * Time.deltaTime));
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
            rig.AddForce(new Vector2(0,jumpForce));
            anim.SetTrigger("Jump");
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
