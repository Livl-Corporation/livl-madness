using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
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
        var playerUI = FindObjectOfType<PlayerUI>();
        
        if (playerUI != null)
        {
            playerUI.TimerFinished();
        }

    }
}