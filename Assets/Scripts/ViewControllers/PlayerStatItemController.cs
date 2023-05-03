using Models;
using TMPro;
using UnityEngine;

public class PlayerStatItemController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text scoreText;
    
    [SerializeField] private TMP_FontAsset boldFont;
    [SerializeField] private TMP_FontAsset regularFont;

    public void Set(PlayerStat stat)
    {
        playerNameText.text = stat.Username;
        scoreText.text = stat.Score.ToString();
        playerNameText.font = stat.Username == Player.LocalPlayerName 
            ? boldFont
            : regularFont;
    }
    
}
