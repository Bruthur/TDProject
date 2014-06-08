using UnityEngine;
using System.Collections;

public class sCreep : MonoBehaviour {
	public GameObject nextWayPoint;
	private float distanceToChangeWayPoint = 2f;

	public float baseMovementSpeed = 10f;

	private float movementSpeed;


	// Use this for initialization
	void Awake () {
		movementSpeed = baseMovementSpeed;
//		nextWayPoint = GameObject.Find ("CreepSpawn");
	}
	
	// Update is called once per frame
	void Update () {
		move();
	}

	void move(){
		if(Vector3.Distance(nextWayPoint.transform.position, transform.position) < distanceToChangeWayPoint){
			if(nextWayPoint.GetComponent<sWayPoint>().nextWayPoint != null){
				nextWayPoint = nextWayPoint.GetComponent<sWayPoint>().nextWayPoint;
			}
			else{
				//Creep reach end of road
				Destroy(this.gameObject);
			}
		}
		rigidbody.MovePosition(rigidbody.position + Vector3.Normalize(nextWayPoint.transform.position - transform.position) * movementSpeed * Time.deltaTime);
	}
}
