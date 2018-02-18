using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlockSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject cubeObj;
    [SerializeField]
    private GameObject closestSpawnPoint;

    void Start () { }
	
	void Update () {
        if (Input.GetMouseButtonDown(0) && isLocalPlayer)
            CmdSpawnCube();
	}
    
    [Command]
    void CmdSpawnCube()
    {
        if (cubeObj == null) return;
        Vector3 spawnPos = closestSpawnPoint.transform.position;
        GameObject spawned = (GameObject)Instantiate(cubeObj, spawnPos, Quaternion.identity);
        NetworkServer.Spawn(spawned);
    }
}