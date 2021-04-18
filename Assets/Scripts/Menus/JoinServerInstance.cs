using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinServerInstance : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyName = null;
    [SerializeField] private TMP_Text playerRatio = null;
    [SerializeField] private TMP_Text lobbyType = null;
    [SerializeField] private Button joinButton = null;
    [SerializeField] private int currentPlayers = 0;
    [SerializeField] private int maxPlayers = 0;

    public void SetLobbyName(string newName) { lobbyName.text = newName; }
    public void SetLobbyType(string newLobbyType) { lobbyType.text = newLobbyType; }
    public void SetCurrentPlayers(int newCurrentPlayers) 
    { 
        currentPlayers = newCurrentPlayers;
        UpdatePlayerRatio();
    }
    public void SetMaxPlayers(int newMaxPlayers) 
    { 
        maxPlayers = newMaxPlayers;
        UpdatePlayerRatio();
    }

    private void UpdatePlayerRatio()
    {
        playerRatio.text = $"{currentPlayers}/{maxPlayers}";
    }

    public void TryJoinServer()
    {
        string address = lobbyName.text;

        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();

        joinButton.interactable = false;
    }
}
