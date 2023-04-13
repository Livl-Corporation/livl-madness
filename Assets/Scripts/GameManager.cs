using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();
    private static Dictionary<string, PhoneController> phoneControllers = new Dictionary<string, PhoneController>();
    
    [SerializeField] private float skyboxRotationSpeed = 1f;
    
    //public MatchSettings matchSettings;

    public static GameManager instance;

    [SerializeField]
    private GameObject sceneCamera;

    public delegate void OnPlayerKilledCallback(string player, string source);
    public OnPlayerKilledCallback onPlayerKilledCallback;

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
    
    private void Update()
    {
        MoveSkybox();
    }
    
    private void MoveSkybox()
    {
        var skybox = RenderSettings.skybox;
        var rotation = skybox.GetFloat("_Rotation");
        skybox.SetFloat("_Rotation", rotation + skyboxRotationSpeed * Time.deltaTime);
        
        // move the directional light as well
        var light = GameObject.Find("Directional Light");
        if(light != null)
        {
            light.transform.Rotate(Vector3.up, skyboxRotationSpeed * Time.deltaTime);
        }
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if(sceneCamera == null)
        {
            return;
        }

        sceneCamera.SetActive(isActive);
    }
    
    public static void RegisterPlayer(uint netID, Player player)
    {
        var playerId = MakePlayerId(netID);
        players.Add(playerId, player);
        player.transform.name = playerId;
    }
    
    public static void RegisterPhoneController(uint netID, PhoneController controller)
    {
        phoneControllers.Add(MakePlayerId(netID), controller);
        
        // TODO : remove test
        controller.messageController.ShowMessage("Jean Marc Muller", "Salut, ça va ?");
    }

    public static void UnregisterPlayer(string playerId)
    {
        players.Remove(playerId);
        phoneControllers.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        return players[playerId];
    }

    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }
}
