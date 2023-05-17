using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerView : MonoBehaviour
{
    
    [Header("Components")]
    [SerializeField]
    private TMP_Text playerName;

    [SerializeField] private Image stateImage;

    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }
    
    public void SetPlayerState(bool isReady)
    {
        stateImage.enabled = isReady;
    }
    
}
