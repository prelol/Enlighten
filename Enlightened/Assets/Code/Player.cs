using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private Rigidbody2D rig;
    // Start is called before the first frame update
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 movement = new Vector2(Input.GetAxisRaw("Horizontal") * speed, 0);
        rig.MovePosition(transform.position + (movement * Time.deltaTime));
        if (/*Input.GetAxisRaw("Vertical") < 0 ||*/ Input.GetKeyDown(KeyCode.LeftShift))
        {
            print("Player Is Crouching");
        }
        if (/*Input.GetAxisRaw("Vertical") > 0 ||*/ Input.GetKeyDown(KeyCode.Space))
        {
            print("Player Is Jumping");
            rig.AddForce(new Vector2(0,jumpForce));
        }
    }
}
