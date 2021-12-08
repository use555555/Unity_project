using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;


public class Equip : NetworkBehaviour
{
    [SerializeField]
    private Transform rightHandSlot;
    [SerializeField]
    //public GameObject[] Player;
    public GameObject[] Gun;
    private GameObject _equippedItem;
    public int Equipped_id;
    public int equip = 0;
    public int selected = -1;
    // Start is called before the first frame update
    private void Start()
    {
        _equippedItem = null;
        selected = -1;
    }

    [ServerRpc]
    private void EquipServerRpc(ulong netID, int Gun_type)
    {
        GameObject go = Instantiate(Gun[Gun_type]);
        go.GetComponent<NetworkObject>().SpawnWithOwnership(netID);
        ulong itemNetID = go.GetComponent<NetworkObject>().NetworkObjectId;

        EquipClientRpc(itemNetID, Gun_type);
    } 

    [ClientRpc]
    private void EquipClientRpc(ulong itemNetID, int Gun_type)
    {
        NetworkObject netObj = NetworkSpawnManager.SpawnedObjects[itemNetID];

        _equippedItem = netObj.gameObject;
        Rigidbody rB = _equippedItem.GetComponent<Rigidbody>();
        rB.isKinematic = true;
        rB.useGravity = false;
        _equippedItem.transform.position = rightHandSlot.position;
        _equippedItem.transform.rotation = rightHandSlot.rotation;
        _equippedItem.transform.SetParent(rightHandSlot);

    }

    // Update is called once per frame
    private void Update()
    {
        //Player = GameObject.FindGameObjectsWithTag("Player");
        if (IsLocalPlayer)
        {
            if (equip == 0 && selected != -1)
            {
                EquipServerRpc(NetworkManager.Singleton.LocalClientId, selected);
                equip = 1;
            }
        }
    }
}
