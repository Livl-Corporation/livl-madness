using Interfaces;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : NetworkBehaviour, ITimerObserver
{
    public static GameManager Instance;

    [SerializeField]
    private GameObject sceneCamera;
    
    [SerializeField] private AudioSource backgroundMusic;
    
    [SerializeField] private Timer gameTimer;
    [SerializeField] private Timer startTimer;
    private bool hasGameStarted = false;

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
        if (!isServer) return;
        
        if (hasGameStarted)
        {
            CmdNotifyTimerFinished();
            return;
        }
        
        hasGameStarted = true;
        StartGame();
        
    }

    private void Start()
    {
        sceneCamera.GetComponent<AudioListener>().enabled = false;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        startTimer.AddObserver(this);
        
        // Start timer after 1s delay
        Invoke(nameof(StartStartTimer), 1);
    }
    
    private void StartStartTimer()
    {
        startTimer.StartTimer();
    }
    
    public Timer GetStartTimer()
    {
        return startTimer;
    }

    public void StartGame()
    {
        
        if (gameTimer == null)
        {
            Debug.LogError("Timer is null");
            return;
        }
        
        gameTimer.AddObserver(this);
        gameTimer.StartTimer();
        FindObjectOfType<MessageBroadcaster>().StartMessageLoop();
        CmdGameStart();
    }

    [Command(requiresAuthority = false)]
    public void CmdGameStart()
    {
        RpcGameStart();
    }
    
    [ClientRpc]
    public void RpcGameStart()
    {
        backgroundMusic.Play();
        var startTimerView = FindObjectOfType<StartTimerView>();
        if (startTimerView == null)
        {
            Debug.LogError("StartTimerView not found");
            return;
        }
        startTimerView.OnTimerFinished();
    }

    public Timer GetTimer()
    {
        return gameTimer;
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