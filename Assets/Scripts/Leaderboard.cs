using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject playerScoreBoardItem;
    [SerializeField] private Transform playerScoreBoardList;
    [SerializeField] public Sprite[] playerImages;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    
    private void OnEnable()
    {

    }

    private int GetSpacingVerticalLayout(Player[] players)
    {
        int spacing = 0;
        if (players.Length <= 2)
            spacing = -150;
        return spacing;
    }
    
    private void OnDisable()
    {
        foreach(Transform child in playerScoreBoardList)
        {
            Destroy(child.gameObject);
        }
    }
}
