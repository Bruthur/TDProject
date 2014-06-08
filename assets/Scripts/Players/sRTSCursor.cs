using UnityEngine;
using System.Collections;

public class sRTSCursor : MonoBehaviour {

	public float iHeightOffSet = 2.6f;
	private Ray rMouseToGround;
	private RaycastHit rhMouseGroundPosition;
	private Vector3 vHeightOffSet;

	// Use this for initialization
	void Awake() {
		vHeightOffSet = new Vector3(0, iHeightOffSet, 0);
	}
	
	// Update is called once per frame
	void Update () {
		rMouseToGround = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(rMouseToGround, out rhMouseGroundPosition, 1000)) 
		{
			transform.position = rhMouseGroundPosition.point + vHeightOffSet;
		}
	}
}
