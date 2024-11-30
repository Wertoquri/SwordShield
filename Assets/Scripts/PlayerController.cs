using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveX, moveY, moveZ;
    [SerializeField] float speed = 4f;
    CharacterController ch;

    private void Start()
    {
        ch = GetComponent<CharacterController>();
        moveY = 0;
    }

    private void MovingWithCharacterController()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        if (ch.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveY = 0.15f;
            }
            else 
            {
                moveY = 0f;
            }
        }
        else
        {
            moveY = -1f * Time.fixedDeltaTime;
        }
        ch.Move(new Vector3(moveX * speed * Time.fixedDeltaTime, 
            moveY, 
            moveZ * speed * Time.fixedDeltaTime));
    }

    private void Update()
    {
        MovingWithCharacterController();
    }
}
