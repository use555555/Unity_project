using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
public class PlayerShooting : NetworkBehaviour
{
    public GameObject RightArm;
    public ParticleSystem bulletParticleSystem;
    private ParticleSystem.EmissionModule em;
    NetworkVariableBool shooting = new NetworkVariableBool(new NetworkVariableSettings{WritePermission =NetworkVariablePermission.OwnerOnly}, false);
    float shootTimer = 0f;
    float fireRate = 0;
    float damage = 0;
    public AudioClip guns;
    // Start is called before the first frame update
    void Start()
    {
        em = bulletParticleSystem.emission;
        em.rateOverTime = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        var gun = RightArm.GetComponentInChildren<GunDetail>();
        if (gun != null)
        {
            fireRate = gun.fireRate*gun.spdmultiplier;
            damage = gun.damage*gun.dmgmultiplier;
        }
        if (IsLocalPlayer)
        {
            shooting.Value = Input.GetMouseButton(0); // left button
            shootTimer += Time.deltaTime;
            if (shooting.Value && shootTimer >= fireRate)
            {
                shootTimer = 0;
                //bulletParticleSystem.Emit(1);
                ShootServerRpc(damage);
            }

        }
    }
    [ServerRpc]
    void ShootServerRpc(float damage)
    {
        //bulletParticleSystem.Emit(1);
        Ray ray = new Ray(bulletParticleSystem.transform.position, bulletParticleSystem.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            //we hit something
            var enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamange(damage);
            }
        }
        ShootClientRpc();
    }
    [ClientRpc]
    void ShootClientRpc()
    {
        bulletParticleSystem.Emit(1);
        gameObject.GetComponent<AudioSource>().PlayOneShot(guns);
    }
}
