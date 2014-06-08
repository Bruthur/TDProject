using UnityEngine;
using System.Collections;

public class sShortCut : MonoBehaviour {

	public GameObject startWayPoint;
	public GameObject baseWayPoint;
	public GameObject shortCutWayPoint;

	private bool shortCutOpened = false;

	[RPC] public void swapOpenStat(){
		shortCutOpened = !shortCutOpened;
		if(shortCutOpened){
			startWayPoint.GetComponent<sWayPoint>().nextWayPoint = shortCutWayPoint;
		}
		else{
			startWayPoint.GetComponent<sWayPoint>().nextWayPoint = baseWayPoint;
		}
	}
}
