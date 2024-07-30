using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour {


    [SerializeField] private int playerIndex;
    [SerializeField] private GameObject readyGameObject;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private Button kickButton;
    [SerializeField] private TextMeshPro playerNameText;


    private void Awake() {
        kickButton.onClick.AddListener(() => {
            PlayerData playerData = SoccerGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            SoccerGameLobby.Instance.KickPlayer(playerData.playerId.ToString());
            //this is a test
            SoccerGameMultiplayer.Instance.KickPlayer(playerData.clientId);
        });
    }

    private void Start() {
        SoccerGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += SoccerGameMultiplayer_OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;

        kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);

        UpdatePlayer();
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, System.EventArgs e) {
        UpdatePlayer();
    }

    private void SoccerGameMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e) {
        UpdatePlayer();
    }

    private void UpdatePlayer() {
        if (SoccerGameMultiplayer.Instance.IsPlayerIndexConnected(playerIndex)) {
            Show();

            PlayerData playerData = SoccerGameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(playerIndex);

            readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));

            playerNameText.text = playerData.playerName.ToString();

            playerVisual.SetPlayerColor(SoccerGameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        SoccerGameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= SoccerGameMultiplayer_OnPlayerDataNetworkListChanged;
    }


}