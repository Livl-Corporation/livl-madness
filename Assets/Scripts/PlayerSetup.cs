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

    [SerializeField] private Player player;
    
    [SerializeField]
    private PlayerStatsController playerStatsController;
    
    Camera sceneCamera;
    
    // Start is called before the first frame update
    private void Start()
    {
        
        player = GetComponent<Player>();
        
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            // Création du UI du joueur local
            playerUIInstance = Instantiate(playerUIPrefab);
            
            // Configuration du UI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if(ui == null)
            {
                Debug.LogError("Pas de component PlayerUI sur playerUIInstance");
            }
            else
            {
                ui.SetPlayer(player);
            }

            // Ajout du joueur au GameManager
            GameManager.RegisterPlayer(player.GetNetId(), player, ui.GetPhoneController());
            
            // Configuration du joueur
            player.Setup();
        }
        
        // Ajout du joueur aux statistiques
        playerStatsController.CmdAddPlayer(Player.LocalPlayerName);
        
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
        
        playerStatsController.CmdRemovePlayer(Player.LocalPlayerName);
        
        if (!isLocalPlayer)
            return;
        
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);

        Destroy(playerUIInstance);
        GameManager.UnregisterPlayer(transform.name);
        
    }
    
}
