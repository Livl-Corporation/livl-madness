using System;
using Mirror;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class PlayerNetwork : NetworkBehaviour
    {
        public static PlayerNetwork localPlayerNetwork;
        
        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;
        
        NetworkMatch networkMatch;

        void Start()
        {
            networkMatch = GetComponent<NetworkMatch>();

            if (isLocalPlayer)
            {
                localPlayerNetwork = this;
            }
            else
            {
                UILobby.instance.SpawnPlayerFramePrefab(this);  
            }
        }

        /*
         * HOST PART
         */
        public void HostGame()
        {
            string matchID = MatchMaker.GetRandomMatchID();
            CmdHostGame(matchID);
        }

        [Command]
        void CmdHostGame(string matchId)
        {
            matchID = matchId; 
            bool hostedSuccesfully = MatchMaker.instance.HostGame(matchId, this, out playerIndex);

            if (hostedSuccesfully)
            {
                Debug.Log("Game hosted successfully");
                Guid guid = matchId.ToGuid();
                networkMatch.matchId = guid;
            }
            else
            {
                Debug.Log("Game hosted failed");
            }
            
            TargetHostGame(hostedSuccesfully, matchId);
        }

        [TargetRpc]
        void TargetHostGame(bool success, string matchID)
        {
            Debug.Log($"MatchID : {matchID} == {this.matchID}");
            UILobby.instance.HostSuccess(success);
        }
        
        /**
         * JOIN PART
         */
        
        public void JoinGame(string matchID)
        {
            CmdJoinGame(matchID);
        }

        [Command]
        void CmdJoinGame(string matchId)
        {
            matchID = matchId; 
            bool joinedSuccesfully = MatchMaker.instance.JoinGame(matchId, this, out playerIndex);

            if (joinedSuccesfully)
            {
                Debug.Log("Game joined successfully");
                Guid guid = matchId.ToGuid();
                networkMatch.matchId = guid;
            }
            else
            {
                Debug.Log("Game joined failed");
            }
            
            TargetJoinGame(joinedSuccesfully, matchId);
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string matchID)
        {
            Debug.Log($"MatchID : {matchID}");
            UILobby.instance.JoinSuccess(success);
        }
        
        /**
         * BEGIN PART
         */
        public void BeginGame()
        {
            CmdBeginGame();
        }

        [Command]
        void CmdBeginGame()
        { 
            Debug.Log("Game started");
            
            RpcBeginGame();
        }

        [ClientRpc]
        void RpcBeginGame()
        {
            Debug.Log($"MatchID : {matchID} | Beginning");
            // Load game scene
            SceneManager.LoadScene("Livl");
        }
        
    }
}