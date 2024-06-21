using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public string[] playerNames;
    public Transform[] spawnPoints;
    public List<GameObject> enemies;
    public string killCondition = "archer";

    private void Start()
    {
        SpawnEnemies();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SpawnEnemies();
        if (Input.GetKeyDown(KeyCode.K))
            KillRandomEnemy();
        if (Input.GetKeyDown(KeyCode.L))
            KillSpecificEnemy(killCondition);
        if (Input.GetKeyDown(KeyCode.J))
            KillAllEnemies();
    }

    /// <summary>
    /// Spawns an single enemy
    /// </summary>
    private void SpawnEnemy()
    {
        int rndEnemy = Random.Range(0, enemyPrefabs.Length);
        int rndSpawn = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyPrefabs[rndEnemy], spawnPoints[rndSpawn].position, spawnPoints[rndSpawn].rotation);
        enemies.Add(enemy);
        print("Enemy Count is: " + GetEnemyCount());
    }

    /// <summary>
    /// Spawns an enemy based on the spawn points length
    /// </summary>
    private void SpawnEnemies()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnEnemy();
        }
    }

    /// <summary>
    /// Gets the amount of enemies in our scene
    /// </summary>
    /// <returns>The amount of enemies</returns>
    private int GetEnemyCount()
    {
        return enemies.Count;
    }

    /// <summary>
    /// Kills an enemy
    /// </summary>
    /// <param name="_enemy">The enemy GameObject</param>
    private void KillEnemy(GameObject _enemy)
    {
        if (GetEnemyCount() == 0)
            return;

        Destroy(_enemy);
        enemies.Remove(_enemy);
        print("Enemy Count is: " + GetEnemyCount());
    }

    /// <summary>
    /// Kills a random enemy
    /// </summary>
    private void KillRandomEnemy()
    {
        if (GetEnemyCount() == 0)
            return;

        int rnd = Random.Range(0, GetEnemyCount());
        KillEnemy(enemies[rnd]);
    }

    /// <summary>
    /// Kills a specific enemy
    /// </summary>
    /// <param name="_condition">The condition of the enemy we want to kill</param>
    private void KillSpecificEnemy(string _condition)
    {
        if (GetEnemyCount() == 0) 
            return;

        for(int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].name.Contains(_condition))
            {
                KillEnemy(enemies[i]);
            }
        }
    }

    /// <summary>
    /// Kills all enemies in our enemy list
    /// </summary>
    private void KillAllEnemies()
    {
        if (GetEnemyCount() == 0)
            return;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            KillEnemy(enemies[i]);
        }
    }

    #region Examples
    private void RandomFun()
    {
        int rnd = Random.Range(0, 10);
        print(rnd);
        print(playerNames[Random.Range(0, playerNames.Length)]);
    }

    private void LoopFun()
    {
        for (int i = 0; i < 10; i++)
        {
            print(i);
        }

        for (int i = 0; i < playerNames.Length; i++)
        {
            print("Player " + (i+1) + " name is: " + playerNames[i]);
        }
    }

    private void ArrayFun()
    {
        print(playerNames[0]);
        print(playerNames.Length);
        playerNames[0] = "Noddy";

        print("Player 1 name is: " + playerNames[0]);
        print("Player 2 name is: " + playerNames[1]);
        print("Player 3 name is: " + playerNames[2]);
        print("Player 4 name is: " + playerNames[3]);
        print("Player 5 name is: " + playerNames[4]);
    }
    #endregion
}
