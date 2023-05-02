using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreBoardItem : MonoBehaviour
{
    [SerializeField] public Image playerImage;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerScoreText;
    
    public void Setup(Player player, Sprite sprite)
    {
        playerNameText.text = player.name;
        playerScoreText.text = player.GetScore().ToString();
        playerImage.sprite = sprite;
    }
}
