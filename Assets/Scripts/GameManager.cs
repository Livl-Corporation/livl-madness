using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private static readonly Dictionary<string, PhoneController> phoneControllers = new Dictionary<string, PhoneController>();
    
    [SerializeField] private float skyboxRotationSpeed = 1f;

    public static GameManager instance;

    [SerializeField]
    private GameObject sceneCamera;
    
    [SerializeField]
    private ScanListController scanListController;

    public delegate void OnPlayerKilledCallback(string player, string source);
    
    public OnPlayerKilledCallback onPlayerKilledCallback;
    
    [SerializeField] private Timer timer;
    
    //public MatchSettings matchSettings;

    private static string MakePlayerId(uint netId)
    {
        return playerIdPrefix + netId;
    }
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }

        Debug.LogError("More than one GameManager in scene.");
    }

    private void Start()
    {
        timer.StartTimer();
    }
    
    private void Update()
    {
        MoveSkybox();
    }
    
    public Dictionary<string, PhoneController> GetPhoneControllers()
    {
        return phoneControllers;
    }
    
    private void MoveSkybox()
    {
        var skybox = RenderSettings.skybox;
        var rotation = skybox.GetFloat("_Rotation");
        skybox.SetFloat("_Rotation", rotation + skyboxRotationSpeed * Time.deltaTime);
        
        // move the directional light as well
        //var light = GameObject.Find("Directional Light");
        //if(light != null)
        //{
        //    light.transform.Rotate(Vector3.up, skyboxRotationSpeed * Time.deltaTime);
        //}
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if(sceneCamera == null)
        {
            return;
        }

        sceneCamera.SetActive(isActive);
    }
    
    public static void RegisterPlayer(uint netID, Player player, PhoneController controller)
    {
        var playerId = MakePlayerId(netID);
        players.Add(playerId, player);
        phoneControllers.Add(playerId, controller);
        player.transform.name = playerId;
        
        // Add timer observable
        if (instance.timer != null)
        {
            instance.timer.AddObserver(controller);
        }
        
        // TODO : remove test
        controller.messageController.ShowMessage("Jean Marc Muller", "Salut, ça va ?");

    }
    
    public static void UnregisterPlayer(string playerId)
    {
        var controller = phoneControllers[playerId];
        if (instance.timer != null)
        {
            instance.timer.RemoveObserver(controller);
        }
        
        players.Remove(playerId);
        phoneControllers.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        return players[playerId];
    }
    
}
