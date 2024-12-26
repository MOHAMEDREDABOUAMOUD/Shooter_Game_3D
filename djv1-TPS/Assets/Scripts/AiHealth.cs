using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiHealth : Health
{
    private AiAgent agent;
    [SerializeField] private GameObject[] items;
    protected override void OnStart()
    {
        agent = GetComponent<AiAgent>();
        maxHealth = agent.health;
        currentHealth = agent.health;
    }

    protected override void OnDeath(Vector3 direction)
    {
        AiDeathState deathState = agent.stateMachine.GetState(AiStateId.Death) as AiDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AiStateId.Death);
        Game.Instance.enemies.RemoveEnemy(gameObject);
        Game.Instance.player.toLevelUp -= agent.currentXp;
        Game.Instance.player.score += agent.currentXp;
        
        // May drop an item when he died
        var chance = Random.Range(0f, 1f);
        if (chance <= agent.config.dropItemChance)
        {
            var itemIndex = Random.Range(0, items.Length);
            Instantiate(items[itemIndex], transform.position + transform.up, quaternion.identity);
        }
    }

    protected override void OnDamage(Vector3 direction)
    {
        agent.ui.EnableUI();
    }
}
