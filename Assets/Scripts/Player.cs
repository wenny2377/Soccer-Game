using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour {


    public static event EventHandler OnAnyPlayerSpawned;
    

    [SerializeField] private GameObject ballPrefab; // 球体预制件
    [SerializeField] private float kickForce = 10f; // 踢球的力


    public static void ResetStaticData() {
        OnAnyPlayerSpawned = null;
    }


    public static Player LocalInstance { get; private set; }



         
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private LayerMask collisionsLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private List<Vector3> spawnPositionList;
    [SerializeField] private PlayerVisual playerVisual;


    private bool isWalking;
    private Vector3 lastInteractDir;
    
    private void Start() {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        GameInput.Instance.OnShootAction += GameInput_OnShootAction;

        PlayerData playerData = SoccerGameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(SoccerGameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
    }

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            LocalInstance = this;
        }

        transform.position = spawnPositionList[SoccerGameMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];

        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

        if (IsServer) {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId) {
       
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if (!SoccerGameManager.Instance.IsGamePlaying()) return;

        
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (!SoccerGameManager.Instance.IsGamePlaying()) return;

       
    }

      private void GameInput_OnShootAction(object sender, EventArgs e)
    {
        if (!SoccerGameManager.Instance.IsGamePlaying()) return;

        KickBall();
    }

    private void KickBall()
    {
        // 实例化球体并应用力
        if (ballPrefab == null)
        {
            Debug.LogError("Ball Prefab not assigned!");
            return;
        }

        // 创建球体并设置其位置
        GameObject ball = Instantiate(ballPrefab, transform.position + transform.forward, Quaternion.identity);

        // 获取球体的 Rigidbody 组件并施加力
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * kickForce, ForceMode.Impulse);
        }
    }

    private void Update() {
        if (!IsOwner) {
            return;
        }

        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteractions() {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
           
        }
    }

    private void HandleMovement() {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .6f;
        bool canMove = !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDir, Quaternion.identity, moveDistance, collisionsLayerMask);

        if (!canMove) {
            // Cannot move towards moveDir

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirX, Quaternion.identity, moveDistance, collisionsLayerMask);

            if (canMove) {
                // Can move only on the X
                moveDir = moveDirX;
            } else {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.BoxCast(transform.position, Vector3.one * playerRadius, moveDirZ, Quaternion.identity, moveDistance, collisionsLayerMask);

                if (canMove) {
                    // Can move only on the Z
                    moveDir = moveDirZ;
                } else {
                    // Cannot move in any direction
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    
    public NetworkObject GetNetworkObject() {
        return NetworkObject;
    }

}