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
        if (playerStatsController == null)
        {
            playerStatsController = FindObjectOfType<PlayerStatsController>();
        }
        
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
        EnableAudioListener();
        InitPlayerUI();
        InitChatBehaviour();
    }

    private void DisableComponents()
    {
        foreach (Behaviour component in componentsToDisable)
            component.enabled = false;
    }
    
    private void EnableAudioListener()
    {
        foreach (Behaviour component in componentsToDisable)
        {
            if (component is Camera)
                component.GetComponent<AudioListener>().enabled = true;
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

        Destroy(playerUIInstance);
        
    }

    private string DefinePlayerName()
    {
        var usedPlayerNames = playerStatsController.GetPlayerNames();
        var playerName = PlayerPrefs.GetString("Username", "Player" + GetComponent<NetworkIdentity>().netId);
        
        // Check if player name is used
        while (usedPlayerNames.Contains(playerName))
        {
            playerName += "1";
        }
        
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
    
    private void RegisterPlayerStats(string playerName)
    {
        if (playerStatsController == null)
        {
            Debug.LogError("PlayerStatsController not found");
            return;
        }
        
        // Ajout du joueur aux statistiques
        playerStatsController.CmdAddPlayer(playerName, netId);
    }
    
}