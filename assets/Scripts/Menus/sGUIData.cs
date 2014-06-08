using UnityEngine;
using System.Collections;

public class sGUIData : MonoBehaviour {
	
	//sGUIData should contain any string, texture, etc. for GUI
	//In the futur, may import values from an extern file

	//GUIData must survive until the game's end
	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	//Start menu
	public int startMenuButtonWidth = 120;
	public int startMenuButtonHeight = 30;
	public string startMenuHostGameButton = "Host Game";
	public string startMenuJoinGameButton = "join Game";
	public string startMenuExitGameButton = "exit Game";

	//Host Menu
	public int hostMenuButtonWidth = 120;
	public int hostMenuButtonHeight = 30;
	public string hostMenuStartServerButton = "Start Server";
	public string hostMenuReturnButton = "Return to Start Menu";

	//Join Menu
	public int joinMenuButtonWidth = 120;
	public int joinMenuButtonHeight = 30;
	public string joinMenuRefreshHostButton = "Refresh Host";
	public string joinMenuPasswordInfo = "Game's password :";
	public string joinMenuReturnButton = "Return to Start Menu";

	//Lobby Menu
	public int lobbyMenuButtonWidth = 120;
	public int lobbyMenuButtonHeight = 30;
	public string lobbyMenuSwapRolesButton = "Swap roles";
	public string lobbyMenuReadyButton = "Ready !";
	public string lobbyMenuLaunchButton = "Launch !";
	public string lobbyMenuExitButton = "Exit !";

	//InGameMenu
	public int inGameMenuButtonWidth = 120;
	public int inGameMenuButtonHeight = 30;
	public string inGameMenuPauseButton = "Pause Game";
	public string inGameMenuResumeButton = "Resume Game";
	public string inGameMenuSwapRolesButton = "Swap roles";
	public string inGameMenuSaveButton = "Save Game";
	public string inGameMenuSettingsButton = "Settings";
	public string inGameMenuDisconnectButton = "Disconnect";
	public string inGameMenuExitGameButton = "Exit Game";
	public string inGameMenuSwapRolesInfo = "A swap roles as been requested, swap ?";
	public string inGameMenuSwapRolesYesButton = "Ok";
	public string inGameMenuSwapRolesNoButton = "No way";
	public string inGameMenuPauseInfo = "The game as been paused";
}
