using System;
using Mirror;
using UnityEngine;

public class Timer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTimeLeftChanged))]
    [SerializeField] private float timeLeft = 150f;
    
    private bool isTimerRunning = false;
    
    private void Update()
    {
        if (isServer && isTimerRunning)
        {
            if (timeLeft > 0)
            {
                SetTimeLeft(timeLeft -= Time.deltaTime);
            }
            else
            {
                isTimerRunning = false;
                SetTimeLeft(timeLeft);
                Debug.Log("Timer has finished");
            }
        }
    }
    
    private void OnTimeLeftChanged(float oldValue, float newValue)
    {
        // This method is called on the clients when the value of timeLeft changes on the server
        // You can add code here to handle the change in value
        foreach (var phoneController in GetComponent<GameManager>().GetPhoneControllers())
        {
            phoneController.Value.SetTimeText(GetTimeLeftText());
        }
    }
    
    [Server]
    private void SetTimeLeft(float newValue)
    {
        timeLeft = newValue;
    }
    
    public void StartTimer()
    {
        isTimerRunning = true;
    }
    
    public string GetTimeLeftText()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timeLeft);
        return timeSpan.ToString(@"mm\:ss");
    }
}
