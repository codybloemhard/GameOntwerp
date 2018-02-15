using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject cubeObj;
    [SerializeField]
    private GameObject closestSpawnPoint;


    void Start () {
		
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if (cubeObj != null)
            {
                Vector3 spawnPos = closestSpawnPoint.transform.position;
                GameObject spawned = (GameObject)Instantiate(cubeObj, spawnPos, Quaternion.identity);
            }
        }
	}
}