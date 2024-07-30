using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI orangeTeamScoreText;
    [SerializeField] private TextMeshProUGUI blueTeamScoreText;

    private void Start() {
        ShootManager.Instance.OnScoreUpdated += ShootManager_OnScoreUpdated;
        UpdateScoreUI();
    }

    private void ShootManager_OnScoreUpdated(object sender, EventArgs e) {
        UpdateScoreUI();
    }

    
    private void UpdateScoreUI() {
       orangeTeamScoreText.text = ShootManager.Instance.GetOrangeTeamScore().ToString();
       blueTeamScoreText.text = ShootManager.Instance.GetBlueTeamScore().ToString();
    }
}

