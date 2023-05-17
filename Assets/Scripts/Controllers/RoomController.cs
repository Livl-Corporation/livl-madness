using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    
    [Header("Components")]
    [SerializeField] private GameObject playerList;
    [SerializeField] private GameObject playerFramePrefab;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject hintText;
    [SerializeField] NetworkRoomPlayer networkRoomPlayer;
    
    public void OnStartButtonClick()
    {
        networkRoomPlayer.readyToBegin = !networkRoomPlayer.readyToBegin;
    }
    
    public void OnBackButtonClick()
    {
        if (NetworkServer.active)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }

}
