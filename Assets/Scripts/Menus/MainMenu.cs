using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private bool useSteam = false;
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private HostLobbyMenu hostLobbyMenu = null;
    [SerializeField] private JoinServer joinServerObject = null;

    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;

    private void Start()
    {
        joinServerObject.SetUseSteam(useSteam);

        if (useSteam)
        {
            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            return;
        }

    }

    public void UpdateNetworkManager()
    {
        NetworkManager.singleton.maxConnections = hostLobbyMenu.GetNumPlayers();
        NetworkManager.singleton.networkAddress = hostLobbyMenu.GetServerName();
    }

    public void HostLobby()
    {
        landingPagePanel.SetActive(false);

        if(useSteam)
        {
            SteamMatchmaking.CreateLobby(hostLobbyMenu.GetLobbyType(), hostLobbyMenu.GetNumPlayers());
            return;
        }

        NetworkManager.singleton.StartHost();
    }

    #region Steamworks

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) return;

        NetworkManager.singleton.StartHost();
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "LobbyName", hostLobbyMenu.GetServerName());

        string lobbyType ="Unknown";
        switch (hostLobbyMenu.GetLobbyType())
        {
            case ELobbyType.k_ELobbyTypePrivate:
                lobbyType = "Private";
                break;
            case ELobbyType.k_ELobbyTypeFriendsOnly:
                lobbyType = "Friends Only";
                break;
            case ELobbyType.k_ELobbyTypePublic:
                lobbyType = "Public";
                break;
            default:
                break;
        }
        SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "LobbyType", lobbyType);

    }

    private void OnLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) return;

        string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress");

        NetworkManager.singleton.networkAddress = hostAddress;
        NetworkManager.singleton.StartClient();

        landingPagePanel.SetActive(false);
    }
    #endregion

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

}
