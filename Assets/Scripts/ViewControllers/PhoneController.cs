using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;
using TMPro;

public class PhoneController : MonoBehaviour, ITimerObserver
{

    [Header("Phone Components")]
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text screenTitle;
    [SerializeField] public MessageController messageController;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private ProductListController productListController;
    
    [Header("Screens")]
    [SerializeField] private GameObject productListScreen;
    [SerializeField] private GameObject pauseScreen;
    
    [Header("Config")]
    [SerializeField] private float screenTransitionTime = 1.0f;

    private GameObject currentScreen;
    private Dictionary<Phone.Screen, GameObject> screens = new Dictionary<Phone.Screen, GameObject>();
    
    private float lastCoroutineTime;

    public ProductListController GetProductListController()
    {
        return productListController;
    }

    public void AddPlayerUI(PlayerUI _playerUi)
    {
        messageController.AddPlayerUI(_playerUi);
    }

    void Start()
    {   
        GenerateScreens();
        
        SetScoreText("0");

        // Hide all screens
        foreach (GameObject screen in screens.Values)
        {
            HideScreen(screen, false);
        }

        // Start on the product list screen
        Navigate(Phone.Screen.ProductList);
        
        // TODO : Remove test message
        messageController.ShowMessage("Jean Marc Muller", "Salut, ça va ?");
        
        // Subscribe to timer
        var timer = GameManager.Instance.GetTimer();
        if (timer == null)
        {
            Debug.LogError("Timer is null");
            return;
        }
        
        timer.AddObserver(this);

    }

    private void GenerateScreens()
    {
        screens.Add(Phone.Screen.ProductList, productListScreen);
        screens.Add(Phone.Screen.Pause, pauseScreen);
    }

    // TODO : Move message to a separate script
    

    public void SetScreenTitle(string title)
    {
        screenTitle.text = title;
    }

    public void UpdateTimer(string time)
    {
        timeText.text = time;
    }

    public void Navigate(Phone.Screen screen)
    {

        // Check if the screen exists
        if (!screens.ContainsKey(screen))
        {
            throw new System.Exception("Screen " + screen.ToString() + " does not exist");
        }
        
        var animationFinished = Time.time - lastCoroutineTime > screenTransitionTime;
        // Cancel current hide coroutine if any
        if (!animationFinished)
        {
            StopAllCoroutines();
        }

        // Hide the previous screen
        if (currentScreen != null)
        {
            HideScreen(currentScreen, animationFinished);
        }

        // Show the new screen
        currentScreen = screens[screen];
        ShowScreen(currentScreen);
        SetScreenTitle(Phone.ScreenTitles[screen]);
    }

    private void HideScreen(GameObject screen, bool animated = true)
    {
        screen.transform.localScale = new Vector3(0, 0, 0);

        if (!animated)
        {
            screen.SetActive(false);
            return;
        }

        lastCoroutineTime = Time.time;
        var coroutine = DelayedSetActive(screen, false, screenTransitionTime);
        StartCoroutine(coroutine);
    }
    
    private void ShowScreen(GameObject screen)
    {
        screen.SetActive(true);
        screen.LeanScale(new Vector3(1, 1, 1), screenTransitionTime).setEaseOutQuint();
    }
    
    private IEnumerator DelayedSetActive(GameObject obj, bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(active);
    }

    public void SetScoreText(string score)
    {
        scoreText.text = score;
    }

    private void OnDestroy()
    {
        var timer = GameManager.Instance.GetTimer();
        if (timer == null)
        {
            Debug.LogError("Timer is null");
            return;
        }
        
        timer.RemoveObserver(this);
    }
}
