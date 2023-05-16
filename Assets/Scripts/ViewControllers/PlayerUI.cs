using UnityEngine;
using Mirror;
using Models;
using TMPro;

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
    [SerializeField] private MessageController messageController;
    
    [SerializeField] private GameObject leaderboardCanvas;
    [SerializeField] private Leaderboard leaderboard;
    
    [Header("Chat system")]
    [SerializeField] public GameObject chatPanel;
    [SerializeField] public TMP_Text chatText;
    [SerializeField] public TMP_InputField chatInput;
    public static bool isPaused
    {
        get => paused;
        set => paused = value;
    }
    public static bool isEndGame => endGame;

    private void Awake()
    {
        phoneController.AddPlayerUI(this);
    }

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        Debug.Log("isOwned" + isOwned);
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
        if (Input.GetKeyDown(KeyCode.Escape) && !endGame && !chatInput.isFocused)
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
