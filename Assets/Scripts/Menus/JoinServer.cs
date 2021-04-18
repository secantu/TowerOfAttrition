using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System;
using UnityEngine.UI;


public class JoinServer : MonoBehaviour
{
    [SerializeField] private Button refreshButton = null;
    [SerializeField] private ScrollBox scrollViewContent = null;
    [SerializeField] private Canvas joinServerInstancePrefab = null;
    [SerializeField] private bool useSteam = false;


    ulong current_lobbyID;
    List<CSteamID> lobbyIDS;

    protected Callback<LobbyMatchList_t> Callback_lobbyList;
    protected Callback<LobbyDataUpdate_t> Callback_lobbyInfo;

    public void SetUseSteam(bool state) { useSteam = state; }

    private void Start()
    {
        if (useSteam)
        {
            Callback_lobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbiesList);
            Callback_lobbyInfo = Callback<LobbyDataUpdate_t>.Create(OnLobbyInfoUpdated);
        }
    }

    private void OnEnable()
    {
        if (useSteam)
        {
            lobbyIDS = new List<CSteamID>();
            FindLobbies();
        }
    }

    private void OnDisable()
    {
        if (useSteam)
        {
            lobbyIDS.Clear();
        }
            scrollViewContent.ClearScrollBox();
    }

    public void FindLobbies()
    {
        Debug.Log("searching for lobbies!");
        scrollViewContent.ClearScrollBox();

        refreshButton.interactable = false;
        if (useSteam)
        {
            lobbyIDS.Clear();
            SteamAPICall_t lobbyList = SteamMatchmaking.RequestLobbyList();
            return;
        }
        UpdateLobbyList();
    }

    public void UpdateLobbyList()
    {

    }

    #region SteamWorks

    public void OnGetLobbiesList(LobbyMatchList_t result)
    {
        Debug.Log($"Found {result.m_nLobbiesMatching} lobbies!");

        refreshButton.interactable = true;

        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbyIDS.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID);
        }
    }

    private void OnLobbyInfoUpdated(LobbyDataUpdate_t result)
    {
        for (int i = 0; i < lobbyIDS.Count; i++)
        {
            if (lobbyIDS[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                Debug.Log("Lobby " + i + " :: " + SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDS[i].m_SteamID, "name"));

                Canvas joinServerInstance = Instantiate(joinServerInstancePrefab);
                JoinServerInstance jsi = joinServerInstance.GetComponent<JoinServerInstance>();
                jsi.SetLobbyName(SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDS[i].m_SteamID, "LobbyName"));
                jsi.SetCurrentPlayers(SteamMatchmaking.GetNumLobbyMembers((CSteamID)current_lobbyID));
                jsi.SetMaxPlayers(SteamMatchmaking.GetLobbyMemberLimit((CSteamID)current_lobbyID));
                jsi.SetLobbyType(SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDS[i].m_SteamID, "LobbyType"));

                scrollViewContent.AddToScrollBox(joinServerInstance.gameObject);
            }
        }
    }
    #endregion
}
