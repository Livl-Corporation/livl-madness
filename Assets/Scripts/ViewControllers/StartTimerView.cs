using System.Collections;
using System.Collections.Generic;
using Interfaces;
using Mirror;
using TMPro;
using UnityEngine;

public class StartTimerView : MonoBehaviour, ITimerObserver
{
    
    [SerializeField] private TMP_Text startTimerText;
    
    public void UpdateTimer(string time)
    {
        startTimerText.text = time;
    }

    // Start is called before the first frame update
    void Start()
    {
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
        StartCoroutine(DelayedDestroy());
    }
    
    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
