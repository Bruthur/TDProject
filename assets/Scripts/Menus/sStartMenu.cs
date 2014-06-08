using UnityEngine;
using System.Collections;

public class sStartMenu : MonoBehaviour {

	//GUI variables
	int iButtonWidth;
	int iButtonHeight;
	string hostGameButton;
	string joinGameButton;
	string exitGameButton;

	public sPlayerSettings playerSettings;
	public sGUIData GUIData;
	public GameObject joinMenu;
	public GameObject hostMenu;
	public string sPlayerName = "MyName";

	void Awake(){
		//On Awake, load data from GUIData
		iButtonWidth = GUIData.startMenuButtonWidth;
		iButtonHeight = GUIData.startMenuButtonHeight;
		hostGameButton = GUIData.startMenuHostGameButton;
		joinGameButton = GUIData.startMenuJoinGameButton;
		exitGameButton = GUIData.startMenuExitGameButton;
	}
	

	void OnGUI () {
		//First area with textfield for player's name and 2 buttons, host and join
		GUILayout.BeginArea(new Rect (Screen.width/2 - iButtonWidth/2, Screen.height/2, iButtonWidth, iButtonHeight * 4));
		sPlayerName = GUILayout.TextField (sPlayerName, 30);

		if (GUILayout.Button (hostGameButton)) {
			playerSettings.SetPlayerName(sPlayerName); 
			hostMenu.SetActive(true);
			this.gameObject.SetActive(false);
		}

		if (GUILayout.Button (joinGameButton)) {
			playerSettings.SetPlayerName(sPlayerName); 	
			joinMenu.SetActive(true);
			this.gameObject.SetActive(false);

		}
		GUILayout.EndArea();

		//second area at the bottom of the screen for the exit button
		GUILayout.BeginArea(new Rect (Screen.width/2 - iButtonWidth/2, Screen.height * 0.7f - iButtonHeight/2, iButtonWidth, iButtonHeight));
		if (GUILayout.Button (exitGameButton)) {
						Application.Quit ();
				}
		GUILayout.EndArea();
	}
}