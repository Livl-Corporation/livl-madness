using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TMP_Text initialsText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text messageText;

    [Header("Config")]
    [SerializeField] private int defaultMessageDuration = 5;

    void Start()
    {
        hideMessage();
    }

    public void showMessage(string name, string message)
    {
        messagePanel.SetActive(true);

        // Split name & lastname
        string[] nameParts = name.Split(' ');

        // Make initials from first letter of each part
        if (nameParts.Length > 1)
        {
            initialsText.text = nameParts[0][0].ToString() + nameParts[1][0].ToString();
        }
        else
        {
            initialsText.text = name.Substring(0, 2);
        }

        nameText.text = name;
        messageText.text = message;
        StartCoroutine(delayedHideMessage(defaultMessageDuration));
    }

    private IEnumerator delayedHideMessage(int duration)
    {
        yield return new WaitForSeconds(duration);
        hideMessage();
    }

    public void hideMessage()
    {
        messagePanel.SetActive(false);
    }

}
