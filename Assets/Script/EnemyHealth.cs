using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class EnemyHealth : NetworkBehaviour
{
    public NetworkVariableFloat enemyhealth = new NetworkVariableFloat(100f);
    public float enemyPoint = 100f;
    public GameObject[] Player;
    public void TakeDamange(float damage)
    {
        enemyhealth.Value -= damage;
        // check health
        if (enemyhealth.Value <= 0)
        {
            Player = GameObject.FindGameObjectsWithTag("Player");
            KilledClientRpc(enemyPoint);
        }
    }

    [ClientRpc]
    public void KilledClientRpc(float enemyPoint)
    {
        foreach (var player in Player)
        {
            player.GetComponent<PlayerDetail>().Point.Value = player.GetComponent<PlayerDetail>().Point.Value + enemyPoint;
            player.GetComponent<PlayerDetail>().Money.Value = player.GetComponent<PlayerDetail>().Money.Value + enemyPoint;
        }
        Destroy(gameObject);
    }

}
