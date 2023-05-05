using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mirror;

public class GameManager : NetworkBehaviour
{
    
    [SerializeField] private float skyboxRotationSpeed = 1f;

    public static GameManager Instance;

    [SerializeField]
    private GameObject sceneCamera;
    
    [SerializeField] private Timer timer;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }

        Debug.LogError("More than one GameManager in scene.");
    }

    public override void OnStartServer()
    {
        // Start game after 1 second
        Invoke(nameof(StartGame), 1f);
    }

    public void StartGame()
    {
        timer.StartTimer();
        FindObjectOfType<MessageBroadcaster>().StartMessageLoop();
    }

    private void Update()
    {
        MoveSkybox();
    }
    
    public Timer GetTimer()
    {
        return timer;
    }

    private void MoveSkybox()
    {
        var skybox = RenderSettings.skybox;
        var rotation = skybox.GetFloat("_Rotation");
        skybox.SetFloat("Rotation", rotation + skyboxRotationSpeed * Time.deltaTime);
    }

    public static void SetSceneCameraActive(bool isActive)
    {
        if(Instance.sceneCamera == null)
        {
            return;
        }

        Instance.sceneCamera.SetActive(isActive);
    }

}
