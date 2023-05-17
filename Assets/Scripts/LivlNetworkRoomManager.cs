using Mirror;
using UnityEngine;

public class LivlNetworkRoomManager : NetworkRoomManager
{
    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        var playerStatsController = FindObjectOfType<PlayerStatsController>();
        
        if (playerStatsController != null)
        {
            playerStatsController.RemovePlayer(conn.identity.netId);
        }
        
        var roomController = FindObjectOfType<RoomController>();

        if (roomController != null)
        {
            roomController.RemovePlayer(conn.identity.netId);
        }
        
        var playerRoomController = FindObjectOfType<PlayerRoomSpawn>();

        if (playerRoomController != null)
        {
            playerRoomController.UnspawnPlayerPrefab();
        }

        base.OnRoomServerDisconnect(conn);
        
        // TODO : si y'a plus de joueurs, on revient a la scene Room
        if (numPlayers == 0)
        {
            Debug.Log("Restart du serveur...");
            StopServer();
            
            CancelInvoke(nameof(GameManager.Instance.RejectAllPlayersAndLoadRoomScene));
        }
    }
}