using System;
using Mirror;
using TMPro;
using UnityEngine;

public class PlayerFrame : NetworkBehaviour
{
    [SerializeField] private NetworkRoomPlayer networkRoomPlayer;
    [SerializeField] private RoomController roomController;
    
    public static string Username;
    
    private void Start()
    {
        
        if (!isLocalPlayer)
        {
            return;
        }
        
        roomController = FindObjectOfType<RoomController>();
        
        if (roomController == null)
        {
            Debug.LogError("RoomController not found");
            return;
        }
        
        Username = DefinePlayerName();
        
        roomController.SetNetworkRoomPlayer(networkRoomPlayer);
        roomController.CmdAddPlayer(networkRoomPlayer.netId, Username);

    }
    
    private string DefinePlayerName()
    {
        var usedPlayerNames = roomController.GetPlayerUsernames();
        var playerName = PlayerPrefs.GetString("Username", "Player" + GetComponent<NetworkIdentity>().netId);
        
        // Check if player name is used
        while (usedPlayerNames.Contains(playerName))
        {
            playerName += "1";
        }
        
        return playerName;
    }

}
