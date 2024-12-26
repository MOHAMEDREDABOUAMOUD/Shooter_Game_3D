using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Idle;
    }

    public void Enter(AiAgent agent)
    {
    }

    public void Update(AiAgent agent)
    {
        Vector3 playerDirection = agent.playerTransform.position - agent.transform.position;
        // If the player is very far from the agent we do nothing
        if(playerDirection.magnitude > agent.config.maxSightDistance)
            return;
        Vector3 agentDirection = agent.transform.forward;
        
        playerDirection.Normalize();

        float dotProduct = Vector3.Dot(playerDirection, agentDirection);
        // If the player and the agent are facing the same direction we change player state from idle to chasePlayer
        if (dotProduct > 0f)
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
        }
    }

    public void Exit(AiAgent agent)
    {
    }
}
