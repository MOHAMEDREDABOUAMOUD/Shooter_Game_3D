using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiLocomotion : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private readonly int speedId = Animator.StringToHash("Speed");
    private Health health;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
    }

    void Update()
    {
        if(health.currentHealth <= 0)
            return;
        if (agent.hasPath)
        {
            animator.SetFloat(speedId, agent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat(speedId, 0);
        }
    }
}
