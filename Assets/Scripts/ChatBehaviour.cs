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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsChatFocused())
        {
            UnfocusChat();
            HideChatPanel();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            // Select the chatBehaviour input field to set focus on it
            ShowChatPanel();
            chatInput.placeholder.GetComponent<TextMeshProUGUI>().text = "Écrivez votre message ici...";
            chatInput.Select();
            chatInput.ActivateInputField();
            PlayerUI.isPaused = true;
        }
        
    }
    
    public bool IsChatFocused()
    {
        return chatInput != null && chatInput.isFocused;
    }

    /**
     * Activate the chatPanel of all clients when a message is received
     */
    private void ShowChatPanel()
    {
        chatPanel.GetComponent<CanvasGroup>().LeanAlpha(1f, 0.5f);
        StartCoroutine(HideChatPanel());
    }

    public IEnumerator HideChatPanel()
    {
        yield return new WaitForSeconds(hideChatPanelAfterDelay);
        if (!PlayerUI.isPaused) // if PlayerUI is pause means that the user is typing a message
        {
            chatPanel.GetComponent<CanvasGroup>().LeanAlpha(0.5f, 0.5f);
        }
    }

    public void UnfocusChat()
    {
        PlayerUI.isPaused = false;
        GUI.FocusControl(null);
        chatInput.placeholder.GetComponent<TextMeshProUGUI>().text = "Appuyez sur 'T' pour parler";
        chatInput.DeactivateInputField();
        chatInput.text = string.Empty;
    }

    [Client]
    public void Send(string message)
    {
        
        UnfocusChat();
        HideChatPanel();

        if (!Input.GetKeyDown(KeyCode.Return)) { return; }
        if (string.IsNullOrWhiteSpace(message)) { return; }
        
        CmdSendMessage(message, Player.LocalPlayerName);

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
    
    [Command]
    public void CmdSendSystemMessage(string message)
    {
        RpcHandleSystemMessage(message);
    }
    
    [ClientRpc]
    private void RpcHandleSystemMessage(string message)
    {
        string formattedMessage = $"\n<color=blue><b><i>{message}</i></b></color>";
        OnMessage?.Invoke(formattedMessage);
    }
}