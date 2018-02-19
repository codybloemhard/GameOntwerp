using UnityEngine.Networking;
using UnityEngine;

public class Shooting : NetworkBehaviour {
    
	[SerializeField]
    private GameObject closestSpawnPoint;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float snelheid;
	
	void Start () { }
	
	void Update () {
		if (Input.GetMouseButtonDown(0) && isLocalPlayer)
            CmdFire(closestSpawnPoint.transform.position);
	}
	
	[Command]
	void CmdFire(Vector3 spawnPos)
    {
        if (bullet == null) return;
        Quaternion rotation = Quaternion.LookRotation(spawnPos);
        GameObject shot = (GameObject)Instantiate(bullet, spawnPos, closestSpawnPoint.transform.rotation);
        shot.GetComponent<Rigidbody>().velocity = shot.transform.forward * snelheid;
        NetworkServer.Spawn(shot);
    }
}