using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChatServer : NetworkBehaviour
{    
    [ClientRpc]
    public void SendChatMessageClientRpc(string message, ulong from, ClientRpcParams clientRpcParams = default)
    {
        string toDisplay = $"[{from}]{message}";
        Debug.Log(toDisplay);
    }


    [ServerRpc(RequireOwnership = false)]
    public void RequestSendMessageServerRpc(string message, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log($"Host got message: {message}");
        SendChatMessageClientRpc(message, serverRpcParams.Receive.SenderClientId);
    }
}
