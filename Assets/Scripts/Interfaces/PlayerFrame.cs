using Mirror;
using TMPro;
using UnityEngine;

public class PlayerFrame : MonoBehaviour
{
    [SerializeField] TMP_Text readyButtonText;
    [SerializeField] NetworkRoomPlayer networkRoomPlayer;

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
