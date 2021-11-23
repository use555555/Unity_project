using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    public static int maxPlayer = 0;
    public static int Wave = 0;
    public float timeRemaining;
    public bool GameStart = false;
    public GameObject[] Player;
    public GameObject[] Enemy;
    public int Gamemode = 0;
    public int Counting = 0;
    public static int WaveEnemy = 0;
    public static float money = 0f;
    public static float Point = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameStart == false)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
            if (Player.Length == maxPlayer && Player.Length !=0)
            {
                GameStart = true;
            }
        }
        if (GameStart == true)
        {
            if(Gamemode == 0 && Counting == 0)
            {
                timeRemaining = 10;
                Counting = 1;
            }
            else if (Gamemode == 1 && Counting == 0)
            {
                Player = GameObject.FindGameObjectsWithTag("Player");
                WaveEnemy = ( (Wave/2) * Player.Length) + (20 * Wave);
                Counting = 1;
                MonsterSpawnerControl.check = 1;
                MonsterSpawnerControl.spawnAllowed = true;
            }
            if (timeRemaining > 0 && Counting == 1)
            {
                timeRemaining -= Time.deltaTime;
                Debug.Log(timeRemaining.ToString());
            }
            if (Counting == 1)
            {
                Debug.Log(Point);
                if (Gamemode == 0)
                {
                    if(timeRemaining <= 0)
                    {
                        Gamemode = 1;
                        Wave = Wave + 1;
                        Counting = 0;
                    }  
                }
                else if (Gamemode == 1)
                {
                    if (WaveEnemy == 0 && MonsterSpawnerControl.spawnAllowed == true)
                    {
                        MonsterSpawnerControl.spawnAllowed = false;
                    }
                    Enemy = GameObject.FindGameObjectsWithTag("Enemy");

                    if ((WaveEnemy == 0 && Enemy.Length == 0))
                    {
                        Gamemode = 0;
                        Counting = 0;
                    }
                }
            }
        }
    }
}
