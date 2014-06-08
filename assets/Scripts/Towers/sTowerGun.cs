using UnityEngine;
using System.Collections;

public class sTowerGun : MonoBehaviour {

	private sCreepSpawner creepSpawn;
	private GameObject towerTarget;
	private Quaternion lookTmpRotation;
	private float lastShotHour = 0;
	private GameObject lastEffect;

	public GameObject towerEffect;
	public Vector3 effectOffSet = Vector3.zero;
	public float towerRange = 50f;
	public float towerFireRate = 0.5f;

	void Awake () {
		creepSpawn = GameObject.Find("CreepSpawn").GetComponent<sCreepSpawner>();
	}

	// Update is called once per frame
	void Update () {
		if(towerTarget == null || Vector3.Distance(towerTarget.transform.position, transform.position) > towerRange){
			towerTarget = null;
			FindTarget();
		}
		else{
			LookAtTarget();
			if(Time.time >= lastShotHour + towerFireRate && towerEffect != null) {
				FireEffect();
			}
		}
	}

	void FindTarget(){
		foreach(GameObject creep in creepSpawn.creepList){
			if(Vector3.Distance(transform.position, creep.transform.position) <= towerRange ){
				towerTarget = creep;
			}
		}
	}

	void LookAtTarget(){
		lookTmpRotation = Quaternion.LookRotation(towerTarget.transform.position - transform.position);
		lookTmpRotation.x = 0;
		lookTmpRotation.z = 0;
		transform.rotation = lookTmpRotation;
	}

	void FireEffect(){
		lastEffect = (GameObject)Instantiate(towerEffect, transform.position , transform.rotation);
		lastEffect.transform.parent = transform;
		lastEffect.transform.position += effectOffSet;
	}
}
