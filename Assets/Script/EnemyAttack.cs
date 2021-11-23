using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class EnemyAttack : NetworkBehaviour
{
    public Component attackpoint;
    float attackRater = 2f;
    float attackTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= 1f / attackRater)
        {
            attackTimer = 0;
            AttackServerRpc();
        }
    }
    [ServerRpc]
    void AttackServerRpc()
    {
        Ray ray = new Ray(attackpoint.transform.position, attackpoint.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            //we hit something
            var player = hit.collider.GetComponent<PlayerHealth>();
            if (player != null)
            {
                //we hit a player
                player.TakeDamange(30f);
            }
        }
    }
}
