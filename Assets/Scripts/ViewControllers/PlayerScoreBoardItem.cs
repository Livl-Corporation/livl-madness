using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreBoardItem : MonoBehaviour 
{
    [SerializeField] public Image playerImage;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerScoreText;
    
    public void Set(PlayerStat player, Sprite sprite)
    {
        playerNameText.text = player.Username;
        playerScoreText.text = player.Score.ToString();
        playerImage.sprite = sprite;
    }
}
