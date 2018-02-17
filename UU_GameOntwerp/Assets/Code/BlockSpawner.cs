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
    [SerializeField]
    public Camera maincam;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float snelheid;
    void Start () {
		
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0) && isLocalPlayer)
            CmdSpawnCube();
        if (Input.GetMouseButtonDown(1) && isLocalPlayer)
            Fire();
        if (Input.GetMouseButton(2) && isLocalPlayer)
            Fire();
    }
    
    [Command]
    void CmdSpawnCube()
    {
        if (cubeObj != null)
        {

            
            Vector3 spawnPos = closestSpawnPoint.transform.position;
            GameObject spawned = (GameObject)Instantiate(cubeObj, spawnPos, Quaternion.identity);
                       
            NetworkServer.Spawn(spawned);
        }
    }
    void Fire()
    {
        if (cubeObj != null)
        {
            Vector3 spawnPos = closestSpawnPoint.transform.position;
         
            Quaternion rotation = Quaternion.LookRotation(spawnPos);

            GameObject shot = (GameObject)Instantiate(bullet, spawnPos, closestSpawnPoint.transform.rotation);

            shot.GetComponent<Rigidbody>().velocity = shot.transform.forward * snelheid;

            NetworkServer.Spawn(shot);
            
        }
    }
}