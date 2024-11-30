using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private float angleY, dirZ, jumpForce = 6f, turnSpeed = 80f;
    private bool isGrounded = true;
    private Rigidbody rb;
    private Animator animator;
    private Vector3 jumpDir;

    [SerializeField] GameObject sword;
    [SerializeField] GameObject hips;
    [SerializeField] GameObject rightHand;

    public bool isAttack = false;

    Vector3 LSP;
    Quaternion LSR;

    bool isEquip = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        angleY = Input.GetAxis("Mouse X") * turnSpeed * Time.fixedDeltaTime;
        dirZ = Input.GetAxis("Vertical");
        transform.Rotate(new Vector3(0f, angleY, 0f));
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            else
            {
                animator.SetTrigger("isLanded");
            }

            Move(dirZ, "isWalkForward", "isWalkBack");
            Dodge();
            Sprint();
        }
        else
        {
            MoveInAir();
        }
        if (Input.GetKeyDown(KeyCode.Q) && !isEquip)
        {
            animator.Play("Sword_Equip");
            isEquip = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isEquip)
        {
            animator.Play("Sword_Holster");
            isEquip = false;
        }
        if (Input.GetMouseButtonDown(0) && isEquip)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    animator.Play("SA1");
                    break;
                case 1:
                    animator.Play("SA2");
                    break;
                case 2:
                    animator.Play("SA3");
                    break;
            }
            isAttack = true;
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("SA1") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("SA2") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("SA3"))
            isAttack = false;
    }

    private void Move(float dir, string parametrName, string altParametrName)
    {
        if (dir > 0f)
        {
            animator.SetBool(parametrName, true);
        }
        else if (dir < 0f)
        {
            animator.SetBool(altParametrName, true);
        }
        else
        {
            animator.SetBool(parametrName, false);
            animator.SetBool(altParametrName, false);
        }
    }

    private void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.Play("Sword_Dodge_Right");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            animator.Play("Sword_Dodgle_Left");
        }
    }

    private void Sprint()
    {
        animator.SetBool("isRun", Input.GetKey(KeyCode.LeftShift));
    }

    private void Jump()
    {
        animator.Play("Sword_Jump_Platformer_Start");
        animator.applyRootMotion = false;
        jumpDir = new Vector3(0f, jumpForce, (dirZ * jumpForce) / 2f);
        jumpDir = transform.TransformDirection(jumpDir);
        rb.AddForce(jumpDir, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        animator.applyRootMotion = true;
        isGrounded = true;
    }

    private void MoveInAir()
    {
        if (new Vector2(rb.velocity.x, rb.velocity.z).magnitude < 1.1f)
        {
            jumpDir = new Vector3(0f, rb.velocity.y, dirZ);
            jumpDir = transform.TransformDirection(jumpDir);
            rb.velocity = jumpDir;
        }
    }

    public void EquipSword()
    {
        LSP = sword.transform.localPosition;
        LSR = sword.transform.localRotation;
        sword.transform
            .SetParent(rightHand.transform);
    }

    public void UnEquipSword()
    {
        sword.transform
            .SetParent(hips.transform);
        sword.transform.localPosition = LSP;
        sword.transform.localRotation = LSR;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EWeapon")
        {
            if (other.gameObject.transform.root.gameObject
            .GetComponent<EnemyController>().isAttack)
                animator.Play("Sword_Hit_L_2");
        }
    }
}
