using Mirror;
using UnityEngine;

public class LivlNetworkRoomManager : NetworkRoomManager
{
    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        var playerStatsController = FindObjectOfType<PlayerStatsController>();

        if (playerStatsController != null)
        {
            playerStatsController.CmdRemovePlayer(conn.identity.netId);
        }
        
        var roomController = FindObjectOfType<RoomController>();

        if (roomController != null)
        {
            roomController.CmdRemovePlayer(conn.identity.netId);
        }
        
        var playerRoomController = FindObjectOfType<PlayerRoomSpawn>();

        if (playerRoomController != null)
        {
            playerRoomController.CmdUnspawnPlayerPrefab();
        }

        base.OnRoomServerDisconnect(conn);
    }
}