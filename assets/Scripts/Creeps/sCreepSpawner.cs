using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sCreepSpawner : MonoBehaviour {

	public GameObject baseCreep;
	public float spawnFrequency = 3f;
	private float lastSpawnHour = 0f;

	public List<GameObject> creepList;

	// Update is called once per frame
	void Update () {
		if(Network.isServer){
			if(Time.time >= lastSpawnHour + spawnFrequency){
				networkView.RPC("SpawnCreep", RPCMode.All);
				lastSpawnHour = Time.time;
			}
		}
	}

	[RPC] void SpawnCreep(){
		creepList.Add((GameObject) Instantiate(baseCreep, transform.position, transform.rotation));
	}
}
