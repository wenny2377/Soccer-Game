using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ShootManager : NetworkBehaviour
{
    public static ShootManager Instance { get; private set; }

    public event EventHandler OnScoreUpdated;

    [SerializeField] private Transform soccerBallPrefab;
    [SerializeField] private Transform ballSpawnPoint;
    [SerializeField] private float shootForce = 20f;
    [SerializeField] private float shootCooldown = 0.5f;

    private List<Transform> soccerBallsList;
    private float lastShootTime;
    private NetworkVariable<int> orangeTeamScore = new NetworkVariable<int>(0);
    private NetworkVariable<int> blueTeamScore = new NetworkVariable<int>(0);

    private void Awake()
    {
        Instance = this;
        soccerBallsList = new List<Transform>();
    }

    private void Start()
    {
        if (IsOwner)
        {
            GameInput.Instance.OnShootAction += GameInput_OnShootAction;
        }
    }

    private void GameInput_OnShootAction(object sender, EventArgs e)
    {
        if (Time.time - lastShootTime >= shootCooldown)
        {
            lastShootTime = Time.time;
            ShootBallServerRpc();
        }
    }

    [ServerRpc]
    private void ShootBallServerRpc()
    {
        Transform soccerBallTransform = Instantiate(soccerBallPrefab, ballSpawnPoint.position, Quaternion.identity);
        soccerBallTransform.GetComponent<NetworkObject>().Spawn(true);
        soccerBallsList.Add(soccerBallTransform);
        ShootBallClientRpc(soccerBallTransform.GetComponent<NetworkObject>().NetworkObjectId);
    }

    [ClientRpc]
    private void ShootBallClientRpc(ulong ballNetworkObjectId)
    {
        NetworkObject networkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[ballNetworkObjectId];
        Rigidbody ballRigidbody = networkObject.GetComponent<Rigidbody>();
        Vector3 shootDirection = (networkObject.transform.position - ballSpawnPoint.position).normalized;
        ballRigidbody.AddForce(shootDirection * shootForce, ForceMode.Impulse);
    }

    public void GoalScored(int teamColorId)
    {
        if (teamColorId == 0)
        {
            blueTeamScore.Value++;
        }
        else if (teamColorId == 1)
        {
            orangeTeamScore.Value++;
        }
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
