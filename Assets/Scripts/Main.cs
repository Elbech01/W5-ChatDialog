using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;

public class Main : NetworkBehaviour
{
    public It4080.NetworkSettings netSettings;
    public It4080.Chat chat;
    public ChatServer chatServer;
    private Button btnStart;
    


    // Start is called before the first frame update
    void Start(){
        netSettings.startServer += NetSettingsOnServerStart;
        netSettings.startHost += NetSettingsOnHostStart;
        netSettings.startClient += NetSettingsOnClientStart;
        netSettings.setStatusText("Not Connected");

        //chat.SendMessage += ChatOnSendMessage;

        btnStart = GameObject.Find("btnStart").GetComponent<Button>();
        btnStart.onClick.AddListener(btnStartOnClick);
    }

    private void ChatOnSendMessage(It4080.Chat.ChatMessage msg)
    {
        chatServer.RequestSendMessageServerRpc(msg.message);
    }
    private void startClient(IPAddress ip, ushort port) {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.OnClientConnectedCallback += ClientOnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += ClientOnClientDisconnected;

        NetworkManager.Singleton.StartClient();
        netSettings.hide();
        Debug.Log("Started client");
    }

    
    private void startHost(IPAddress ip, ushort port) {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HostOnClientDisconnected;

        NetworkManager.Singleton.StartHost();
        netSettings.hide();
        Debug.Log("Started host");
    }

    private void startServer(IPAddress ip, ushort port) {
        var utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.ConnectionData.Address = ip.ToString();
        utp.ConnectionData.Port = port;

        NetworkManager.Singleton.OnClientConnectedCallback += HostOnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HostOnClientDisconnected;

        NetworkManager.Singleton.StartServer();
        netSettings.hide();
        Debug.Log("Started server");
        printIs("startServer");
    }



    // ------------------------------------
    // Events

    private void btnStartOnClick()
    {
        StartGame();
    }

    private void StartGame()
    {
        NetworkManager.SceneManager.LoadScene("Arena1",
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    private void NetSettingsOnServerStart(IPAddress ip, ushort port) {
        startServer(ip, port);
        Debug.Log("Server started");
        printIs("");
    }

    private void NetSettingsOnHostStart(IPAddress ip, ushort port) {
        startHost(ip, port);
        Debug.Log("Host started");
        printIs("");
    }

    private void NetSettingsOnClientStart(IPAddress ip, ushort port) {
        startClient(ip, port);
        Debug.Log("Client started");
        printIs("");
    }

    private void printIs(string msg) {
        Debug.Log($"[{msg}] {MakeIsString()}");
    }
    private string MakeIsString()
    {
        return $"server:{IsServer} host:{IsHost} client:{IsClient} owner:{IsOwner}";
    }

    private void HostOnClientConnected(ulong clientID) {
        Debug.Log($"Client connected to me: {clientID}");
        chat.SystemMessage($"{clientID} connected to server.");
    }

    private void HostOnClientDisconnected(ulong clientID)
    {
        Debug.Log($"Client disconnected from me: {clientID}");
    }

    private void ClientOnClientConnected(ulong clientID) {
        Debug.Log($"Client connected: {clientID}");
        chat.SystemMessage($"{clientID} connected to server.");
    }

    private void ClientOnClientDisconnected(ulong clientID) {
        Debug.Log($"Client disconnected: {clientID}");
    }


}
