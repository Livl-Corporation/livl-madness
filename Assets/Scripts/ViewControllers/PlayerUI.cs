using UnityEngine;
using Mirror;

public class PlayerUI : NetworkBehaviour
{

    private static bool paused = false;
    
    private Player player;
    private NetworkManager networkManager;
    private PlayerController controller;

    [Header("Components")]
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private PhoneController phoneController;
    [SerializeField] private GameObject smartphoneCanvas;

    public static bool isPaused
    {
        get => paused;
    }

    private void Awake()
    {
        phoneController.AddPlayerUI(this);
    }

    private void Start()
    {
        networkManager = NetworkManager.singleton;
        
        pauseOverlay.SetActive(false);
        smartphoneCanvas.SetActive(true);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }
    
    public void TogglePauseMenu()
    {
        SetPauseMenuVisibility(!isPaused);
    }   
    
    public void SetPauseMenuVisibility(bool visible)
    {
        PlayerUI.paused = visible;
        pauseOverlay.SetActive(visible);
        if (isPaused)
        {
            phoneController.Navigate(Phone.Screen.Pause);
        }
        else
        {
            phoneController.Navigate(Phone.Screen.ProductList);
        }
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
