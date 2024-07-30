using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;


    private void Awake() {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start() {
        SoccerGameMultiplayer.Instance.OnFailedToJoinGame += SoccerGameMultiplayer_OnFailedToJoinGame;
        SoccerGameLobby.Instance.OnCreateLobbyStarted += SoccerGameLobby_OnCreateLobbyStarted;
        SoccerGameLobby.Instance.OnCreateLobbyFailed += SoccerGameLobby_OnCreateLobbyFailed;
        SoccerGameLobby.Instance.OnJoinStarted += SoccerGameLobby_OnJoinStarted;
        SoccerGameLobby.Instance.OnJoinFailed += SoccerGameLobby_OnJoinFailed;
        SoccerGameLobby.Instance.OnQuickJoinFailed += SoccerGameLobby_OnQuickJoinFailed;

        Hide();
    }

    private void SoccerGameLobby_OnQuickJoinFailed(object sender, System.EventArgs e) {
        ShowMessage("Could not find a Lobby to Quick Join!");
    }

    private void SoccerGameLobby_OnJoinFailed(object sender, System.EventArgs e) {
        ShowMessage("Failed to join Lobby!");
    }

    private void SoccerGameLobby_OnJoinStarted(object sender, System.EventArgs e) {
        ShowMessage("Joining Lobby...");
    }

    private void SoccerGameLobby_OnCreateLobbyFailed(object sender, System.EventArgs e) {
        ShowMessage("Failed to create Lobby!");
    }

    private void SoccerGameLobby_OnCreateLobbyStarted(object sender, System.EventArgs e) {
        ShowMessage("Creating Lobby...");
    }

    private void SoccerGameMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e) {
        if (NetworkManager.Singleton.DisconnectReason == "") {
            ShowMessage("Failed to connect");
        } else {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message) {
        Show();
        messageText.text = message;
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        SoccerGameMultiplayer.Instance.OnFailedToJoinGame -= SoccerGameMultiplayer_OnFailedToJoinGame;
        SoccerGameLobby.Instance.OnCreateLobbyStarted -= SoccerGameLobby_OnCreateLobbyStarted;
        SoccerGameLobby.Instance.OnCreateLobbyFailed -= SoccerGameLobby_OnCreateLobbyFailed;
        SoccerGameLobby.Instance.OnJoinStarted -= SoccerGameLobby_OnJoinStarted;
        SoccerGameLobby.Instance.OnJoinFailed -= SoccerGameLobby_OnJoinFailed;
        SoccerGameLobby.Instance.OnQuickJoinFailed -= SoccerGameLobby_OnQuickJoinFailed;
    }

}