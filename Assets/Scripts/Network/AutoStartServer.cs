using System;
using Mirror;
using UnityEngine;

public class AutoStartServer : MonoBehaviour
{
    private void Start()
    {
        // Parse command line argument for server builds
        string[] args = Environment.GetCommandLineArgs();

        foreach (string argument in args)
        {
            if (argument == "--server")
            {
                Debug.Log("=== Server build ===");
                NetworkManager.singleton.StartServer();
                return;
            }
        }
        
        Debug.Log("=== Client build ===");
    }
}
