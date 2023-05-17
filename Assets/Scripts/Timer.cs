using System;
using System.Collections.Generic;
using Interfaces;
using Mirror;
using UnityEngine;

public class Timer : NetworkBehaviour, ITimerObservable
{
    [SyncVar(hook = nameof(OnTimeLeftChanged))]
    [SerializeField] private float timeLeft = 150f;

    [SerializeField] private string timeFormat = @"mm\:ss";
    
    private bool isTimerRunning = false;
    
    private List<ITimerObserver> observers = new List<ITimerObserver>();

    private void Update()
    {
        if (isServer && isTimerRunning)
        {
            if (timeLeft > 1)
            {
                SetTimeLeft(timeLeft -= Time.deltaTime);
            }
            else
            {
                isTimerRunning = false;
                SetTimeLeft(timeLeft);
                Debug.Log("Timer has finished");
                
                observers.ForEach(observer => observer.OnTimerFinished());
                
            }
        }
    }
    
    private void OnTimeLeftChanged(float oldValue, float newValue)
    {
        // This method is called on the clients when the value of timeLeft changes on the server
        // You can add code here to handle the change in value
        NotifyObservers();
    }
    
    [Server]
    private void SetTimeLeft(float newValue)
    {
        timeLeft = newValue;
    }
    
    public void StartTimer()
    {
        isTimerRunning = true;
        NotifyObservers();
    }

    public bool IsTimerFinished()
    {
        return timeLeft <= 0;
    }
    
    public void AddObserver(ITimerObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(ITimerObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        observers.ForEach(observer => observer.UpdateTimer(GetTimeLeftText()));
    }

    public string GetTimeLeftText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeLeft);
        return timeSpan.ToString(timeFormat);
    }
}
