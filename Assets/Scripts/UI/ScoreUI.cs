using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI orangeTeamScoreText;
    [SerializeField] private TextMeshProUGUI blueTeamScoreText;

    private void Start() {
        KickManager.Instance.OnScoreUpdated += KickManager_OnScoreUpdated;
        UpdateScoreUI();
    }

    private void KickManager_OnScoreUpdated(object sender, EventArgs e) {
        UpdateScoreUI();
    }

    
    private void UpdateScoreUI() {
       orangeTeamScoreText.text = KickManager.Instance.GetOrangeTeamScore().ToString();
       blueTeamScoreText.text = KickManager.Instance.GetBlueTeamScore().ToString();
    }
}

