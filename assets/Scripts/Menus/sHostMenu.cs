using UnityEngine;
using System.Collections;

public class sHostMenu : MonoBehaviour {

	private string sGameName;
	const string sTypeName = "UltimateTD";
	private string sGamePassword = "";

	public sPlayerSettings playerSettings;
	public GameObject startMenu;
	public GameObject gameLobbyMenu;
	public sGUIData GUIData;

	//GUI variables
	int iButtonWidth;
	int iButtonHeight;
	string startServerButton;
	string returnButton;

	void Awake (){
		sGameName = playerSettings.GetPlayerName() + "'s game";
		//on awake, load datafrom GUIData
		iButtonWidth = GUIData.hostMenuButtonWidth;
		iButtonHeight = GUIData.hostMenuButtonHeight;
		startServerButton = GUIData.hostMenuStartServerButton;
		returnButton = GUIData.hostMenuReturnButton;
	}


	void OnGUI()
	{
		GUILayout.BeginArea(new Rect (Screen.width/2 - iButtonWidth/2, Screen.height/2, iButtonWidth, iButtonHeight * 4));
		sGameName = GUILayout.TextField (sGameName, 30);
		sGamePassword = GUILayout.TextField (sGamePassword, 15);

		if (!Network.isClient && !Network.isServer) {
			if (GUILayout.Button (startServerButton)) {
							StartServer ();
							gameLobbyMenu.SetActive(true);
							this.gameObject.SetActive(false);
					}
			}

		GUILayout.EndArea();

		GUILayout.BeginArea(new Rect (Screen.width/2 - iButtonWidth/2, Screen.height * 0.7f - iButtonHeight/2, iButtonWidth, iButtonHeight));
		if (GUILayout.Button (returnButton)) {
			startMenu.SetActive(true);
			this.gameObject.SetActive(false);
		}
		GUILayout.EndArea();
	}

	void StartServer()
	{
		Network.incomingPassword = sGamePassword;
		Network.InitializeServer(1, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(sTypeName, sGameName);
	}
}


