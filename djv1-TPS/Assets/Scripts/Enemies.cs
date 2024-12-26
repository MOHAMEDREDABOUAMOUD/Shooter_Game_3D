using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemies : MonoBehaviour
{
    [SerializeField] private GameObject aiAgentFistFab;
    [SerializeField] private GameObject aiAgentKnifeFab;
    [SerializeField] private GameObject aiAgentSwordFab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject spawnEffect;

    // How fast the enemies spawn
    public float spawnRate = 5f;
    // Enemies can't spawn faster than this
    public float minSpawRate;
    // Max number of enemies in the scene at one time
    public int maxEnemies;
    private List<GameObject> enemies;
    private float timer;
    

    private void Start()
    {
        enemies = new();
        timer = spawnRate;
    }

    public void Update()
    {
        if (timer <= 0 && enemies.Count < maxEnemies)
        {
            StartCoroutine(SpawnEnemy());
            timer = spawnRate;
        } 
        timer -= Time.deltaTime;
    }

    private IEnumerator SpawnEnemy()
    {
        
        AiAgentConfig.AttackType type =
            (AiAgentConfig.AttackType)Random.Range(0, System.Enum.GetNames(typeof(AiAgentConfig.AttackType)).Length);
        GameObject enemyToSpawn;
        switch (type)
        {
            case(AiAgentConfig.AttackType.Fist) : 
                enemyToSpawn = aiAgentFistFab;
                break;
            case(AiAgentConfig.AttackType.Knife):
                enemyToSpawn = aiAgentKnifeFab;
                break;
            default:
                enemyToSpawn = aiAgentSwordFab;
                break;
        }

        int posIndex = Random.Range(0, spawnPoints.Count);

        var effect = Instantiate(spawnEffect, spawnPoints[posIndex].position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        var enemy = Instantiate(enemyToSpawn, spawnPoints[posIndex].position, Quaternion.identity);
        enemies.Add(enemy);
        Destroy(effect);
    }

    public bool RemoveEnemy(GameObject enemy)
    {
        return enemies.Remove(enemy);
    }

    public void LevelUp()
    {
        maxEnemies += 5;
        var newSpawnRate = spawnRate - spawnRate * 10 / 100;
        spawnRate = newSpawnRate < minSpawRate ? minSpawRate : newSpawnRate;
    }
    
}
