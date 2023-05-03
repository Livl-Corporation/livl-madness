using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UILobby : MonoBehaviour
	{
		public static UILobby instance;
	    
	    [SerializeField] TMP_InputField matchIdInputField;
	    [SerializeField] Button joinButton;
	    [SerializeField] Button hostButton;
	    [SerializeField] Canvas lobbyCanvas;

	    void Start()
	    {
		    instance = this;
	    }
	    
	    public void Host()
	    {
		    EnableComponents(false);

		    Network.PlayerNetwork.localPlayerNetwork.HostGame();
	    }

	    public void HostSuccess(bool success)
	    {
		    if (!success)
		    {
			    EnableComponents(true);
			    return;
		    }
		    lobbyCanvas.enabled = true;
	    }
	    
	    public void Join()
	    {
		    EnableComponents(false);

		    string matchID = matchIdInputField.text;
		    
		    Network.PlayerNetwork.localPlayerNetwork.JoinGame(matchID);
	    }

	    public void JoinSuccess(bool success)
	    {
		    if (!success)
		    {
			    EnableComponents(true);
			    return;
		    }
		    
		    lobbyCanvas.enabled = true;
	    } 
	    
	    public void EnableComponents(bool enabled)
	    {
		    matchIdInputField.enabled = enabled;
		    joinButton.enabled = enabled;
		    hostButton.enabled = enabled;
	    }
    }
}

