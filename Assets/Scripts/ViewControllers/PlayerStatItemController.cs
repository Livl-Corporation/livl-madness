using System;
using Models;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatItemController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text scoreText;
    
    [SerializeField] private TMP_FontAsset boldFont;
    [SerializeField] private TMP_FontAsset regularFont;

    [SerializeField] private Image background;

    [Header("Configuration")] 
    [SerializeField] private Color localPlayerColor;
    [SerializeField] private Color otherPlayerColor;

    public void Set(PlayerStat stat)
    {
        var isLocalPlayer = stat.Username == Player.LocalPlayerName;
        playerNameText.text = stat.Username;
        scoreText.text = stat.Score.ToString();
        playerNameText.font =  isLocalPlayer
            ? boldFont
            : regularFont;
        background.color = isLocalPlayer
            ? localPlayerColor
            : otherPlayerColor;
    }
    
}
