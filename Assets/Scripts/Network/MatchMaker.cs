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
        public List<PlayerNetwork> players = new List<PlayerNetwork>();

        public Match(string matchID, PlayerNetwork playerNetwork)
        {
            this.matchID = matchID;
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

        public bool HostGame(string matchID, PlayerNetwork playerNetwork)
        {
            if (matchIds.Contains(matchID))
            {
                Debug.Log($"Match ID {matchID} already exists");
                return false;
            }
            
            matches.Add(new Match(matchID, playerNetwork));
            matchIds.Add(matchID);
            
            return true;
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