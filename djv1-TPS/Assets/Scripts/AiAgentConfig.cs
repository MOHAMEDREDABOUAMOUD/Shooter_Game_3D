using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiAgentConfig : ScriptableObject
{
    public enum AttackType
    {
        Fist = 0,
        Knife = 1,
        Sword = 2
    }
    /// <summary>
    /// Time between updates to look for player for better performance 
    /// </summary>
    public float maxTime = 1f;
    /// <summary>
    /// Used to determine how far will the agent stop from the player
    /// </summary>
    public float maxDistance = 1f;
    /// <summary>
    /// Force to push the agent when he dies 
    /// </summary>
    public float dieForce = 10f;
    /// <summary>
    /// How far does the agent detect the player
    /// </summary>
    public float maxSightDistance = 5f;
    /// <summary>
    /// how many second to attack again, small value mean faster attack 
    /// </summary>
    public float attackSpeed = 2.633f;
    /// <summary>
    /// How mush damage the enemy does to the player;
    /// </summary>
    public float attackPower = 20f;
    public float rotationSpeed = 10f;
    public AttackType attackType = AttackType.Fist;
    public int xp;
    public float health;
    /// <summary>
    /// The chance the agnet will drop an item when he died
    /// </summary>
    public float dropItemChance = 0.5f;
}
