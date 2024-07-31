using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KickManager : NetworkBehaviour
{
    public static KickManager Instance { get; private set; }

    public event EventHandler OnGoalScored;
    public event EventHandler OnScoreUpdated;
    public event EventHandler OnPlayerJoined;

    [SerializeField] private Transform soccerBallPrefab;

    private NetworkVariable<int> orangeTeamScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<int> blueTeamScore = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private Dictionary<ulong, int> playerTeams;

    private void Awake()
    {
        Instance = this;
        playerTeams = new Dictionary<ulong, int>();
    }

    private void OnEnable()
    {
        orangeTeamScore.OnValueChanged += OnScoreChanged;
        blueTeamScore.OnValueChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        orangeTeamScore.OnValueChanged -= OnScoreChanged;
        blueTeamScore.OnValueChanged -= OnScoreChanged;
    }

    public void RegisterPlayer(ulong clientId, int colorId)
    {
        int teamColorId = colorId < 6 ? 0 : 1;
        playerTeams[clientId] = teamColorId;
        OnPlayerJoined?.Invoke(this, EventArgs.Empty);
    }

    public int GetPlayerTeam(ulong clientId)
    {
        return playerTeams.ContainsKey(clientId) ? playerTeams[clientId] : -1;
    }

    public void GoalScored(int scoringTeamColorId)
    {
        if (IsServer)
        {
            if (scoringTeamColorId == 1)
            {
                orangeTeamScore.Value++;
            }
            else if (scoringTeamColorId == 0)
            {
                blueTeamScore.Value++;
            }

            OnGoalScored?.Invoke(this, EventArgs.Empty);
            OnScoreUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnScoreChanged(int oldScore, int newScore)
    {
        OnScoreUpdated?.Invoke(this, EventArgs.Empty);
    }

    public int GetOrangeTeamScore()
    {
        return orangeTeamScore.Value;
    }

    public int GetBlueTeamScore()
    {
        return blueTeamScore.Value;
    }
}
