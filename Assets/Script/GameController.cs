using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    public GameObject Me;
    public static int maxPlayer = 0;
    public static int Wave = 0;
    public static int Death = 0;
    public float timeRemaining;
    public bool GameStart = false;
    public GameObject[] Player;
    public GameObject[] Enemy;
    public int Gamemode = 0;
    public int Counting = 0;
    public static int WaveEnemy = 0;

    public GameObject UiPanel;
    public GameObject Weapon;

    public Text Wave_txt;
    public Text Status_txt;
    public Text Detail_txt;
    public Text Health_txt;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Death);
        if (Gamemode == 0)
        {
            StatusupdateClientRpc(Gamemode, (int)timeRemaining);
        }
        else if (Gamemode == 1)
        {
            Enemy = GameObject.FindGameObjectsWithTag("Enemy");
            StatusupdateClientRpc(Gamemode, (int)(Enemy.Length+WaveEnemy));
        }
        HealthClientRpc();
        WaveupdateClientRpc(Wave);
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
                timeRemaining = 5;
                Counting = 1;
            }
            else if (Gamemode == 1 && Counting == 0)
            {
                Player = GameObject.FindGameObjectsWithTag("Player");
                WaveEnemy = ((Wave * 5) * Player.Length) + (5 * Wave);
                //WaveEnemy = ((Wave * 1) * Player.Length) + (1 * Wave);
                Counting = 1;
                MonsterSpawnerControl.check = 1;
                MonsterSpawnerControl.spawnAllowed = true;
            }
            if (timeRemaining > 0 && Counting == 1)
            {
                timeRemaining -= Time.deltaTime;
            }
            if (Counting == 1)
            {
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
                        Death = 0;
                        ResetClientRpc();
                    }
                }
            }
        }
    }

    [ClientRpc]
    public void HealthClientRpc()
    {
        Player = GameObject.FindGameObjectsWithTag("Player");
        string Health = "";
        int Ref = 1;
        foreach (var player in Player)
        {
            Health += "P"+Ref.ToString()+": "+player.GetComponent<PlayerHealth>().health.Value.ToString()+" | ";
            Ref += 1;
        }
        Health_txt.text = Health;
    }
    [ClientRpc]
    public void ResetClientRpc()
    {
        foreach (var player in Player)
        {
            player.GetComponent<PlayerHealth>().Heal();
        }
    }

    [ClientRpc]
    public void WaveupdateClientRpc(int Wave)
    {
        Wave_txt.text = Wave.ToString();
    }

    [ClientRpc]
    public void StatusupdateClientRpc(int Mode,int Detail)
    {
        if (Mode == 0)
        {
            Status_txt.text = "Standby";
            Detail_txt.text = Detail.ToString();
        }
        else if (Mode == 1)
        {
            Status_txt.text = "Remaining";
            Detail_txt.text = Detail.ToString();
        }
    }
}
