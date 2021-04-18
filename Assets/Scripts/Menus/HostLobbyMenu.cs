using System.Collections;
using System.Collections.Generic;
using TMPro;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class HostLobbyMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField serverNameInputField = null;
    [SerializeField] private TMP_Dropdown numPlayersDropdown = null;
    [SerializeField] private TMP_Dropdown lobbyTypeDropdown = null;
    [SerializeField] private Button startGameButton = null; 

    private string serverName = "Server Not Set";
    private int numPlayers = 1;
    private ELobbyType lobbyType = ELobbyType.k_ELobbyTypePrivate;

    public string GetServerName() { return serverName; }
    public int GetNumPlayers() { return numPlayers; }
    public ELobbyType GetLobbyType() { return lobbyType; }

    public void UpdateServerName()
    {
        serverName = serverNameInputField.text.ToString();
        if(serverName.Length > 1 && serverName != "Server Not Set")
        {
            startGameButton.interactable = true;
            return;
        }
        startGameButton.interactable = false;
    }

    public void UpdateNumPlayers()
    {
        numPlayers = numPlayersDropdown.value + 1; 
    }

    public void UpdateLobbyType()
    {
        switch (lobbyTypeDropdown.value)
        {
            case 0:
                lobbyType = ELobbyType.k_ELobbyTypePrivate;
                break;
            case 1:
                lobbyType = ELobbyType.k_ELobbyTypeFriendsOnly;
                break;
            case 2:
                lobbyType = ELobbyType.k_ELobbyTypePublic;
                break;
            default:
                break;
        }
    }

}
