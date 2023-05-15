using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Network
{
    [Serializable]
    public class Match
    {
        public string matchID;
        public bool started;
        public List<PlayerNetwork> players = new List<PlayerNetwork>();

        public Match(string matchID, PlayerNetwork playerNetwork)
        {
            this.matchID = matchID;
            this.started = false;
            this.players.Add(playerNetwork);
        }
        
        public Match() {}
    }
    
    
    
    public class MatchMaker : NetworkBehaviour
    {
        public static MatchMaker instance;

        public SyncList<Match> matches = new SyncList<Match>();
        public SyncList<string> matchIds = new SyncList<string>();

        private void Start()
        {
            instance = this;
        }

        public bool HostGame(string matchID, PlayerNetwork playerNetwork, out int playerIndex)
        {
            playerIndex = -1;
            
            if (matchIds.Contains(matchID))
            {
                Debug.Log($"Match ID {matchID} already exists");
                return false;
            }
            
            matches.Add(new Match(matchID, playerNetwork));
            matchIds.Add(matchID);

            playerIndex = 1;
            
            return true;
        }
        
        public bool JoinGame(string matchID, PlayerNetwork playerNetwork, out int playerIndex)
        {
            playerIndex = -1;

            if (!matchIds.Contains(matchID))
            {
                Debug.Log($"Match ID {matchID} doesn't exists");
                return false;
            }

            foreach (Match match in matches)
            {
                if (match.matchID == matchID)
                {
                    match.players.Add(playerNetwork);
                    playerIndex = match.players.Count;
                    break;
                }
            }
            
            Debug.Log($"Match ID {matchID} joined");
            return true;
        }

        public void BeginGame(string matchID)
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == matchID)
                {
                    matches[i].started = true;
                    foreach (PlayerNetwork player in matches[i].players)
                    {
                        player.StartGame();
                    }
                    break;
                }
            }

        }
        
        public void PlayerDisconnected (string matchID, PlayerNetwork playerNetwork) {
            for (int i = 0; i < matches.Count; i++) {
                if (matches[i].matchID == matchID) {
                    int playerIndex = matches[i].players.IndexOf (playerNetwork);
                    if (matches[i].players.Count > playerIndex) matches[i].players.RemoveAt (playerIndex);
                    Debug.Log ($"Player disconnected from match {matchID} | {matches[i].players.Count} players remaining");

                    if (matches[i].players.Count == 0) {
                        Debug.Log ($"No more players in Match. Terminating {matchID}");
                        matches.RemoveAt (i);
                        matchIds.Remove (matchID);
                    }
                    
                    break;
                }
            }
        }

        public static string GetRandomMatchID()
        {
            string id = String.Empty;

            for (int i = 0; i < 5; i++)
            {
                int random = Random.Range(0, 36);

                if (random < 26)
                {
                    id += (char)(random + 65);
                }
                else
                {
                    id += (random - 26).ToString();
                }
            }
            
            Debug.Log($"Random Match ID : {id}");

            return id;
        }
    }
    
    public static class MatchExtensions {
        public static Guid ToGuid (this string id) {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider ();
            byte[] inputBytes = Encoding.Default.GetBytes (id);
            byte[] hashBytes = provider.ComputeHash (inputBytes);

            return new Guid (hashBytes);
        }
    }
}