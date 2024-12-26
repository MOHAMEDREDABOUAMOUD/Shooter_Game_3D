using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackPlayerState : AiState
{
    private float timeToAttack;
    private Animator animator;
    private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
    private AiAgent currentAgent;
    private static readonly int AttackType = Animator.StringToHash("AttackType");

    public AiStateId GetId()
    {
        return AiStateId.AttackPlayer;
    }

    public void Enter(AiAgent agent)
    {
        animator = agent.GetComponent<Animator>();
    }

    public void Update(AiAgent agent)
    {
        currentAgent = agent;
        RotateTowards(agent.playerTransform, agent);
        if (timeToAttack <= 0f)
        {
            animator.SetBool(IsAttacking, true);
            animator.SetInteger(AttackType, (int)agent.config.attackType);
        }
        timeToAttack -= Time.deltaTime;
        var distance = (agent.playerTransform.position - agent.transform.position).magnitude;
        if (distance > agent.config.maxDistance)
        {
            animator.SetBool(IsAttacking, false);
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }

    public void Exit(AiAgent agent)
    {
        timeToAttack = 0f;
    }

    public void Attack()
    {
        if (timeToAttack > 0f) return;
        if(!currentAgent) return;
        var position = currentAgent.attackPoint.position;
        Vector3 start = position;
        Vector3 end = (position + currentAgent.attackPoint.forward * 1.5f);
        float amount = currentAgent.currentAttackPower;
        Vector3 direction = end - start;
        //Debug.DrawRay(start, direction, Color.red, 20f);

        Collider[] hitColliders = Physics.OverlapSphere(start, 2);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                playerHealth.TakeDamage(amount, direction * 2);
            }
        }
        timeToAttack = currentAgent.config.attackSpeed;
    }
    
    private void RotateTowards (Transform target, AiAgent agent) {
        Vector3 direction = (target.position - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * agent.config.rotationSpeed);
    }
}
