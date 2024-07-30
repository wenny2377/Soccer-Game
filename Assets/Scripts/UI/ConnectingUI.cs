using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour {



    private void Start() {
        SoccerGameMultiplayer.Instance.OnTryingToJoinGame += SoccerGameMultiplayer_OnTryingToJoinGame;
        SoccerGameMultiplayer.Instance.OnFailedToJoinGame += SoccerGameManager_OnFailedToJoinGame;

        Hide();
    }

    private void SoccerGameManager_OnFailedToJoinGame(object sender, System.EventArgs e) {
        Hide();
    }

    private void SoccerGameMultiplayer_OnTryingToJoinGame(object sender, System.EventArgs e) {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        SoccerGameMultiplayer.Instance.OnTryingToJoinGame -= SoccerGameMultiplayer_OnTryingToJoinGame;
        SoccerGameMultiplayer.Instance.OnFailedToJoinGame -= SoccerGameManager_OnFailedToJoinGame;
    }

}