using Network;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class UILobby : MonoBehaviour
	{
		public static UILobby instance;
	    
		[Header("Host Join")]
	    [SerializeField] TMP_InputField matchIdInputField;
	    [SerializeField] Button joinButton;
	    [SerializeField] Button hostButton;
	    [SerializeField] Canvas lobbyCanvas;

	    [Header("Lobby")] 
	    [SerializeField] Transform UIPlayerParent;

	    [SerializeField] private GameObject PlayerFramePrefab;
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

		    SpawnPlayerFramePrefab(PlayerNetwork.localPlayerNetwork);
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

		    SpawnPlayerFramePrefab(PlayerNetwork.localPlayerNetwork);
	    } 
	    
	    public void EnableComponents(bool enabled)
	    {
		    matchIdInputField.enabled = enabled;
		    joinButton.enabled = enabled;
		    hostButton.enabled = enabled;
	    }
	    
	    public void SpawnPlayerFramePrefab(PlayerNetwork player)
	    {
		    GameObject newFrame = Instantiate(PlayerFramePrefab, UIPlayerParent);
		    newFrame.GetComponent<PlayerFrame>().SetPlayer(player);
	    }
    }
}

