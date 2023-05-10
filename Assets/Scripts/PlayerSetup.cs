using System.Collections;
using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    
    [SerializeField]
    string remoteLayerName = "RemotePlayer";
    
    [SerializeField]
    private GameObject playerUIPrefab;
    
    [HideInInspector]
    public GameObject playerUIInstance;
    
    [SerializeField]
    private PlayerStatsController playerStatsController;
    
    private Camera sceneCamera;
    private ChatBehaviour chatBehaviour;
    private PlayerUI playerUI;
    
    // Start is called before the first frame update
    private void Start()
    {
        var playerName = DefinePlayerName();

        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
            return;
        }

        RegisterPlayerStats(playerName);
        Player.LocalPlayerName = playerName;
        GameManager.SetSceneCameraActive(false);
        InitPlayerUI();
    }

    private void DisableComponents()
    {
        foreach (Behaviour component in componentsToDisable)
        {
            component.enabled = false;
        }
    }

    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }
    
    // When player quit the server
    private void OnDisable()
    {

        if (!isLocalPlayer)
            return;
        
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);

        playerStatsController.CmdRemovePlayer(transform.name);
        Destroy(playerUIInstance);
        
    }

    private string DefinePlayerName()
    {
        var playerName = "Player" + GetComponent<NetworkIdentity>().netId;
        transform.name = playerName;
        return playerName;
    }

    private void InitPlayerUI()
    {
        // Création du UI du joueur local
        playerUIInstance = Instantiate(playerUIPrefab);
        playerUIInstance.SetActive(true);

        // Configuration du UI
        playerUI = playerUIInstance.GetComponent<PlayerUI>();

        if(playerUI == null)
        {
            Debug.LogError("Pas de component PlayerUI sur playerUIInstance");
            return;
        }
        
        // Configuration du chat
        InitChatBehaviour();

        playerUI.SetPlayer(GetComponent<Player>());
    }

    private void InitChatBehaviour()
    {
        chatBehaviour = GetComponent<ChatBehaviour>();
        
        if(chatBehaviour == null)
        {
            Debug.LogError("Pas de component ChatBehaviour sur PlayerArmature");
            return;
        }
        
        chatBehaviour.chatPanel = playerUI.chatPanel;
        chatBehaviour.chatText = playerUI.chatText;
        chatBehaviour.chatInput = playerUI.chatInput;
        chatBehaviour.chatInput.onEndEdit.AddListener(chatBehaviour.Send);
        chatBehaviour.isInitialized = true;
        
        chatBehaviour.OnStartAuthority();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && chatBehaviour != null)
        {
            // Select the chatBehaviour input field to set focus on it
            StopAllCoroutines();
            chatBehaviour.chatPanel.SetActive(true);
            chatBehaviour.chatInput.Select();
            chatBehaviour.chatInput.ActivateInputField();
            PlayerUI.isPaused = true;
        }
        else if (chatBehaviour != null && !chatBehaviour.chatInput.isFocused && !PlayerUI.isPaused && chatBehaviour.chatPanel.activeSelf)
        {
            // If the player is not interacting with the input field and has not pressed any key recently,
            // start a coroutine to hide the chat panel after a delay
            StartCoroutine(HideChatAfterDelay());
        }
    }
    
    private IEnumerator HideChatAfterDelay()
    {
        yield return new WaitForSeconds(chatBehaviour.hideChatPanelAfterDelay);
        chatBehaviour.chatPanel.SetActive(false);
    }

    private void RegisterPlayerStats(string playerName)
    {
        if (playerStatsController == null)
        {
            playerStatsController = FindObjectOfType<PlayerStatsController>();
        }
        
        if (playerStatsController == null)
        {
            Debug.LogError("PlayerStatsController not found");
            return;
        }
        
        // Ajout du joueur aux statistiques
        playerStatsController.CmdAddPlayer(playerName);

    }
    
}
