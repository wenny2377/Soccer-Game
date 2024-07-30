using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour {


    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    


    private void Awake() {
        resumeButton.onClick.AddListener(() => {
            SoccerGameManager.Instance.TogglePauseGame();
        });
        mainMenuButton.onClick.AddListener(() => {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        
    }

    private void Start() {
        SoccerGameManager.Instance.OnLocalGamePaused += SoccerGameManager_OnLocalGamePaused;
        SoccerGameManager.Instance.OnLocalGameUnpaused += SoccerGameManager_OnLocalGameUnpaused;

        Hide();
    }

    private void SoccerGameManager_OnLocalGameUnpaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void SoccerGameManager_OnLocalGamePaused(object sender, System.EventArgs e) {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);

        resumeButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}