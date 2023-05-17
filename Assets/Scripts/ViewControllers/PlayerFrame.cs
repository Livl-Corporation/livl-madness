using System;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerFrame : NetworkBehaviour
{
    [SerializeField] TMP_Text readyButtonText;
    [SerializeField] NetworkRoomPlayer networkRoomPlayer;

    private void Start()
    {
        Debug.Log("PlayerFrame Start " + isLocalPlayer);
    }

    public void OnReadyButtonClick()
    {
        networkRoomPlayer.readyToBegin = !networkRoomPlayer.readyToBegin;

        if (networkRoomPlayer.readyToBegin)
        {
            Debug.Log("Ready to begin !");
            readyButtonText.text = "Cancel";
        }
        else
        {
            Debug.Log("Not ready to begin !");
            readyButtonText.text = "Ready ?";
        }
    }
}
