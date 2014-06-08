using UnityEngine;
using System.Collections;

public class sRTSPlayer : MonoBehaviour {
	
	private sPlayerSettings playerSettings;
	public GameObject RTSCamera;
	public GameObject RTSCursor;
	
	void Awake(){
		playerSettings = GameObject.Find ("PlayerSettings").GetComponent<sPlayerSettings>();
		playerSettings.SetRTSCamera(RTSCamera);
		playerSettings.SetRTSPlayer(gameObject);
		
		if(playerSettings.GetPlayerIsRTS()){
			RTSCamera.SetActive(true);
			RTSCamera.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SwapPlayer(bool rts){
		if(rts){

		}
	}
}
