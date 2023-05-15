using System;
using Mirror;
using Mirror.SimpleWeb;
using UnityEngine;

namespace Network
{
    public class AutoHostClient : MonoBehaviour
    {
        
        [SerializeField] NetworkManager networkManager;

        void Start()
        {
            if (Application.isBatchMode) // headless build
            {
                Debug.Log($"=== Server build ===");
                Debug.Log($"Listening to port {(networkManager.transport as SimpleWebTransport).port}");
                return;
            }
            
            Debug.Log("=== Client build ===");
            networkManager.StartClient();
        }

        public void JoinLocal()
        {
            networkManager.networkAddress = "localhost";
            networkManager.StartClient();
        }
    }
}