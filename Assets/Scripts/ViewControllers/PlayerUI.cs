using UnityEngine;
using Mirror;

public class PlayerUI : NetworkBehaviour
{

    private static bool paused = false;
    private static bool endGame = false;
    
    private Player player;
    private NetworkManager networkManager;
    private PlayerController controller;

    [Header("Components")]
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private GameObject smartphoneCanvas;
    
    [SerializeField] private GameObject leaderboardCanvas;
    [SerializeField] private Leaderboard leaderboard;

    public static bool isPaused => paused;
    public static bool isEndGame => endGame;

    private void Awake()
    {
        phoneController.AddPlayerUI(this);
    }

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        
        pauseOverlay.SetActive(false);
        leaderboardCanvas.SetActive(false);
        smartphoneCanvas.SetActive(true);
        paused = false;
    }
    
    public void TimerFinished()
    {
        leaderboardCanvas.SetActive(true);
        smartphoneCanvas.SetActive(false);
        paused = true;
        endGame = true;
        pauseOverlay.SetActive(true);
    }
    
    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePauseInput();
    }
    
    void HandlePauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !endGame)
        {
            TogglePauseMenu();
        }
    }
    
    public void TogglePauseMenu()
    {
        SetPauseMenuVisibility(!isPaused);
    }   
    
    private void SetPauseMenuVisibility(bool visible)
    {
        paused = visible;
        pauseOverlay.SetActive(visible);
        phoneController.Navigate(isPaused ? Phone.Screen.Pause : Phone.Screen.ProductList);
    }

    public void LeaveRoomButton()
    {
        if (isClientOnly)
        {
            networkManager.StopClient();
        }
        else
        {
            networkManager.StopHost();
        }
    }
    
    public bool IsActualPlayer()
    {
        return controller.isLocalPlayer;
    }

    public void PlayNotificationSound()
    {
        controller.PlayIOSMessageSound();
    }
}
