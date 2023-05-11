using Mirror;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ChatBehaviour : NetworkBehaviour
{
    [Header("Chat panel Canvas")]
    [HideInInspector] public GameObject chatPanel;
    [HideInInspector] public TMP_Text chatText;
    [HideInInspector] public TMP_InputField chatInput;

    [Header("Commons")]
    [HideInInspector] public bool isInitialized; // add an initialization flag
    [SerializeField] public float hideChatPanelAfterDelay = 5f;
    
    private static event Action<string> OnMessage;

    public override void OnStartAuthority()
    {
        if (!isInitialized) return;
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
        
        ShowChatPanel(); 
    }
    
    /**
     * Activate the chatPanel of all clients when a message is received
     */
    private void ShowChatPanel()
    {
        chatPanel.SetActive(true);
        StartCoroutine(HideChatPanel());
    }

    private IEnumerator HideChatPanel()
    {
        yield return new WaitForSeconds(hideChatPanelAfterDelay);
        if(!PlayerUI.isPaused) // if PlayerUI is pause means that the user is typing a message
            chatPanel.SetActive(false);
    }

    [Client]
    public void Send(string message)
    {
        PlayerUI.isPaused = false;
        if (!Input.GetKeyDown(KeyCode.Return)) { return; }
        if (string.IsNullOrWhiteSpace(message)) { return; }
        
        CmdSendMessage(message, Player.LocalPlayerName);

        chatInput.text = string.Empty;
        chatInput.placeholder.GetComponent<TextMeshProUGUI>().text = "Press 'T' to chat";
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