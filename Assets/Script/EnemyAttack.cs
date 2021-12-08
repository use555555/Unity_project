using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class EnemyAttack : NetworkBehaviour
{
    public Component attackpoint;
    float attackRater = 1f;
    float attackTimer = 0f;
    public AudioClip zattack;
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
            AttackClientRpc();
        }
    }
    [ClientRpc]
    void AttackClientRpc()
    {
        Ray ray = new Ray(attackpoint.transform.position, attackpoint.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 1.5f))
        {
            //we hit something
            var player = hit.collider.GetComponent<PlayerHealth>();
            if (player != null)
            {
                //we hit a player
                gameObject.GetComponent<AudioSource>().PlayOneShot(zattack);
                player.TakeDamange(30f);
            }
        }
    }
}
