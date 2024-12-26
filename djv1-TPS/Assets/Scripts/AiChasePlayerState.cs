using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChasePlayerState : AiState
{
    private float timer = 0f;
    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Enter(AiAgent agent)
    {

    }

    public void Update(AiAgent agent)
    {
        timer -= Time.deltaTime;
        // For better performance we don't update agent destination every frame
        if (timer < 0f)
        {
            var distance = (agent.playerTransform.position - agent.navMeshAgent.destination).magnitude;
            if (distance > agent.config.maxDistance)
            {
                agent.navMeshAgent.destination = agent.playerTransform.position;
            }
            else
            {
                agent.stateMachine.ChangeState(AiStateId.AttackPlayer);
            }
            timer = agent.config.maxTime;
        }
    }

    public void Exit(AiAgent agent)
    {
        
    }
}
