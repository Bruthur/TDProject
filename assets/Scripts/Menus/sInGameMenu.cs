using UnityEngine;
using System.Collections;

public class sInGameMenu : MonoBehaviour {

	private sPlayerSettings playerSettings;	
	public sGUIData GUIData;
	//GUI variables
	int iInGameMenuButtonWidth;
	int inGameMenuButtonHeight;
	string pauseButton;
	string resumeButton;
	string swapRolesButton;
	string saveButton;
	string settingsButton;
	string disconnectButton;
	string exitGameButton;
	string swapRolesInfo;
	string swapRolesYesButton;
	string swapRolesNoButton;
	string menuPauseInfo;

	private bool bShowInGameMenu = false;
	private bool bGamePaused = false;
	private bool bSwapAsked = false;
	//Chat variables
	private bool bDrawChat = false;
	private bool bEnterPressed = false;
	private Vector2 v2ScrollPosition;
	private string sChat;
	private string sNextLine = "";
	private float fTimeSinceDrawChat = 0f;
	private bool bDrawNextLine = false;
	public float fDrawChatDuration = 5f;


	void Awake(){
		playerSettings = GameObject.Find ("PlayerSettings").GetComponent<sPlayerSettings>();
		GUIData = GameObject.Find ("GUIData").GetComponent<sGUIData>();
		//On awake, load data from GUIData
		iInGameMenuButtonWidth = GUIData.inGameMenuButtonWidth;
		inGameMenuButtonHeight = GUIData.inGameMenuButtonHeight;
		pauseButton = GUIData.inGameMenuPauseButton;
		resumeButton = GUIData.inGameMenuResumeButton;
		swapRolesButton = GUIData.inGameMenuSwapRolesButton;
		saveButton = GUIData.inGameMenuSaveButton;
		settingsButton = GUIData.inGameMenuSettingsButton;
		disconnectButton = GUIData.inGameMenuDisconnectButton;
		exitGameButton = GUIData.inGameMenuExitGameButton;
		swapRolesInfo = GUIData.inGameMenuSwapRolesButton;
		swapRolesYesButton = GUIData.inGameMenuSwapRolesYesButton;
		swapRolesNoButton = GUIData.inGameMenuSwapRolesNoButton;
		menuPauseInfo = GUIData.inGameMenuPauseInfo;
	}

	void Update (){
		//if escape is pressed, draw main menu
		if(Input.GetKeyDown(KeyCode.Escape)){
			bShowInGameMenu = !bShowInGameMenu;
		}
		//if enter is pressed draw chat
		if(Input.GetKeyDown(KeyCode.Return)){
			bDrawChat = true;
			bDrawNextLine = true;
			fTimeSinceDrawChat = Time.time;
		}
		//if more than fDrawChatDuration passed, hide chat
		if(bDrawChat){
			if(Time.time > fTimeSinceDrawChat + fDrawChatDuration && !bDrawNextLine){
				bDrawChat = false;
			}
		}
	}

	void OnGUI () {
		if(bShowInGameMenu){
			DrawMainMenu();
		}
		if(bSwapAsked){
			DrawSwapMenu();
		}
		if(bGamePaused){
			DrawPauseMenu();
		}
		if(bDrawChat){
			DrawChat();
		}
	}

	//Main menu is a column of buttons in mid screen
	void DrawMainMenu(){
		GUILayout.BeginArea(new Rect((Screen.width - iInGameMenuButtonWidth)/2, Screen.height/5, iInGameMenuButtonWidth, Screen.height * 2/3));
		if(!bShowInGameMenu){
			if(GUILayout.Button(pauseButton)) {
				networkView.RPC("PauseGame", RPCMode.All);
				bShowInGameMenu = false;
			}
		}
		else {
			if(GUILayout.Button(resumeButton)) {
				networkView.RPC("PauseGame", RPCMode.All);
				bShowInGameMenu = false;
			}
		}
		if(GUILayout.Button(swapRolesButton)) {
			networkView.RPC("AskSwap", RPCMode.Others);
			bShowInGameMenu = false;
		}
		if(GUILayout.Button(saveButton)) {
			
			bShowInGameMenu = false;
		}
		if(GUILayout.Button(settingsButton)) {
			
		}
		if(GUILayout.Button(disconnectButton)) {
			
		}
		if(GUILayout.Button(exitGameButton)) {
			Application.Quit();
		}
		GUILayout.EndArea();
	}

	//Swap menu is a 2 line menu asking if the player accept to swap roles
	void DrawSwapMenu(){
		GUILayout.BeginArea(new Rect(Screen.width/2 - iInGameMenuButtonWidth, Screen.height/6, iInGameMenuButtonWidth * 2, Screen.height/3));
		GUILayout.Box(swapRolesInfo);
		GUILayout.BeginHorizontal();
		if(GUILayout.Button(swapRolesYesButton))
		{
			bSwapAsked = false;
			networkView.RPC("SwapRoles", RPCMode.All);
		}
		if(GUILayout.Button(swapRolesNoButton))
		{
			bSwapAsked = false;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	//1 line menu telling that the game is paused
	void DrawPauseMenu (){
		GUILayout.BeginArea(new Rect(Screen.width/2 - iInGameMenuButtonWidth, Screen.height/7, iInGameMenuButtonWidth * 2, Screen.height/3));
		GUILayout.Box(menuPauseInfo);
		GUILayout.EndArea();
	}

	//--------------------------------------In game Chat functions------------------------------------------------------

	void DrawChat(){
		Event e = Event.current;
		if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "ChatInput") {       
			bEnterPressed = true;
		}

		if(sNextLine != ""){
			fTimeSinceDrawChat = Time.time;
		}
		
		GUILayout.BeginArea(new Rect(0, Screen.height * 0.5f,Screen.width * 0.5f, Screen.height * 0.3f));
		v2ScrollPosition = GUILayout.BeginScrollView (v2ScrollPosition, GUILayout.Width (Screen.width * 0.5f), GUILayout.Height (Screen.height * 0.3f - 40));
		GUILayout.Label(sChat);
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal();
		if(bDrawNextLine){
			GUI.SetNextControlName("ChatInput");
			sNextLine = GUILayout.TextField(sNextLine, 50);
			GUI.FocusControl("ChatInput");
		}
		if (bEnterPressed) {
			bEnterPressed = false;
			bDrawNextLine = false;
			if(sNextLine != ""){
				networkView.RPC("SendNextChatLine", RPCMode.All, playerSettings.GetPlayerName() + " :" + sNextLine);
				sNextLine = "";
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	//Add a line to the chat and put the scrollbar down
	[RPC] void SendNextChatLine(string line)
	{
		sChat += System.Environment.NewLine + line;
		v2ScrollPosition.y += Mathf.Infinity;
		bDrawChat = true;
		fTimeSinceDrawChat = Time.time;
	}

	//---------------------------------------------------RPC Functions--------------------------------------------------

	[RPC] void PauseGame() {
		bGamePaused = !bGamePaused;
		if(bGamePaused)	{
			Time.timeScale = 0;
		}
		else {
			Time.timeScale = 1;
		}
	}

	[RPC] void AskSwap() {
		bSwapAsked = true;
	}

	[RPC] void SwapRoles() {
		playerSettings.SwapRoles();
	}
}
