using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Models;
using UnityEngine;

public class RoomListController : MonoBehaviour, IRoomObserver
{
    
    [SerializeField] private GameObject playerFramePrefab;
    
    [SerializeField] private RoomController roomController;
    
    private Dictionary<uint, GameObject> playerFrames = new Dictionary<uint, GameObject>();

    private void Start()
    {
        roomController = FindObjectOfType<RoomController>();
        
        if (roomController == null)
        {
            Debug.LogError("RoomController not found");
            return;
        }
        
        roomController.AddObserver(this);
    }

    private void OnDestroy()
    {
        if (roomController == null)
        {
            Debug.LogError("RoomController not found");
            return;
        }
        
        roomController.RemoveObserver(this);
    }

    public void UpdateRoom(Dictionary<uint, PlayerRoom> roomPlayers)
    {
        
        var missingPlayers = new List<uint>(playerFrames.Keys);
        
        foreach (var player in roomPlayers)
        {

            missingPlayers.Remove(player.Key);

            if (playerFrames.TryGetValue(player.Key, out var frame))
            {
                frame.GetComponent<RoomPlayerView>().SetPlayerState(player.Value.IsReady);
                continue;
            }
            
            var playerFrame = Instantiate(playerFramePrefab, transform);
            var roomPlayerView = playerFrame.GetComponent<RoomPlayerView>();
            
            roomPlayerView.SetPlayerName(player.Value.Username);
            roomPlayerView.SetPlayerState(player.Value.IsReady);
            
            playerFrames[player.Key] = playerFrame;
        }
        
        foreach (var missingPlayer in missingPlayers)
        {
            Destroy(playerFrames[missingPlayer]);
            playerFrames.Remove(missingPlayer);
        }

    }
}
