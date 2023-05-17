using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Mirror;
using TMPro;
using UnityEngine;

public class StartTimerView : MonoBehaviour, ITimerObserver
{
    
    [SerializeField] private TMP_Text startTimerText;
    
    [SerializeField] private AudioClip timerCountdownSound;
    [SerializeField] private AudioClip timerFinishedSound;
    
    private AudioSource audioSource;
    
    public void UpdateTimer(string time)
    {
        
        if (startTimerText.text == time) return;
        
        startTimerText.text = time;
        audioSource.PlayOneShot(timerCountdownSound);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        var gameManager = FindObjectOfType<GameManager>();
        
        if (gameManager == null)
        {
            Debug.LogError("StartTimerView: GameManager not found");
            return;
        }
        
        gameManager.GetStartTimer().AddObserver(this);
    }
    
    public void OnTimerFinished()
    {
        startTimerText.text = "GO";
        audioSource.PlayOneShot(timerFinishedSound);
        
        // animate opatity to 0
        var canvasGroup = startTimerText.GetComponent<CanvasGroup>();

        if (canvasGroup != null)
        {
            canvasGroup.LeanAlpha(0, 1).setEaseInOutQuint();
        }
        
        StartCoroutine(DelayedDestroy());
    }
    
    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
