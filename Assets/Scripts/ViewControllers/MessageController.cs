using System;
using System.Collections;
using Mirror;
using Models;
using UnityEngine;
using TMPro;

public class MessageController : NetworkBehaviour
{
    [Header("Message Components")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text messageInitials;
    [SerializeField] private TMP_Text messageName;
    [SerializeField] private TMP_Text messageText;
    
    [Header("Config")]
    [SerializeField] private int messageDuration = 5;
    [SerializeField] private int messageHiddenMultiplier = 0;
    [SerializeField] private int messageShownMultiplier = 0;
    [SerializeField] private float messageTransitionTime = 1.0f;
    
    private PlayerUI playerUi;
    
    public void AddPlayerUI(PlayerUI _playerUi)
    {
        playerUi = _playerUi;
    }

    // Start is called before the first frame update
    void Start()
    {
        HideMessage(false);
    }
    
    public void ReceiveMessage(PhoneMessage message)
    {
        Debug.Log("Message received : " + message.Sender + " : " + message.Message + " !");
        
        // Split name & lastname
        string[] nameParts = message.Sender.Split(' ');

        // Make initials from first letter of each part
        if (nameParts.Length > 1)
        {
            messageInitials.text = nameParts[0][0].ToString() + nameParts[1][0].ToString();
        }
        else
        {
            messageInitials.text = name.Substring(0, 2);
        }

        messageName.text = message.Sender;
        messageText.text = message.Message;
        
        // Start animation
        messagePanel.transform.LeanMoveLocal(messagePanel.transform.up * messageShownMultiplier, messageTransitionTime).setEaseOutQuint();
        
        // Hide message after a while
        StartCoroutine(DelayedHideMessage(messageDuration));

        // Play the message notification sound
        if(playerUi.IsActualPlayer())
        {
            playerUi.PlayNotificationSound();
        } 
    }

    private IEnumerator DelayedHideMessage(int duration)
    {
        yield return new WaitForSeconds(duration);
        HideMessage();
    }

    public void HideMessage(bool animated = true)
    {
        if (!animated)
        {
            messagePanel.transform.localPosition = messagePanel.transform.up * messageHiddenMultiplier;
            return;
        }
        
        messagePanel.transform.LeanMoveLocal(messagePanel.transform.up * messageHiddenMultiplier, messageTransitionTime).setEaseInQuint();
    }
}
