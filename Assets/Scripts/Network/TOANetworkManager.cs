using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;

public class TOANetworkManager : NetworkManager
{
    [SerializeField] private string startingSceneName = "Scene Not Set";
    [SerializeField] private string mainMenuScene = "Main_Menu";

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    public List<TOANetworkPlayer> Players { get; } = new List<TOANetworkPlayer>();

    public string GetStartingSceneToLoad() { return startingSceneName; }
    public void SetStartingSceneToLoad(string newScene) { startingSceneName = newScene; }

    #region Server

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        TOANetworkPlayer player = conn.identity.GetComponent<TOANetworkPlayer>();
        Players.Remove(player);
        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        Players.Clear();

    }

    public void StartGame()
    {
        Debug.Log(NetworkManager.singleton.networkAddress);
        ServerChangeScene(startingSceneName);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        TOANetworkPlayer player = conn.identity.GetComponent<TOANetworkPlayer>();

        Players.Add(player);

        //TODO:: Add steam name here
        player.SetDisplayName($"Player {Players.Count}");

    }

    public override void OnServerChangeScene(string newSceneName)
    {
        //TODO:: Save player data

        base.OnServerChangeScene(newSceneName);
        
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith(mainMenuScene)) return;

        foreach(TOANetworkPlayer player in Players)
        {
            //Todo:: Read player data and create a character to spawn in
//             GameObject playerInstance = Instantiate(playerCharacter, GetStartPosition().position, Quaternion.identity);
//             NetworkServer.Spawn(playerCharacter, player.connectionToClient);
        }
    }

    #endregion

    #region Client

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        ClientOnDisconnected?.Invoke();
    }

    public override void OnStopClient()
    {
        Players.Clear();
    }

    #endregion
}
