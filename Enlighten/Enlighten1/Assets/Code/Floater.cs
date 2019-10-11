using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public float floatSpeed;
    float originalY;
    public float floatHeight;

    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float offsetY = (float)Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector2(transform.position.x, originalY + offsetY);
    }
}
