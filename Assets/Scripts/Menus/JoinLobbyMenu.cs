using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPagePanel = null;
    [SerializeField] private TMP_InputField ipInputField = null;
    [SerializeField] private Button joinButton = null;

    private void OnEnable()
    {
        TOANetworkManager.ClientOnConnected += HandleClientConnected;
        TOANetworkManager.ClientOnDisconnected += HandleClientDisconnected;
    }

    private void OnDisable()
    {
        TOANetworkManager.ClientOnConnected -= HandleClientConnected;
        TOANetworkManager.ClientOnDisconnected -= HandleClientDisconnected;
    }

    public void JoinIPAddress()
    {
        string address = ipInputField.text;

        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();

        joinButton.interactable = false;
    }

    public void CancelJoinAttempt()
    {
        NetworkManager.singleton.StopClient();

        joinButton.interactable = true;
    }

    private void HandleClientConnected()
    {
        joinButton.interactable = true;
        gameObject.SetActive(false);
        landingPagePanel.SetActive(false);
    }

    private void HandleClientDisconnected()
    {
        joinButton.interactable = true;
    }
    
}
