using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerListUI : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> orangeTeamTextList;
    [SerializeField] private List<TextMeshProUGUI> blueTeamTextList;

    private void Awake()
    {
        SoccerGameMultiplayer.Instance.OnPlayerDataNetworkListChanged += UpdatePlayerListUI;
    }

    private void Start()
    {
        UpdatePlayerListUI(this, null);
    }

    private void OnDestroy()
    {
        SoccerGameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= UpdatePlayerListUI;
    }

    private void UpdatePlayerListUI(object sender, System.EventArgs e)
    {
        NetworkList<PlayerData> playerDataNetworkList = SoccerGameMultiplayer.Instance.GetPlayerDataNetworkList();

        // Clear all player slots
        ClearAllPlayerSlots();

        // Update player slots based on player data
        foreach (PlayerData playerData in playerDataNetworkList)
        {
            int colorId = playerData.colorId;

            if (colorId >= 0 && colorId < orangeTeamTextList.Count)
            {
                // Update orange team slots
                orangeTeamTextList[colorId].text = $"{colorId + 1}. {playerData.playerName}";
            }
            else if (colorId >= orangeTeamTextList.Count && colorId < orangeTeamTextList.Count + blueTeamTextList.Count)
            {
                // Update blue team slots
                blueTeamTextList[colorId - orangeTeamTextList.Count].text = $"{(colorId%6)+1}. {playerData.playerName}";
            }
        }
    }

    private void ClearAllPlayerSlots()
    {
        foreach (var text in orangeTeamTextList)
        {
            text.text = "  ";
        }
        foreach (var text in blueTeamTextList)
        {
            text.text = "  ";
        }
    }
}
