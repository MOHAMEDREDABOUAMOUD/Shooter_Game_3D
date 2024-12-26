using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Game : Singleton<Game>
{
    [SerializeField] private GameObject pistolPickupFab;
    [SerializeField] private GameObject riflePickupFab;
    
    public Player player;
    public Enemies enemies;

    private void Start()
    {
        SpawnStartupWeapon();
        enemies = GetComponent<Enemies>();
    }

    private void SpawnStartupWeapon()
    {
        var playerTransform = player.transform;
        Instantiate(pistolPickupFab, 
            playerTransform.position + playerTransform.forward * 2 + Vector3.up, 
            Quaternion.identity);

        Instantiate(riflePickupFab,
            playerTransform.position + (playerTransform.forward + playerTransform.right) * 2 + Vector3.up,
            Quaternion.identity);
    }

    private void Update()
    {
        if (player.toLevelUp <= 0)
        {
            player.LevelUp();
            enemies.LevelUp();
        }
    }
}
