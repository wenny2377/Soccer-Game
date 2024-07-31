using UnityEngine;
using Unity.Netcode;

public class SoccerBall : NetworkBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    
    private Rigidbody rb;
    private NetworkVariable<int> lastPlayerColorId = new NetworkVariable<int>(-1);
    private NetworkVariable<Vector3> networkedVelocity = new NetworkVariable<Vector3>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
       
        if (IsServer)
        {

            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }

            
            networkedVelocity.Value = rb.velocity;
        }
        else
        {
           
            rb.velocity = networkedVelocity.Value;
        }
    }

    public void OnPlayerTouch(Vector3 direction, float force, int playerColorId)
    {
        if (IsServer)
        {
            rb.AddForce(direction * force, ForceMode.Impulse);
            lastPlayerColorId.Value = playerColorId; // Update last player to touch the ball
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (other.CompareTag("OrangeGate"))
        {
            if (IsOrangeTeam(lastPlayerColorId.Value))
            {
                KickManager.Instance.GoalScored(1);
            }
        }
        else if (other.CompareTag("BlueGate"))
        {
            if (IsBlueTeam(lastPlayerColorId.Value))
            {
                KickManager.Instance.GoalScored(0);
            }
        }
    }

    private bool IsOrangeTeam(int colorId)
    {
        return colorId < 6;
    }

    private bool IsBlueTeam(int colorId)
    {
        return colorId >= 6;
    }
}
