
using Unity.Netcode;
using UnityEngine;

public class PlayerControl : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.001f;

    [SerializeField]
    private Vector2 defaultPositionRange = new Vector2(-4, 4);

    [SerializeField]
    private NetworkVariable<float> forwardBackPosition = new NetworkVariable<float>();

    [SerializeField]
    private NetworkVariable<float> leftRightPosition = new NetworkVariable<float>();

    // client caching
    private float oldForwardBackPosition;
    private float oldLeftRightPosition;

    private void Start() {
        {
            float x = Random.Range(defaultPositionRange.x, defaultPositionRange.y);
            float z = Random.Range(defaultPositionRange.x, defaultPositionRange.y);
            transform.position = new Vector3(x, 0, z);
            Debug.Log($"Starting at {x}, 0, {z}");

        }
    }

    private void Update() 
    {
        if (IsServer)
        {
            UpdateServer();
        }

        if (IsClient && IsOwner)
        {
            UpdateClient();
        }
    }

    private void UpdateServer()
    {
        float x = transform.position.x + leftRightPosition.Value;
        float y = transform.position.y;
        float z = transform.position.z + forwardBackPosition.Value;
        Debug.Log($"Moved to {x}, {y}, {z}");
        transform.position = new Vector3(x, y, z);
    }

    private void UpdateClient()
    {
        float forwardBackward = 0;
        float leftRight = 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) 
        {
        Debug.Log($"Moving up");
            forwardBackward += moveSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) 
        {
            Debug.Log($"Moving down");
            forwardBackward -= moveSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
        {
            Debug.Log($"Moving left");
            leftRight -= moveSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) 
        {
            Debug.Log($"Moving right");
            leftRight += moveSpeed;
        }

        if (oldForwardBackPosition != forwardBackward || oldLeftRightPosition != leftRight) 
        {
            Debug.Log($"Really making moves to {forwardBackward}, {leftRight}");
            oldForwardBackPosition = forwardBackward;
            oldLeftRightPosition = leftRight;

            UpdateClientPositionServerRpc(forwardBackward, leftRight);
        }
    }

    [ServerRpc]
    public void UpdateClientPositionServerRpc(float forwardBackward, float leftRight)
    {
        forwardBackPosition.Value = forwardBackward;
        leftRightPosition.Value = leftRight;
    }
}
