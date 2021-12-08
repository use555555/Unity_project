using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using UnityEngine.UI;
public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariableFloat health = new NetworkVariableFloat(100f);
    MeshRenderer[] renderers;
    CharacterController cc;
    private void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        cc = GetComponent<CharacterController>();
    }
    public void TakeDamange(float damage)
    {
        health.Value -= damage;
        // check health
        if (health.Value <= 0)
        {;
            // respwan
            Vector3 pos = new Vector3(0f, 25f, 0f);
            ClientRespawnClientRpc(pos);
            GameController.Death += 1;

        }
    }
    public void Heal()
    {
        health.Value = 100f;
        Vector3 pos = new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10));
        ClientRespawnClientRpc(pos);
    }

    [ClientRpc]
    void ClientRespawnClientRpc(Vector3 position)
    {
        StartCoroutine(Respawn(position));
    }
    IEnumerator Respawn(Vector3 position)
    {
        foreach (var renderer in renderers)
        {
            renderer.enabled = false;
        }
        transform.position = position;
        cc.enabled = false;
        yield return new WaitForSeconds(1f);
        cc.enabled = true;
        foreach (var renderer in renderers)
        {
            renderer.enabled = true;
        }
    }
}
