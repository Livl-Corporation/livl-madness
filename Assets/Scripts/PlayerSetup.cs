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
    
    Camera sceneCamera;
    
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
        PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
        
        if(ui == null)
        {
            Debug.LogError("Pas de component PlayerUI sur playerUIInstance");
            return;
        }

        ui.SetPlayer(GetComponent<Player>());
        
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
