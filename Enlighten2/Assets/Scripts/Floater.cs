using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public float floatSpeed;
    float originalY;
    float targetPos = 0f;
    public float floatHeight;
    [Tooltip("When the crystal spawns after a gem block is interacted with.")]
    public float spawnOffsetY = 1.5f;

    Vector3 velocity = Vector3.zero;
    Vector3 offset = Vector3.up;

    [HideInInspector]
    public bool canFloat = true;

    // Start is called before the first frame update
    void Start()
    {
        if (!canFloat)
        {
            Collider2D lightCollider;
            int layerMask = 1 << 11;
            lightCollider = Physics2D.OverlapCircle(transform.position, 2f, layerMask);
            Destroy(lightCollider.gameObject);
        }

        originalY = transform.position.y;
        targetPos = originalY + spawnOffsetY; 
    }

    // Update is called once per frame
    void Update()
    {
        if (canFloat)
        {
            float offsetY = (float)Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector2(transform.position.x, originalY + offsetY);
        }
        else
        {
            if (transform.position.y < targetPos)
            {
                transform.position = Vector3.SmoothDamp(transform.position, transform.position + offset, ref velocity, .05f);
            }
        }
    }
}
