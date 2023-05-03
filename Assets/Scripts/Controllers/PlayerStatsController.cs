using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Mirror;
using Models;
using UnityEngine;

public class PlayerStatsController : NetworkBehaviour, IPlayerStatsObservable
{
   
    private readonly SyncDictionary<string, PlayerStat> _playerStats = new SyncDictionary<string, PlayerStat>();
    
    private readonly List<IPlayerStatsObserver> _observers = new List<IPlayerStatsObserver>();

    private void Start()
    {
        _playerStats.Callback += (op, key, item) => NotifyObservers();
    }

    [Command(requiresAuthority = false)]
    public void CmdAddPlayer(string playerName)
    {
        if (_playerStats.ContainsKey(playerName))
        {
            Debug.LogError("Player already exists");
            return;
        }
        
        _playerStats.Add(playerName, new PlayerStat(playerName));
    }

    [Command(requiresAuthority = false)]
    public void CmdRemovePlayer(string playerName)
    {
        if (!_playerStats.ContainsKey(playerName))
        {
            Debug.LogError("Player not found");
            return;
        }
        
        _playerStats.Remove(playerName);
    }
    
    [Command(requiresAuthority = false)]
    public void CmdIncrementScore(string playerName)
    {
        
        if (!_playerStats.ContainsKey(playerName))
        {
            Debug.LogError("Player not found");
            return;
        }

        var playerStat = _playerStats[playerName];
        _playerStats[playerName] = new PlayerStat(playerStat, playerStat.Score + 1);
    }
    
    [Command(requiresAuthority = false)]
    public void CmdDecrementScore(string playerName)
    {
        
        if (!_playerStats.ContainsKey(playerName))
        {
            Debug.LogError("Player not found");
            return;
        }
        
        var playerStat = _playerStats[playerName];
        _playerStats[playerName] = new PlayerStat(playerStat, playerStat.Score - 1);
    }

    public void AddObserver(IPlayerStatsObserver observer)
    {
        _observers.Add(observer);
        Dictionary<string, PlayerStat> playerStats = _playerStats.ToDictionary(entry => entry.Key, entry => entry.Value);
        observer.UpdatePlayerStats(playerStats);
    }

    public void RemoveObserver(IPlayerStatsObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        Dictionary<string, PlayerStat> playerStats = _playerStats.ToDictionary(entry => entry.Key, entry => entry.Value);
        foreach (var observer in _observers)
        {
            observer.UpdatePlayerStats(playerStats);
        }
    }
}
