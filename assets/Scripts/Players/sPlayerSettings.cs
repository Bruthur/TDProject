using UnityEngine;
using System.Collections;

public class sPlayerSettings : MonoBehaviour {
	//playerSettings contain the player personnal informations

	public string sPlayerName;
	public bool bIsRTS;
	private NetworkPlayer playerID;
	private GameObject goFPSCamera;
	private GameObject goRTSCamera;
	private GameObject goFPSPlayer;
	private GameObject goRTSPlayer;


	//The playersettings must survive until the game's end
	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}

	public void SetPlayerName(string name){
		sPlayerName = name;
	}

	public string GetPlayerName(){
		return sPlayerName;
	}

	public void SetPlayerIsRTS(bool b){
		bIsRTS = b;
	}
	
	public bool GetPlayerIsRTS(){
		return bIsRTS;
	}

	public void SetPlayerID(NetworkPlayer n){
		playerID = n;
	}
	
	public NetworkPlayer GetPlayerID(){
		return playerID;
	}

	public void SetRTSCamera(GameObject camera){
		goRTSCamera = camera;
	}

	public void SetFPSCamera(GameObject camera){
		goFPSCamera = camera;
	}

	public void SetRTSPlayer(GameObject player){
		goRTSPlayer = player;
	}
	
	public void SetFPSPlayer(GameObject player){
		goFPSPlayer = player;
	}

	public void SwapRoles(){
		bIsRTS = !bIsRTS;
		if(bIsRTS){
			goFPSCamera.SetActive(false);
			goRTSCamera.SetActive(true);
		}
		else{
			NetworkViewID newID = Network.AllocateViewID();
			networkView.RPC("SetFPSPlayerViewID", RPCMode.All, newID);
			goRTSCamera.SetActive(false);
			goFPSCamera.SetActive(true);
		}
	}

	[RPC]	void SetFPSPlayerViewID(NetworkViewID newID) {
		goFPSPlayer.networkView.viewID = newID;
	}
}
