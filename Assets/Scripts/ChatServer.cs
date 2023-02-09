using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChatServer : MonoBehaviour
{
    [ClientRpc]
    public void SendChatMessageClientRpc(string message, ClientRpcParams clientRpcParams = default)
    {

    }

    [ServerRpc]
    public void SendChatMessageServerRpc(string message, ServerRpcParams serverRpcParams = default)
    {

    }
}
