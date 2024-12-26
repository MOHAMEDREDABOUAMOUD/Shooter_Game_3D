using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    public AiAgentConfig config;
    [HideInInspector] public Ragdoll ragdoll;
    [HideInInspector] public SkinnedMeshRenderer mesh;
    [HideInInspector] public UIHealthBar ui;
    
    [HideInInspector] public Transform playerTransform;
    public Transform attackPoint;

    public float currentAttackPower;
    public int currentXp;
    public float health;
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        ui = GetComponentInChildren<UIHealthBar>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        stateMachine = new AiStateMachine(this);
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiAttackPlayerState());
        stateMachine.ChangeState(initialState);
        
        ui.DisableUI();

        currentAttackPower = config.attackPower + (Game.Instance.player.level * 20 * config.attackPower) / 100;
        currentXp = config.xp + (Game.Instance.player.level * 20 * config.xp) / 100;
        health = config.health + (Game.Instance.player.level * 20 * config.health) / 100;
    }

    void Update()
    {
        stateMachine.Update();
    }

    private void Attack()
    {
        AiAttackPlayerState attackPlayerState= (AiAttackPlayerState) stateMachine.GetState(AiStateId.AttackPlayer);
        attackPlayerState.Attack();
    }
}
