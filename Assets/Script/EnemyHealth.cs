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
    private void Start()
    {

    }
    public void TakeDamange(float damage)
    {
        enemyhealth.Value -= damage;
        // check health
        if (enemyhealth.Value <= 0)
        {
            Destroy(gameObject);
            GameController.money = GameController.money + enemyPoint;
            GameController.Point = GameController.Point + enemyPoint;
        }
    }   
}
