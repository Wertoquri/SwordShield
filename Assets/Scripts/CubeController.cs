using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
     
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MovingWithRigidbody(5f);
    }

    public void MovingWithTransformPosition()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y,
                transform.position.z + 0.1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y,
                transform.position.z - 0.1f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - 0.1f,
                transform.position.y,
                transform.position.z );
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + 0.1f,
                transform.position.y,
                transform.position.z );
        }
    }

    public void MovingWithTransformTranslate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.forward * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(transform.forward * Time.fixedDeltaTime * -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(transform.right * Time.fixedDeltaTime * -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(transform.right * Time.fixedDeltaTime);
        }
    }

    public void MovingWithRigidbody(float speed = 4f)
    {
        float dirX = Input.GetAxis("Horizontal");
        float dirZ = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(dirX * speed, rb.velocity.y, dirZ * speed);
    }
}
