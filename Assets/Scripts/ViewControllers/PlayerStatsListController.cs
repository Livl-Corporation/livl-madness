using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Models;
using UnityEngine;

public class PlayerStatsListController : MonoBehaviour, IPlayerStatsObserver
{
    
    private PlayerStatsController _playerStatsController;
    private readonly List<PlayerStatItemController> _playerStatItems = new List<PlayerStatItemController>();
    
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
        
        // For each child of playerStatItemsList, get the PlayerStatItemController component
        foreach (Transform child in transform)
        {
            _playerStatItems.Add(child.GetComponent<PlayerStatItemController>());
        }
    }

    public void UpdatePlayerStats(Dictionary<string, PlayerStat> playerStats)
    {
        
        var diff = playerStats.Count - _playerStatItems.Count;
        BalancePlayerCount(diff);

        var playerStatsValues = playerStats.Values.ToArray();
        for (int i = 0; i < _playerStatItems.Count; i++)
        {
            var playerStat = playerStatsValues[i];
            _playerStatItems[i].Set(playerStat, playerStat.Username == Player.LocalPlayerName);
        }
        
    }

    private void BalancePlayerCount(int diff)
    {
        if (diff == 0) return;
        
        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                AddPlayerStat();
            }
        }
        else
        {
            for (int i = 0; i < Math.Abs(diff); i++)
            {
                RemovePlayerStat();
            }
        }
    }
    
    private void AddPlayerStat()
    {
        var playerStatItem = Instantiate(playerStatPrefab, transform);
        _playerStatItems.Add(playerStatItem.GetComponent<PlayerStatItemController>());
    }

    private void RemovePlayerStat()
    {
        var playerStatItem = _playerStatItems[_playerStatItems.Count - 1];
        _playerStatItems.Remove(playerStatItem);
        Destroy(playerStatItem.gameObject);
    }    
    
    private void OnDestroy()
    {
        if (_playerStatsController == null) return;
        _playerStatsController.RemoveObserver(this);
    }
}
