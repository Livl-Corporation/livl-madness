
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
	
	[Header("Buttons")]
	[SerializeField]
    public Button hostButton;

    [Header("Texts")]
	[SerializeField]
	public TMP_Text errorText, hourText;

	[Header("Other")] [SerializeField] public NetworkManagerHUD networkManagerHUD;
	
	private void Start()
    {
	    
	    // Parse command line argument for server builds
	    string[] args = System.Environment.GetCommandLineArgs();

	    foreach (string argument in args)
	    {
		    if (argument == "--server")
		    {
			    NetworkManager.singleton.StartServer();
			    return;
		    }
	    }
	    
	    SetError("");
	    
		if (Application.platform == RuntimePlatform.WebGLPlayer)
			hostButton.gameObject.SetActive(false);
		
    }

	private void Update()
	{
		hourText.text = DateTime.Now.ToString("HH:mm");
	}

	public void OnUsernameChanged(string username)
	{
		
		if (string.IsNullOrEmpty(username))
		{
			PlayerPrefs.DeleteKey("Username");
			return;
		}
		
		PlayerPrefs.SetString("Username", username);
		Debug.Log("Username changed to " + username);
	}

	public void ButtonHost()
    {
		SetError("");
		
		try
		{
			NetworkManager.singleton.StartHost();
		}
		catch (System.Exception e)
		{
			SetError("An error happened : " + e.Message);
		}
		
    }
	
    public void ButtonClient()
    {   
	    SetError("");
		
		try
		{
			NetworkManager.singleton.StartClient();
		}
		catch (System.Exception e)
		{
			SetError("An error happened : " + e.Message);
		}
		
    }
    
	public void ButtonSettings()
	{
		networkManagerHUD.enabled = !networkManagerHUD.enabled;
	}

	private void SetError(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			errorText.enabled = false;
			return;
		}
		
		errorText.enabled = true;
		errorText.text = text;
	}
    
}
