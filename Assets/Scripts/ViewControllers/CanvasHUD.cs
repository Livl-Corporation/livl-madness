
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class CanvasHUD : MonoBehaviour
{
    [SerializeField]
	public GameObject PanelStart;
    
	[SerializeField]
    public Button buttonHost, buttonServer, buttonClient;
    
	[SerializeField]
    public InputField inputFieldAddress;
	
	[SerializeField]
	public Text textInfo; 
   
    private void Start()
    {
        //Update the canvas text if you have manually changed network managers address from the game object before starting the game scene
        if (NetworkManager.singleton.networkAddress != "localhost") { inputFieldAddress.text = NetworkManager.singleton.networkAddress; }

        //Adds a listener to the main input field and invokes a method when the value changes.
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        //Make sure to attach these Buttons in the Inspector
        buttonHost.onClick.AddListener(ButtonHost);
        buttonServer.onClick.AddListener(ButtonServer);
        buttonClient.onClick.AddListener(ButtonClient);

		// Parse command line argument for server builds
		string[] args = System.Environment.GetCommandLineArgs();

		foreach (string argument in args)
		{
			if (argument == "--server")
				ButtonServer();
		}
		

        //This updates the Unity canvas, we have to manually call it every change, unlike legacy OnGUI.
        SetupCanvas();
    }

    // Invoked when the value of the text field changes.
    private void ValueChangeCheck()
    {
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;
    }

    private void ButtonHost()
    {
		textInfo.text = "Please wait. Connecting...";
		
		try
		{
			NetworkManager.singleton.StartHost();
		}
		catch (System.Exception e)
		{
			textInfo.text = "An error happened : " + e.Message;
		}
		
        SetupCanvas();
    }

    private void ButtonServer()
    {
        textInfo.text = "Please wait. Connecting...";
        
        try
        {
	        NetworkManager.singleton.StartServer();
        }
        catch (System.Exception e)
        {
	        textInfo.text = "An error happened : " + e.Message;
        }
        
        SetupCanvas();
    }

    private void ButtonClient()
    {   
		textInfo.text = "Please wait. Connecting...";
		
		try
		{
			NetworkManager.singleton.StartClient();
		}
		catch (System.Exception e)
		{
			textInfo.text = "An error happened : " + e.Message;
		}
		
        SetupCanvas();
    }

    private void SetupCanvas()
    {
        // Here we will dump majority of the canvas UI that may be changed.
		if (NetworkClient.isConnected && NetworkServer.active)
        {
			PanelStart.SetActive(false);
        }
    }
}
