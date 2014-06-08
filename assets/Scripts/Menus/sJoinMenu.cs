using UnityEngine;
using System.Collections;

public class sJoinMenu : MonoBehaviour {
	public sGUIData GUIData;
	public sPlayerSettings playerSettings;
	public GameObject startMenu;
	public GameObject gameLobbyMenu;
	private HostData[] hostList;

	//GUI variables
	int iButtonWidth;
	int iButtonHeight;
	string refreshHostButton;
	string passwordInfo;
	string returnButton;
	
	private Vector2 v2ScrollPosition;
	const string sTypeName = "UltimateTD";
	private string sGamePassword = "";
	public string sConnectionErrorMessage = "";

	void Awake(){
		//On awake, load data from GUIData
		iButtonWidth = GUIData.joinMenuButtonWidth;
		iButtonHeight = GUIData.joinMenuButtonHeight;
		refreshHostButton = GUIData.joinMenuRefreshHostButton;
		passwordInfo = GUIData.joinMenuPasswordInfo;
		returnButton = GUIData.joinMenuReturnButton;
	}

	//Ask master server the host list
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	void OnGUI()
	{
		//Draw a refresh button
		GUILayout.BeginArea(new Rect (Screen.width/4 - iButtonWidth/2, Screen.height/2, iButtonWidth * 1.5f, Screen.height/2));
		if (GUILayout.Button(refreshHostButton)){
			RefreshHostList();
		}
		GUILayout.BeginHorizontal();
		GUILayout.Label(passwordInfo);
		sGamePassword = GUILayout.TextField(sGamePassword, 15);
		GUILayout.EndHorizontal();
		GUILayout.Label(sConnectionErrorMessage);
		if (GUILayout.Button (returnButton)) {
			startMenu.SetActive(true);
			this.gameObject.SetActive(false);
		}
		GUILayout.EndArea();

		//Draw a button for each host found
		if (hostList != null)
		{
			GUILayout.BeginArea(new Rect (Screen.width * 0.75f - iButtonWidth/2, Screen.height * 0.25f, iButtonWidth, Screen.height/2));
			v2ScrollPosition = GUILayout.BeginScrollView (v2ScrollPosition);
			for (int i = 0; i < hostList.Length; i++)
			{
				if (GUILayout.Button(hostList[i].gameName)){
					JoinServer(hostList[i]);
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
		}
	}

	public void JoinServer(HostData hostData)
	{
		Network.Connect(hostData, sGamePassword);
		gameLobbyMenu.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void RefreshHostList()
	{
		MasterServer.ClearHostList();
		MasterServer.RequestHostList(sTypeName);
	}
}
