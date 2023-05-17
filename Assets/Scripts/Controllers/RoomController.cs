using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Mirror;
using Models;
using TMPro;
using UnityEngine;

public class RoomController : NetworkBehaviour, IRoomObservable
{
    
    [Header("Components")]
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private NetworkRoomPlayer networkRoomPlayer;
    [SerializeField] private TMP_Text hourText;
    
    [Header("Config")]
    [SerializeField] private string readyButtonSoloText = "Démarrer la partie";
    [SerializeField] private string readyButtonMultiText = "Prêt";
    [SerializeField] private string unreadyButtonText = "Annuler";
    [SerializeField] private string soloHintText = "Attendez que d'autres joueurs rejoingnent la partie ou démarrez seul dès maintenant";
    [SerializeField] private string multiHintText = "La partie démarrera automatiquement lorsque tous les joueurs seront prêts";
    
    private List<IRoomObserver> observers = new List<IRoomObserver>();
    
    private readonly SyncDictionary<uint, PlayerRoom> roomPlayers = new SyncDictionary<uint, PlayerRoom>();

    private void Start()
    {
        roomPlayers.Callback += (op, key, item) => NotifyObservers();
    }

    public void SetNetworkRoomPlayer(NetworkRoomPlayer _networkRoomPlayer)
    {
        networkRoomPlayer = _networkRoomPlayer;
    }

    private void Update()
    {
        if (hourText == null) return;
        
        hourText.text = DateTime.Now.ToString("HH:mm");
    }

    public void OnStartButtonClick()
    {
        var ready = !networkRoomPlayer.readyToBegin;
        CmdSetReady(networkRoomPlayer.netId, ready);
        networkRoomPlayer.CmdChangeReadyState(ready);
    }
    
    public void OnBackButtonClick()
    {
        if (NetworkServer.active)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.StopClient();
        }
    }


    public void AddObserver(IRoomObserver observer)
    {
        observers.Add(observer);
        NotifyObservers();
    }

    public void RemoveObserver(IRoomObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        var result = roomPlayers.ToDictionary(entry => entry.Key, entry => entry.Value);
        foreach (var observer in observers)
        {
            observer.UpdateRoom(result);
        }
        
        if(networkRoomPlayer == null) return;
        
        var isSolo = result.Count == 1;
        
        hintText.text = isSolo ? soloHintText : multiHintText;
        buttonText.text = isSolo ? readyButtonSoloText : readyButtonMultiText;

        if (!roomPlayers.TryGetValue(networkRoomPlayer.netId, out var player)) return;
        
        if (player.IsReady)
        {
            buttonText.text = unreadyButtonText;
        }
        
    }
    
    [Command(requiresAuthority = false)]
    public void CmdAddPlayer(uint id, string username)
    {
        if (roomPlayers.ContainsKey(id))
        {
            Debug.LogError($"Player with id {id} already exists in room");
            return;
        }
        
        var player = new PlayerRoom(username, id);
        roomPlayers.Add(id, player);
        FindObjectOfType<PlayerRoomSpawn>().SpawnPlayerPrefab(username);
    }
    
    public void RemovePlayer(uint id)
    {
        if (!roomPlayers.ContainsKey(id))
        {
            Debug.LogError($"Player with id {id} not found in room");
            return;
        }
        
        roomPlayers.Remove(id);
    }
    
    [Command(requiresAuthority = false)]
    public void CmdSetReady(uint id, bool ready)
    {
        var player = roomPlayers[id];
        
        if (player == null)
        {
            Debug.LogError($"Player with id {id} not found in room");
            return;
        }

        roomPlayers[id] = new PlayerRoom(player, ready);
    }

    public List<string> GetPlayerUsernames()
    {
        return roomPlayers.Values.Select(player => player.Username).ToList();
    }
}
