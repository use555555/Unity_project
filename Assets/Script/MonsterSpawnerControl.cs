using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using MLAPI.Spawning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnerControl : NetworkBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] zombie;
    int randomSpawnPoint, randomZombie;
    public static bool spawnAllowed;
    public int zombie_type;
    public static int check = 0;

    // Start is called before the first frame update
    void Start()
    {
        spawnAllowed = false;
        InvokeRepeating("SpawnAMonster", 0f, 1f);
    }
    void SpawnAMonster()
    {
        if (spawnAllowed)
        {
            randomSpawnPoint = Random.Range(0, spawnPoints.Length);
            randomZombie = Random.Range(0, zombie_type);
            GameObject enemy = Instantiate(zombie[randomZombie], spawnPoints[randomSpawnPoint].position, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn();
            ulong zombieNetID = enemy.GetComponent<NetworkObject>().NetworkObjectId;
            GameController.WaveEnemy = GameController.WaveEnemy - 1;
            SpawnAMonsterClient(zombieNetID);
        }
    }
    void SpawnAMonsterClient(ulong zombieNetID)
    {
        NetworkObject netObj = NetworkSpawnManager.SpawnedObjects[zombieNetID];
    }

    private void Update()
    {
        if (check == 1)
        {
            check = 0;
            if (GameController.Wave % 3 == 1 && zombie_type < 6)
            {
                zombie_type = zombie_type + 1;
            }
        }
    }
}
