using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int rifleAmmo;
    public int level = 1;
    public int score;
    [HideInInspector] public int toLevelUp;
    [HideInInspector] public int xpGoal;
    [HideInInspector] public float healthBonus;
    [HideInInspector] public float damageBonus;
    [HideInInspector] public float attackSpeedBonus;
    

    private void Start()
    {
        toLevelUp = level * 100;
        xpGoal = toLevelUp;
    }

    public void LevelUp()
    {
        healthBonus += level * 10;
        damageBonus += level * 10;
        attackSpeedBonus = (level - 1) * 2;
        toLevelUp = level * 100;
        xpGoal = toLevelUp;
        level++;

        var playerHealth = GetComponent<PlayerHealth>();
        playerHealth.currentHealth += healthBonus;
        playerHealth.maxHealth += healthBonus;
    }
}
