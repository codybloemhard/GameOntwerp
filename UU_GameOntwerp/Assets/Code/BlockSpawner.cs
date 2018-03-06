using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlockSpawner : NetworkBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject closestSpawnPoint;
    [SerializeField]
    private float grapDistance = 5f, pullPower = 0.5f;
    [SerializeField]
    GameObject[] blocks;
    private GameObject caught;

    void Start () { }
	
	void Update () {
        if (Input.GetMouseButton(0) && isLocalPlayer && caught == null)
        {
            RaycastHit hit;
            Physics.Raycast(closestSpawnPoint.transform.position, cam.transform.forward, out hit);
            if (hit.collider != null)
            {
                if (hit.distance < grapDistance && hit.collider.tag == "BuildingBlock"
                    || hit.collider.tag == "TargetA" || hit.collider.tag == "TargetB")
                {
                    caught = hit.collider.gameObject;
                }
            }
        }
        else if(caught != null && !Input.GetMouseButton(0))
            caught = null;
        if (caught != null)
            CmdPullObject(caught);
        
        if(isLocalPlayer && Center.instance.toBeSpawned != -1)
        {
            CmdSpawnCube(Center.instance.toBeSpawned);
            Center.instance.toBeSpawned = -1;
        }
	}

    public void End()
    {
        if (caught == null) return;
    }

    [Command]
    private void CmdSpawnCube(int o)
    {
        Vector3 spawnPos = closestSpawnPoint.transform.position;
        GameObject spawned = (GameObject)Instantiate(blocks[o], spawnPos, Quaternion.identity);
        NetworkServer.Spawn(spawned);
    }
    [Command]
    private void CmdPullObject(GameObject go)
    {
        Vector3 gotopos = Vector3.Lerp(go.transform.position, closestSpawnPoint.transform.position, pullPower);
        go.GetComponent<Dragable>().Set(gotopos);
    }
}