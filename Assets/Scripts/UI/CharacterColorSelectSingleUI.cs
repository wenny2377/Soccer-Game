using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectSingleUI : MonoBehaviour {


    [SerializeField] private int colorId;
    [SerializeField] private Image image;
    [SerializeField] private GameObject selectedGameObject;


    private void Awake() {
        GetComponent<Button>().onClick.AddListener(() => {
            SoccerGameMultiplayer.Instance.ChangePlayerColor(colorId);
        });
    }

    private void Start() {
        SoccerGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += SoccerGameMultiplayer_OnPlayerDataNetworkListChanged;
        image.color = SoccerGameMultiplayer.Instance.GetPlayerColor(colorId);
        UpdateIsSelected();
    }

    private void SoccerGameMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e) {
        UpdateIsSelected();
    }

    private void UpdateIsSelected() {
        if (SoccerGameMultiplayer.Instance.GetPlayerData().colorId == colorId) {
            selectedGameObject.SetActive(true);
        } else {
            selectedGameObject.SetActive(false);
        }
    }

    private void OnDestroy() {
        SoccerGameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= SoccerGameMultiplayer_OnPlayerDataNetworkListChanged;
    }
}