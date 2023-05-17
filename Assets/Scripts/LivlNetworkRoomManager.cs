using Mirror;
using UnityEngine;

public class LivlNetworkRoomManager : NetworkRoomManager
{
    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        var playerStatsController = FindObjectOfType<PlayerStatsController>();

        if (playerStatsController == null)
        {
            Debug.LogError("PlayerStatsController not found");
            return;
        }

        playerStatsController.CmdRemovePlayer(conn.identity.netId);
        
        base.OnRoomServerDisconnect(conn);
    }
}