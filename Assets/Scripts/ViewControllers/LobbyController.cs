
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
    public Button hostButton, quitButton;
	    
	[Header("Input")]
	[SerializeField] public TMP_InputField usernameInput;

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
	    {
		    hostButton.gameObject.SetActive(false);
	    }
	    
	    if (Application.platform == RuntimePlatform.WebGLPlayer || Application.isEditor )
	    {
		    quitButton.gameObject.SetActive(false);
	    }
	    
	    if (!Application.isPlaying) return;
	    var username = PlayerPrefs.GetString("Username");
	    if (!string.IsNullOrEmpty(username))
		    usernameInput.text = username;

    }

	private void Update()
	{
		hourText.text = DateTime.Now.ToString("HH:mm");
	}

	public void OnUsernameChanged(string username)
	{
		
		if (string.IsNullOrWhiteSpace(username))
		{
			PlayerPrefs.DeleteKey("Username");
			return;
		}
		
		PlayerPrefs.SetString("Username", username);
	}

	public void ButtonHost()
    {
		SetError("");
		
		try
		{
			NetworkManager.singleton.StartHost();
		}
		catch (Exception e)
		{
			SetError("Erreur : " + e.Message);	
			Debug.LogError(e);
		}
		
    }
	
    public void ButtonClient()
    {   
	    SetError("");
		
		try
		{
			NetworkManager.singleton.StartClient();
		}
		catch (Exception e)
		{
			SetError("Erreur : " + e.Message);
			Debug.LogError(e);
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

	public void ButtonQuit()
	{
		Application.Quit();
	}
    
}
