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

    [ServerRpc (RequireOwnership = false)]
    public void RequestSendMessageServerRpc(string message, ServerRpcParams serverRpcParams = default)
    {
        SendChatMessageClientRpc(message, serverRpcParams.Receive.SenderClientId);
    }
    [ServerRpc]
    public void SendSystemMessageServerRpc(string msg, ulong clientId)
    {
        It4080.Chat chat = new It4080.Chat();
        chat.SystemMessage(msg);
    }
}
