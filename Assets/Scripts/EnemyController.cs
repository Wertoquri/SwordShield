using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public int hp = 3;
    private float prevHitTime, ignoreDamageWindow;
    private Animator animator;

    private NavMeshAgent agent;
    private Transform playerT;

    private float prevTimeAttack, pauseTimeAttack;
    [SerializeField] Transform[] patrolPos;
    private int currentTargetIndex = 0;
    public bool isAttack = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        prevHitTime = 0f;
        ignoreDamageWindow = 1.5f;

        agent = GetComponent<NavMeshAgent>();
        playerT = GameObject.Find("Player").transform;
        prevTimeAttack = 0f;
        pauseTimeAttack = 2.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Weapon" &&
            Time.time > prevHitTime + ignoreDamageWindow)
        {
            if (other.gameObject.transform.root.gameObject
            .GetComponent<Controller>().isAttack)
            {
                hp--;
                prevHitTime = Time.time;
                if (hp > 1)
                {
                    animator.Play("KnockdownRight");
                }
                else if (hp == 1)
                {
                    animator.Play("Sword_Defeat_1_Start");
                }
                else
                {
                    animator.SetTrigger("isDead");
                }
            }
        }
    }

    private void Update()
    {
        isAttack = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
        if(hp > 1)
        {
            float distanceToPlayer = Vector3.Distance(transform.position
                , playerT.position);
            if(distanceToPlayer < 2.5f)
            {
                Attack();
            }else if(distanceToPlayer > 30f)
            {
                PatrolBehaviour();
            }
            else
            {
                MoveToPlayer();
            }
        }
    }

    private void MoveToPlayer()
    {
        animator.SetBool("isWalk", true);
        agent.destination = playerT.position;
    }

    private void Attack()
    {
        animator.SetBool("isWalk", false);
        agent.destination = transform.position;
        transform.LookAt(playerT.position);
        if (!isAttack && Time.time > prevTimeAttack + pauseTimeAttack)
        {
            animator.Play("Attack");
            prevTimeAttack = Time.time;
        }
    }

    private void PatrolBehaviour()
    {
        if (patrolPos.Length > 0)
        {
            animator.SetBool("isWalk", true);
            agent.destination = patrolPos[currentTargetIndex].position;
            CheckNewPatrolPos();
        }
    }

    private void CheckNewPatrolPos()
    {
        Vector3 target = patrolPos[currentTargetIndex].position;
        if(Vector3.Distance(transform.position, target) < 0.5f)
        {
            if(currentTargetIndex < patrolPos.Length)
            {
                currentTargetIndex++;
            }
            else
            {
                currentTargetIndex = 0;
            }
        }
    }
}
