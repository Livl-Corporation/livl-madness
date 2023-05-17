using System.Collections.Generic;
using Interfaces;
using Mirror;
using Models;
using TMPro;
using UnityEngine;

public class PlayerNameplate : NetworkBehaviour, IPlayerStatsObserver
{
    
    [SerializeField] private TMP_Text playerNameText;
    
    private PlayerStatsController _playerStatsController;

    // Start is called before the first frame update
    void Start()
    {
        _playerStatsController = FindObjectOfType<PlayerStatsController>();
        
        if (_playerStatsController == null)
        {
            Debug.LogError("PlayerStatsController not found");
            return;
        }
        
        _playerStatsController.AddObserver(this);
    }

    private void OnDestroy()
    {
        if (_playerStatsController == null)
        {
            return;
        }
        
        _playerStatsController.RemoveObserver(this);
    }

    public void UpdatePlayerStats(Dictionary<string, PlayerStat> playerStats)
    {
        var username = _playerStatsController.GetUsername(netId);
        SetUserName(username);
    }

    void SetUserName(string username)
    {
        playerNameText.text = username;
    }

}
