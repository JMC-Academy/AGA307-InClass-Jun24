using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyType 
{ 
    OneHanded, 
    TwoHanded, 
    Archer
}
public enum PatrolType { Linear, Random, Loop, Patrol, Detect, Chase, Attack, Die }

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject[] enemyPrefabs;
    public string[] playerNames;
    public Transform[] spawnPoints;
    public List<GameObject> enemies;
    public string killCondition = "archer";
    public float spawnDelay = 1f;
    public int spawnAmount = 6;
    private Vector3 defaultScale = Vector3.one * 2.2f;

    private void Start()
    {
        //SpawnEnemies();
        StartCoroutine(SpawnEnemiesDelay());
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
        enemy.GetComponent<Enemy>().Setup(spawnPoints[rndSpawn]);
        enemy.transform.localScale = defaultScale;
        enemies.Add(enemy);
        _UI.UpdateEnemyCount(enemies.Count);
        //print("Enemy Count is: " + GetEnemyCount());
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
    /// Spawns in enemies with a delay until we reach our spawn amount
    /// </summary>
    private IEnumerator SpawnEnemiesDelay()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnEnemy();
        }
        //yield return new WaitForSeconds(spawnDelay);
        //if (enemies.Count <= spawnAmount)
        //    StartCoroutine(SpawnEnemiesDelay());
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
    public void KillEnemy(GameObject _enemy, float _delay = 0)
    {
        if (GetEnemyCount() == 0)
            return;

        Destroy(_enemy, _delay);
        enemies.Remove(_enemy);
        _UI.UpdateEnemyCount(enemies.Count);
        //print("Enemy Count is: " + GetEnemyCount());
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

        for (int i = 0; i < enemies.Count; i++)
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
    public void KillAllEnemies()
    {
        if (GetEnemyCount() == 0)
            return;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            KillEnemy(enemies[i]);
        }
    }

    /// <summary>
    /// Gets a random spawn point
    /// </summary>
    public Transform GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    /// <summary>
    /// Gets a specific spawn point based on the passed in int
    /// </summary>
    /// <param name="_spawnPoint">The position of the spawn point in the array</param>
    public Transform GetSpawnPoint(int _spawnPoint)
    {
        return spawnPoints[_spawnPoint];
    }

    /// <summary>
    /// Gets the total number of our spawn points
    /// </summary>
    public int GetSpawnPointsCount() => spawnPoints.Length;

    /// <summary>
    /// Scales all enemies to a new size
    /// </summary>
    /// <param name="_scale">The size we want to change to</param>
    public void ScaleEnemies(float _scale)
    {
        defaultScale = Vector3.one * _scale;
        for(int i=0; i<enemies.Count; i++)
        {
            enemies[i].transform.localScale = defaultScale;
        }
    }

    #region Events
    private void OnEnemyHit(GameObject _go)
    {
        
    }

    private void OnEnemyDie(GameObject _go)
    {
        KillEnemy(_go, 5);
    }

    private void OnEnable()
    {
        GameEvents.OnEnemyHit += OnEnemyHit;
        GameEvents.OnEnemyDie += OnEnemyDie;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyHit -= OnEnemyHit;
        GameEvents.OnEnemyDie -= OnEnemyDie;
    }
    #endregion

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
