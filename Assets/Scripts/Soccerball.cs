using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SoccerBall : NetworkBehaviour
{
    [SerializeField] private float ballRadius = 0.5f;
    [SerializeField] private float ballMass = 1f;
    [SerializeField] private List<Vector3> spawnPositionList;

    private Rigidbody rb;

    private void Awake()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = ballMass;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.radius = ballRadius;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            TransformToSpawnPosition();
        }
    }

    private void TransformToSpawnPosition()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPositionList.Count);
        transform.position = spawnPositionList[randomIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            if (IsServer)
            {
                int teamColorId = other.gameObject.GetComponent<Goal>().TeamColorId;
                ShootManager.Instance.GoalScored(teamColorId);
            }
        }
    }
}
