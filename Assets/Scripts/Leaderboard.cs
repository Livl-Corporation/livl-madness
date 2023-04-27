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
        Player[] players = sortPlayersScore();

        int index = 0;
        foreach (Player player in players)
        {
            Debug.Log(player.name + " : " + player.GetScore());
            GameObject itemGO = Instantiate(playerScoreBoardItem, playerScoreBoardList);
            PlayerScoreBoardItem item = itemGO.GetComponent<PlayerScoreBoardItem>();
            
            if(item != null)
            {
                item.Setup(player, playerImages[index++]);
            }
        }
    }
    
    private Player[] sortPlayersScore()
    {
        Player[] players = GameManager.GetAllPlayers();
        int n = players.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int min_idx = i;
            for (int j = i + 1; j < n; j++)
                if (players[j].GetScore() < players[min_idx].GetScore())
                    min_idx = j;
            (players[min_idx], players[i]) = (players[i], players[min_idx]);
        }
        return players;
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
