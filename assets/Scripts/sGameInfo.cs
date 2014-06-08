using UnityEngine;
using System.Collections;

public class sGameInfo : MonoBehaviour {
	//prefab to instatiate players
	public GameObject goRTSPlayerPrefab;
	public GameObject goFPSPlayerPrefab;
	public GameObject goFPSSpawn;
	public GameObject goRTSSpawn;

	private sPlayerSettings playerSettings;

	void Awake(){
		playerSettings = GameObject.Find ("PlayerSettings").GetComponent<sPlayerSettings>();
	}
	
	void Start () {
		if(playerSettings.GetPlayerIsRTS()){
			Network.Instantiate(goRTSPlayerPrefab, goRTSSpawn.transform.position, Quaternion.identity, 0);
		}
		else{
			Network.Instantiate(goFPSPlayerPrefab, goFPSSpawn.transform.position, Quaternion.identity, 0);
		}
	}

	//-------------------------------Server events------------------------------
	
	//Called on server on player's disconnection
	void OnPlayerDisconnected() {
		Network.Disconnect();
		GameObject.Destroy(playerSettings.gameObject);
		Application.LoadLevel("StartMenu");
	}
	
	//-------------------------------Client events------------------------------
	
	//Called on client on player's disconnection
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer) {
			GameObject.Destroy(playerSettings.gameObject);
			Application.LoadLevel("StartMenu");
		}
		else {
			GameObject.Destroy(playerSettings.gameObject);
			Application.LoadLevel("StartMenu");
		}
	}
}
