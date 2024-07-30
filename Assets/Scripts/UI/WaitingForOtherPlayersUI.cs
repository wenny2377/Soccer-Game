using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour {


    private void Start() {
        SoccerGameManager.Instance.OnLocalPlayerReadyChanged += KitchenGameManager_OnLocalPlayerReadyChanged;
        SoccerGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        Hide();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (SoccerGameManager.Instance.IsCountdownToStartActive()) {
            Hide();
        }
    }

    private void KitchenGameManager_OnLocalPlayerReadyChanged(object sender, System.EventArgs e) {
        if (SoccerGameManager.Instance.IsLocalPlayerReady()) {
            Show();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}