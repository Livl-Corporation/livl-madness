using UnityEngine;
using Mirror;
using Models;
using TMPro;

public class PlayerUI : NetworkBehaviour
{

    private static bool paused = false;
    private static bool inGame = false;
    private static bool captureCursor = true;
    
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
    [SerializeField] private GameObject controls;

    
    [Header("Chat system")]
    [SerializeField] public GameObject chatPanel;
    [SerializeField] public TMP_Text chatText;
    [SerializeField] public TMP_InputField chatInput;
    public static bool isPaused
    {
        get => paused;
        set => paused = value;
    }
    public static bool isInGame => inGame;

    public static bool isCursorCaptured
    {
        get => captureCursor;
    }
    
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

    public void StartGame()
    {
        inGame = true;
    }
    
    public void TimerFinished()
    {
        controls.SetActive(false);
        paused = true;
        inGame = false;
        captureCursor = false;
        pauseOverlay.SetActive(true);
        CollapsePhone();
        OpenLeaderboard();
    }

    private void OpenLeaderboard()
    {
        
        // save previous leaderboard scale 
        var previousScale = leaderboardCanvas.transform.localScale;
        
        // set leaderboard scale to 0
        leaderboardCanvas.transform.localScale = Vector3.zero;
        
        // set leaderboard opacity to 0
        var canvasGroup = leaderboardCanvas.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        
        // set leaderboard active
        leaderboardCanvas.SetActive(true);
        
        // animate leaderboard
        LeanTween.scale(leaderboardCanvas, previousScale, 0.5f).setEaseOutExpo();
        LeanTween.alphaCanvas(canvasGroup, 1, 0.5f).setEaseOutExpo();
    }

    private void CollapsePhone()
    {
        
        smartphoneCanvas.LeanMoveY(-500, 1f).setEaseOutExpo();
        
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
        if (Input.GetKeyDown(KeyCode.Escape) && isInGame && !chatInput.isFocused)
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
