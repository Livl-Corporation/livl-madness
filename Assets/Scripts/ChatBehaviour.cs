using Mirror;
using System;
using TMPro;
using UnityEngine;

public class ChatBehaviour : NetworkBehaviour
{
    [HideInInspector] public GameObject chatPanel;
    [HideInInspector] public TMP_Text chatText;
    [HideInInspector] public TMP_InputField chatInput;
    
    [HideInInspector] public bool isInitialized = false; // add an initialization flag

    private static event Action<string> OnMessage;

    public override void OnStartAuthority()
    {
        if (!isInitialized) // check if the component is initialized before continuing
        {
            Debug.LogWarning("ChatBehaviour is not initialized");
            return;
        }
        
        Debug.Log("OnStartAuthority");

        chatPanel.SetActive(true);

        OnMessage += HandleNewMessage;
    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!isOwned) { return; }

        OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(string message)
    {
        chatText.text += message;
    }
    
    [Client]
    public void Send(string message)
    {
        PlayerUI.isPaused = false;

        if (!Input.GetKeyDown(KeyCode.Return)) { return; }

        if (string.IsNullOrWhiteSpace(message)) { return; }

        Debug.Log($"Send message: {message}");
        
        CmdSendMessage(message, Player.LocalPlayerName);

        chatInput.text = string.Empty;
    }
    
    [Command]
    private void CmdSendMessage(string message, string playerName)
    {
        RpcHandleMessage(playerName, message);
    }

    [ClientRpc]
    private void RpcHandleMessage(string playerName, string message)
    {
        string formattedMessage = $"\n<color=blue><b>[{playerName}]</b></color>: {message}";
        OnMessage?.Invoke(formattedMessage);
    }
}