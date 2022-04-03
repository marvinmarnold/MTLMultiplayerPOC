using Unity.Netcode;
using TMPro;

public class PlayerHud : NetworkBehaviour
{
    private NetworkVariable<NetworkString> playerName = new NetworkVariable<NetworkString>();

    private bool isPlayerNameSet = false;

  public override void OnNetworkSpawn()
  {
    if (IsServer)
    {
        playerName.Value = $"Player {OwnerClientId}";
    }
  }

  public void SetPlayerName()
  {
      var localPlayerName = gameObject.GetComponentInChildren<TextMeshProUGUI>();
      localPlayerName.text = playerName.Value;
  }

  private void Update()
  {
      if (!isPlayerNameSet && !string.IsNullOrEmpty(playerName.Value))
      {
          SetPlayerName();
          isPlayerNameSet = true;
      }
  }
}


