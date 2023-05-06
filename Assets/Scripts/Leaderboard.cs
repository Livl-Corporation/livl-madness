using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour, IPlayerStatsObserver
{
    [SerializeField] private GameObject playerScoreBoardItem;
    [SerializeField] private Transform transformPlayerScoreBoardList;
    [SerializeField] public Sprite[] playerImages;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    [SerializeField] private int maxPlayersToDisplayScore = 3;
    
    private PlayerStatsController _playerStatsController;
    private readonly Dictionary<string, PlayerScoreBoardItem> _playerStatItems = new Dictionary<string, PlayerScoreBoardItem>();
    
    private void Start()
    {
        // Subscribe to the PlayerStatsController
        _playerStatsController = FindObjectOfType<PlayerStatsController>();
        if (_playerStatsController == null)
        {
            Debug.LogError("PlayerStatsListController: PlayerStatsController not found");
            return;
        }
        _playerStatsController.AddObserver(this);
    }
    
    public void UpdatePlayerStats(Dictionary<string, PlayerStat> playerStats)
    {
        var missingKeys = new List<string>(_playerStatItems.Keys);
        
        // Order the player stats by score
        playerStats = playerStats.OrderByDescending(x => x.Value.Score).ToDictionary(x => x.Key, x => x.Value);

        // Remove all existing player score board items
        foreach (Transform child in transformPlayerScoreBoardList)
        {
            Destroy(child.gameObject);
        }

        // For each connected player
        var index = 0;
        foreach (var playerStat in playerStats)
        {
            if (!_playerStatItems.ContainsKey(playerStat.Key))
            {
                // If player is not displayed, add it
                AddPlayerStat(playerStat.Value.Username);
            }
            
            missingKeys.Remove(playerStat.Key);
            
            if(index > maxPlayersToDisplayScore)
                break;
            
            var controller = _playerStatItems[playerStat.Key];
            controller.Set(playerStat.Value, playerImages[index++]);
        }

        // Remove disconnected players from list
        foreach (var missingKey in missingKeys)
        {
            RemovePlayerStat(missingKey);
        }
        
    }
    
    private void AddPlayerStat(string playerName)
    {
        var playerStatItem = Instantiate(playerScoreBoardItem, transformPlayerScoreBoardList);
        _playerStatItems.Add(playerName, playerStatItem.GetComponent<PlayerScoreBoardItem>());
    }

    private void RemovePlayerStat(string playerName)
    {
        var playerStatItem = _playerStatItems[playerName];
        _playerStatItems.Remove(playerName);
        Destroy(playerStatItem.gameObject);
    }    
    
    private void OnDestroy()
    {
        if (_playerStatsController == null) return;
        _playerStatsController.RemoveObserver(this);
        
        foreach(Transform child in transformPlayerScoreBoardList)
        {
            Destroy(child.gameObject);
        }
    }

    private int GetSpacingVerticalLayout(Player[] players)
    {
        int spacing = 0;
        if (players.Length <= 2)
            spacing = -150;
        return spacing;
    }
}
