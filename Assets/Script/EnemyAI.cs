using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : NetworkBehaviour
{
    float dist_ref;
    int index_ref;
    NavMeshAgent nm;
    public GameObject[] Targets;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
        StartCoroutine(Think());
    }

    // Update is called once per frame
    void Update()
    {
        Targets = GameObject.FindGameObjectsWithTag("Player");
    }

    IEnumerator Think()
    {
        while(true)
        {
            dist_ref = 0;
            index_ref = 0;
            if (Targets.Length > 0)
            {
                dist_ref = Vector3.Distance(Targets[0].transform.position, transform.position);
                index_ref = 0;
                if (Targets.Length > 1)
                {
                    for (int i = 1; i < Targets.Length; i = i + 1)
                    {
                        {
                            if (Vector3.Distance(Targets[i].transform.position, transform.position) < dist_ref)
                            {
                                dist_ref = Vector3.Distance(Targets[i].transform.position, transform.position);
                                index_ref = i;
                            }
                        }
                    }
                }
                target = Targets[index_ref].transform;
                nm.SetDestination(target.position);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
