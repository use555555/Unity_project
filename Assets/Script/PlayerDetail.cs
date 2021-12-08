using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class PlayerDetail : NetworkBehaviour
{
    public NetworkVariableFloat Money = new NetworkVariableFloat(0f);
    public NetworkVariableFloat Point = new NetworkVariableFloat(0f);
}
