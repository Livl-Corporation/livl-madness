using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameplate : MonoBehaviour
{
    
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        SetUserName(parentTransform.name);
    }
    
    void SetUserName(string username)
    {
        playerNameText.text = username;
    }

}
