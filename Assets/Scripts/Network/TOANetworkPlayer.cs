using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class TOANetworkPlayer : NetworkBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [SyncVar(hook = nameof(ClientHandleDisplayNameUpdated))]
    private string displayName;



    public Transform GetCameraTransform() { return cameraTransform; }
    public string GetDisplayName() { return displayName; }

    #region Server
    public override void OnStartServer()
    {


        DontDestroyOnLoad(gameObject);
    }

    public override void OnStopServer()
    {

    }

    [Server]
    public void SetDisplayName(string newDisplayName) { displayName = newDisplayName; }

    [Command]
    public void CmdStartGame()
    {
        ((TOANetworkManager)NetworkManager.singleton).StartGame();
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        //don't do anything if this instance is the server
        if (NetworkServer.active) return;

    }

    public override void OnStartClient()
    {
        //don't do anything if this instance is the server
        if (NetworkServer.active) return;

        DontDestroyOnLoad(gameObject);

        ((TOANetworkManager)NetworkManager.singleton).Players.Add(this);
    }

    public override void OnStopClient()
    {

        if (!isClientOnly) return;

        ((TOANetworkManager)NetworkManager.singleton).Players.Remove(this);

        if (!hasAuthority) return;

    }

    private void ClientHandleDisplayNameUpdated(string oldDisplayName, string newDisplayName)
    {

    }

    #endregion
}
