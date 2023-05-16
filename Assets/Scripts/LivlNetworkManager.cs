using Mirror;
using UnityEngine;

public class LivlNetworkManager : NetworkManager
{
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        
        var playerStatsController = FindObjectOfType<PlayerStatsController>();
        
        if (playerStatsController == null)
        {
            Debug.LogError("PlayerStatsController not found");
            return;
        }
        
        playerStatsController.CmdRemovePlayer(conn.identity.netId);
        
        base.OnServerDisconnect(conn);

    }
}
