using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Models;
using UnityEngine;

public class PlayerStatsListController : MonoBehaviour, IPlayerStatsObserver
{
    
    private PlayerStatsController _playerStatsController;
    private readonly Dictionary<string, PlayerStatItemController> _playerStatItems = new Dictionary<string, PlayerStatItemController>();
    
    [SerializeField] private GameObject playerStatPrefab;
    
    // Start is called before the first frame update
    void Start()
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

        Debug.Log(String.Join(", ", playerStats.Select(a => a.Value.Username + " : " + a.Value.Score)));

        var missingKeys = new List<string>(_playerStatItems.Keys);
        
        // For each connected player
        foreach (var playerStat in playerStats)
        {
            if (!_playerStatItems.ContainsKey(playerStat.Key))
            {
                // If player is not displayed, add it
                AddPlayerStat(playerStat.Value.Username);
            }
            
            _playerStatItems[playerStat.Key].Set(playerStat.Value);
            
            missingKeys.Remove(playerStat.Key);
        }
        
        // Remove disconnected players from list
        foreach (var missingKey in missingKeys)
        {
            RemovePlayerStat(missingKey);
        }
        
    }

    private void AddPlayerStat(string playerName)
    {
        var playerStatItem = Instantiate(playerStatPrefab, transform);
        _playerStatItems.Add(playerName, playerStatItem.GetComponent<PlayerStatItemController>());
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
    }
}
