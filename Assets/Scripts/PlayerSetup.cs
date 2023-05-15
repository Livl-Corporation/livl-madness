using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

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
    
    Camera sceneCamera;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (!isLocalPlayer || SceneManager.GetActiveScene().name != "Livl")
        {
            EnableComponents(false);
            AssignRemoteLayer();
        }
        else
        {
            Init();
        }
    }

    public void Init()
    {
        Debug.Log("Initialisation du Player " + (isLocalPlayer ? "local." : "distant."));
        EnableComponents(true);
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
            ui.SetPlayer(GetComponent<Player>());
        }

        var player = GetComponent<Player>();
        GameManager.RegisterPlayer(player.GetNetId(), player, ui.GetPhoneController());
        player.Setup();
        
    }
    
    [Command]
    void CmdSetUsername(string playerID, string username)
    {
        Player player = GameManager.GetPlayer(playerID);
        if(player != null)
        {
            Debug.Log(username + " has joined !");
            player.username = username;
        }
    }

    private void EnableComponents(bool state)
    {
        foreach (Behaviour component in componentsToDisable)
        {
            component.enabled = state;
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
        GameManager.UnregisterPlayer(transform.name);
        
    }
    
}
