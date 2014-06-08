using UnityEngine;
using System.Collections;

public class sGameLobbyMenu : MonoBehaviour {

	//This contain each player's information for this menu
	private struct Player{
		public NetworkPlayer playerID;
		public string sPlayerName;
		public bool bIsRTS;
		public bool bIsReady;
	}
	//There is 2 players
	private Player[] players;
	//gameobjects needed
	public sPlayerSettings playerSettings;
	public GameObject hostMenu;
	public GameObject joinMenu;
	public sGUIData GUIData;

	//GUI Data
	int iButtonWidth;
	int iButtonHeight;
	string swapRolesButton;
	string readyButton;
	string launchButton;
	string exitButton;

	//variables for the lobby's chat
	private string sChat;
	private string sNextLine = "";
	private Vector2 v2ScrollPosition;
	private bool bEnterPressed = false;


	void Awake() {
		//On awake, load GUI data from GUIData
		iButtonWidth = GUIData.lobbyMenuButtonWidth;
		iButtonHeight = GUIData.lobbyMenuButtonHeight;
		swapRolesButton = GUIData.lobbyMenuSwapRolesButton;
		readyButton = GUIData.lobbyMenuReadyButton;
		launchButton = GUIData.lobbyMenuLaunchButton;
		exitButton = GUIData.lobbyMenuExitButton;
		//Some welcome message
		sChat = "Welcome in the lobby of your game" + System.Environment.NewLine + "You can chat here.";
		//Create the player tabs and initialize it
		players = new Player[2];
		initializePlayers ();
	}

	void OnEnable()
	{
		//Some welcome message
		sNextLine = "";
		sChat = "Welcome in the lobby of your game" + System.Environment.NewLine + "You can chat here.";
	}

	//Set default values for known variables
	[RPC] void initializePlayers (){
		if(Network.isServer)
		{
			networkView.RPC("SetMyPlayerID", RPCMode.All);
			players[0].sPlayerName = playerSettings.GetPlayerName();
			players[0].bIsRTS = true;
			players[0].bIsReady = false;
			players[1].bIsRTS = false;
			players[1].bIsReady = false;
		}
		else{
			players[0].bIsRTS = true;
			players[0].bIsReady = false;
			players[1].sPlayerName = playerSettings.GetPlayerName();
			players[1].bIsRTS = false;
			players[1].bIsReady = false;
		}
	}

	void OnGUI(){
		//This area is reserved for the map selection
		GUILayout.BeginArea(new Rect(0, 0,Screen.width * 0.7f, Screen.height * 0.5f));
		GUILayout.Box("Map selection");
		GUILayout.EndArea();

		//This area is reserved for game's settings selection
		GUILayout.BeginArea(new Rect(Screen.width * 0.7f, 0,Screen.width * 0.3f, Screen.height * 0.5f));
		GUILayout.Box("Game parameters");
		GUILayout.EndArea();

		//This area contain the lobby's chat
		ShowLobbyChat();


		//This area contains buttons to swap roles, be ready, launch the game or exit it
		GUILayout.BeginArea(new Rect(Screen.width * 0.7f, Screen.height * 0.5f,Screen.width * 0.3f, Screen.height * 0.5f));
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		GUILayout.Box(players[0].sPlayerName);
		GUILayout.Box(players[0].bIsRTS.ToString());
		GUILayout.Box(players[0].bIsReady.ToString());
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Box(players[1].sPlayerName);
		GUILayout.Box(players[1].bIsRTS.ToString());
		GUILayout.Box(players[1].bIsReady.ToString());
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if(Network.isServer)
		{
			if(GUILayout.Button(swapRolesButton)){
				networkView.RPC("SwapPlayerIsRTS", RPCMode.All);
			}
		}
		if(GUILayout.Button(readyButton)){
			networkView.RPC("SetPlayerIsReady", RPCMode.All);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if(Network.isServer && players[0].bIsReady && players[1].bIsReady)
		{
			if(GUILayout.Button(launchButton)){
				networkView.RPC("LoadScene", RPCMode.AllBuffered, "InGame");
			}
		}
		if(GUILayout.Button(exitButton))
		{
			PlayerDisconnect();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	//Chat function
	void ShowLobbyChat(){
		Event e = Event.current;
		if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "ChatInput") {       
			bEnterPressed = true;
		}

		GUILayout.BeginArea(new Rect(0, Screen.height * 0.5f,Screen.width * 0.7f, Screen.height * 0.5f));
		v2ScrollPosition = GUILayout.BeginScrollView (v2ScrollPosition, GUILayout.Width (Screen.width * 0.7f), GUILayout.Height (Screen.height * 0.5f - 40));
		GUILayout.Label(sChat);
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal();
		GUI.SetNextControlName("ChatInput");
		sNextLine = GUILayout.TextField(sNextLine, 50);
		if (GUILayout.Button ("Send")|| bEnterPressed) {
			bEnterPressed = false;
			if(sNextLine != ""){
				networkView.RPC("SendNextChatLine", RPCMode.All, playerSettings.GetPlayerName() + " :" + sNextLine);
				sNextLine = "";
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	//Things to do is a player disconnect
	void PlayerDisconnect(){
		if(Network.isServer)
		{
			Network.Disconnect();
			hostMenu.SetActive(true);
			this.gameObject.SetActive(false);
		}
		if(Network.isClient)
		{
			Network.Disconnect();
			joinMenu.SetActive(true);
			this.gameObject.SetActive(false);
		}
	}

//-------------------------------Server events------------------------------

	//When a player connect to the server, called on server side
	void OnPlayerConnected(NetworkPlayer player) {
		//Reset roles and readiness
		initializePlayers();
		players[1].playerID = Network.connections[0];
		networkView.RPC("SetPlayerName", RPCMode.Others, players[0].sPlayerName, 0);
	}

	//Called on server on player's disconnection
	void OnPlayerDisconnected() {
		SendNextChatLine(players[1].sPlayerName + " disconnected");
		players[1].sPlayerName = "";
		players[1].bIsReady = false;
	}

//-------------------------------Client events------------------------------

	//When a player connect, called on client side
	void OnConnectedToServer()
	{
		players[0].playerID = Network.connections[0];
		networkView.RPC("SetMyPlayerID", RPCMode.All);
		networkView.RPC("SetPlayerName", RPCMode.Others, players[1].sPlayerName, 1);
		//A message is send
		networkView.RPC("SendNextChatLine", RPCMode.All, players[1].sPlayerName + " has connected");
	}

	//Called on client on player's disconnection
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer) {
			hostMenu.SetActive(true);
			this.gameObject.SetActive(false);
		}
		else {
			joinMenu.GetComponent<sJoinMenu>().sConnectionErrorMessage = "Disconnected from server: " + info.ToString();
			joinMenu.SetActive(true);
			this.gameObject.SetActive(false);
		}
	}

	//Called on client side if the connection failed
	void OnFailedToConnect(NetworkConnectionError error) {
		joinMenu.GetComponent<sJoinMenu>().sConnectionErrorMessage = "Could not connect to server: " + error;
		joinMenu.SetActive(true);
		this.gameObject.SetActive(false);
	}

//-------------------------------RPC fonctions------------------------------

	//Add a line to the chat and put the scrollbar down
	[RPC] void SendNextChatLine(string line)
	{
		sChat += System.Environment.NewLine + line;
		v2ScrollPosition.y += Mathf.Infinity;
	}

	//Load InGame scene and save players ID
	[RPC] void LoadScene(string scene){
		if(Network.isServer){
			playerSettings.SetPlayerID(players[0].playerID);
			playerSettings.SetPlayerIsRTS(players[0].bIsRTS);
		}
		else{
			playerSettings.SetPlayerID(players[1].playerID);
			playerSettings.SetPlayerIsRTS(players[1].bIsRTS);
		}
		Application.LoadLevel(scene);
	}
	
	//Send player's name
	[RPC] void SetPlayerName(string name, int playerIndex, NetworkMessageInfo msg)
	{
		players[playerIndex].sPlayerName = name;
	}
	
	//Toggle player's ready
	[RPC] void SetPlayerIsReady(NetworkMessageInfo msg)
	{
		if(msg.sender == players[0].playerID){
			players[0].bIsReady = !players[0].bIsReady;
		}
		else if (msg.sender == players[1].playerID){
			players[1].bIsReady = !players[1].bIsReady;
		}
	}

	//Since msg.sender = -1 when received by the sender...
	[RPC] void SetMyPlayerID(NetworkMessageInfo msg)
	{
		//First case, the server is empty, so the sender is the server
		if(Network.connections.Length == 0){
			players[0].playerID = msg.sender;
		}
		//Second case, 2 players so the sender is not the server
		else if(msg.sender != players[0].playerID){
			players[1].playerID = msg.sender;
		}
	}
	
	//Switch roles between players
	[RPC] void SwapPlayerIsRTS(NetworkMessageInfo msg)
	{
		players[0].bIsRTS = !players[0].bIsRTS;
		players[1].bIsRTS = !players[1].bIsRTS;
	}
}
