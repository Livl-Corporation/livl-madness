using System.Collections;
using System.Collections.Generic;
using Interfaces;
using TMPro;
using UnityEngine;

public class StartTimerView : MonoBehaviour, ITimerObserver
{
    
    [SerializeField] private TMP_Text startTimerText;
    [SerializeField] private ITimerObservable timerObservable;
    
    public void UpdateTimer(string time)
    {
        startTimerText.text = time;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (timerObservable != null)
        {
            timerObservable.AddObserver(this);
        }   
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
