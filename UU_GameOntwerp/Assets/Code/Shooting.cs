using UnityEngine.Networking;
using UnityEngine;

public class Shooting : NetworkBehaviour {
    
	[SerializeField]
    private GameObject closestSpawnPoint;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float snelheid;
    [SerializeField]
    private float waitTime;
    private int playerNr;
    private float time;

    void Start () { }
	
	void Update () {
        if (!isLocalPlayer) return;
        time += Time.deltaTime;
        Center.instance.shootPercentage = time / waitTime;
        if (Input.GetMouseButtonDown(0) && time >= waitTime)
        {
            time = 0f;
            CmdFire(closestSpawnPoint.transform.position, playerNr);
        }
	}
	
	[Command]
	void CmdFire(Vector3 spawnPos, int nr)
    {
        if (bullet == null) return;
        Quaternion rotation = Quaternion.LookRotation(spawnPos);
        GameObject shot = (GameObject)Instantiate(bullet, spawnPos, closestSpawnPoint.transform.rotation);
        shot.GetComponent<Rigidbody>().velocity = shot.transform.forward * snelheid;
        shot.name = "bullet" + nr;
        NetworkServer.Spawn(shot);
    }

    public void SetNr(int nr)
    {
        playerNr = nr;
    }
}