using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {


    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    [SerializeField] private Button playAgainButton;


    private void Awake() {
        playAgainButton.onClick.AddListener(() => {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void Start() {
        SoccerGameManager.Instance.OnStateChanged += SoccerGameManager_OnStateChanged;

        Hide();
    }

    private void SoccerGameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (SoccerGameManager.Instance.IsGameOver()) {
            Show();

            
        } else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
        playAgainButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }


}