using System;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] private float skyboxRotationSpeed = 0.5f;

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

    private void Start()
    {
        sceneCamera.GetComponent<AudioListener>().enabled = false;
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
        skybox.SetFloat("_Rotation", rotation + skyboxRotationSpeed * Time.deltaTime);
    }

    public static void SetSceneCameraActive(bool isActive)
    {
        if(Instance.sceneCamera == null)
        {
            return;
        }

        Instance.sceneCamera.SetActive(isActive);
    }

    [Command(requiresAuthority = false)]
    public void CmdNotifyTimerFinished()
    {
        RpcOnTimerFinished();
    }

    [ClientRpc]
    public void RpcOnTimerFinished()
    {
        var playerUI = FindObjectOfType<PlayerUI>();
        
        if (playerUI != null)
        {
            playerUI.TimerFinished();
        }

    }
    
    private void OnDestroy()
    {
        var skybox = RenderSettings.skybox;
        skybox.SetFloat("_Rotation", 0);
    }
}