using System;
using Mirror;
using TMPro;
using UI;
using UnityEngine;

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
    }
}