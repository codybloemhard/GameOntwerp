using UnityEngine.Networking;
using UnityEngine;

public class Shooting : NetworkBehaviour {

	[SerializeField]
    public Camera maincam;
	[SerializeField]
    private GameObject closestSpawnPoint;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float snelheid;
	
	void Start () { }
	
	void Update () {
		if (Input.GetMouseButtonDown(0) && isLocalPlayer)
            CmdFire();
	}
	
	[Command]
	void CmdFire()
    {
        if (bullet == null || maincam == null) return;
        Vector3 spawnPos = closestSpawnPoint.transform.position;
        Quaternion rotation = Quaternion.LookRotation(spawnPos);
        GameObject shot = (GameObject)Instantiate(bullet, spawnPos, closestSpawnPoint.transform.rotation);
        shot.GetComponent<Rigidbody>().velocity = shot.transform.forward * snelheid;
        NetworkServer.Spawn(shot);
    }
}