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

    [Command]
    public void CmdAddPlayer(string playerName)
    {
        _playerStats.Add(playerName, new PlayerStat());
    }

    [Command]
    public void CmdRemovePlayer(string playerName)
    {
        _playerStats.Remove(playerName);
    }
    
    [Command]
    public void CmdIncrementScore(string playerName)
    {
        
        if (!_playerStats.ContainsKey(playerName))
        {
            Debug.LogError("Player not found");
            return;
        }

        var score = _playerStats[playerName].Score;
        _playerStats[playerName] = new PlayerStat(score + 1);
    }
    
    [Command]
    public void CmdDecrementScore(string playerName)
    {
        
        if (!_playerStats.ContainsKey(playerName))
        {
            Debug.LogError("Player not found");
            return;
        }

        var score = _playerStats[playerName].Score;
        _playerStats[playerName] = new PlayerStat(score - 1);
    }

    public void AddObserver(IPlayerStatsObserver observer)
    {
        _observers.Add(observer);
        NotifyObservers();
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
            observer.Update(playerStats);
        }
    }
}
