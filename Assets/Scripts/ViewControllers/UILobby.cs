using Network;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
	    [SerializeField] Canvas UICanvas;
	    [SerializeField] Canvas lobbyCanvas;

	    [Header("Lobby")] 
	    [SerializeField] Transform UIPlayerParent;

	    [SerializeField] GameObject PlayerFramePrefab;
	    [SerializeField] TMP_Text matchIdText;
	    [SerializeField] GameObject beginGameButton;

	    void Start()
	    {
		    instance = this;
	    }
	    
	    public void Host()
	    {
		    EnableComponents(false);

		    PlayerNetwork.localPlayerNetwork.HostGame();
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
		    matchIdText.text = PlayerNetwork.localPlayerNetwork.matchID;
		    beginGameButton.SetActive(true);
	    }
	    
	    public void Join()
	    {
		    EnableComponents(false);
		    
		    string matchID = matchIdInputField.text;
		    
		    PlayerNetwork.localPlayerNetwork.JoinGame(matchID);
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
		    matchIdText.text = PlayerNetwork.localPlayerNetwork.matchID;
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

	    public void BeginGame()
	    {
		    lobbyCanvas.enabled = false;
		    UICanvas.enabled = false;
		    
		    PlayerNetwork.localPlayerNetwork.BeginGame();
	    }
    }
}

