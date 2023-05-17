using Interfaces;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour, ITimerObserver
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject sceneCamera;
    
    [SerializeField] private AudioSource backgroundMusic;
    
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

    public void UpdateTimer(string time)
    {
    }

    public void OnTimerFinished()
    {
        if (isServer)
        {
            CmdNotifyTimerFinished();
        }
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
        
        if (timer == null)
        {
            Debug.LogError("Timer is null");
            return;
        }
        
        timer.AddObserver(this);
        timer.StartTimer();
        FindObjectOfType<MessageBroadcaster>().StartMessageLoop();
    }

    public Timer GetTimer()
    {
        return timer;
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
        backgroundMusic.Stop();
        
        var playerUI = FindObjectOfType<PlayerUI>();

        if (playerUI != null)
        {
            playerUI.TimerFinished();
        }

    }
}