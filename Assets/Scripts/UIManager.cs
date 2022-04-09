using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startServerButton;

    [SerializeField]
    private Button startClientButton;

    [SerializeField]
    private TMP_InputField joinCodeInput;

    [SerializeField]
    private TextMeshProUGUI playersInGameText;

    private void Awake()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        playersInGameText.text = $"Players in game: {PlayerManager.Instance.PlayersInGame}";
    }

    private void Start()
    {
        startHostButton.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.IsRelayEnabled)
            {
                Debug.Log("Host SETTING UP RELAY...");
                await RelayManager.Instance.SetupRelay();
            }
            if(NetworkManager.Singleton.StartHost())
            {
                Debug.Log("Host started...");
            }
            else
            {
                Debug.Log("Host failed to start...");
            }
        });

        // startServerButton.onClick.AddListener(() =>
        // {
        //     if(NetworkManager.Singleton.StartServer())
        //     {
        //         Debug.Log("Server started...");
        //     }
        //     else
        //     {
        //         Debug.Log("Server failed to start...");

        //     }
        // });

        startClientButton.onClick.AddListener(async () =>
        {
            if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text))
            {
                Debug.Log("Client connecting via relay...");
                await RelayManager.Instance.JoinRelay(joinCodeInput.text);
            }

            if(NetworkManager.Singleton.StartClient())
            {
                Debug.Log("Client started...");
            }
            else
            {
                Debug.Log("Client failed to start...");
            }
        });
    }
}
