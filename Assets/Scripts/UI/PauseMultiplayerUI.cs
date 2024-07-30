using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMultiplayerUI : MonoBehaviour {



    private void Start() {
        SoccerGameManager.Instance.OnMultiplayerGamePaused += SoccerGameManager_OnMultiplayerGamePaused;
        SoccerGameManager.Instance.OnMultiplayerGameUnpaused += SoccerGameManager_OnMultiplayerGameUnpaused;

        Hide();
    }

    private void SoccerGameManager_OnMultiplayerGameUnpaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void SoccerGameManager_OnMultiplayerGamePaused(object sender, System.EventArgs e) {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}