using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlockSpawner : NetworkBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private GameObject cubeObj;
    [SerializeField]
    private GameObject closestSpawnPoint;
    [SerializeField]
    private float grapDistance = 5f, pullPower = 0.5f;
    private GameObject caught;

    void Start () { }
	
	void Update () {
        if (Input.GetMouseButton(0) && isLocalPlayer && caught == null)
        {
            RaycastHit hit;
            Physics.Raycast(closestSpawnPoint.transform.position, cam.transform.forward, out hit);
            if (hit.collider != null)
            {
                if (hit.distance < grapDistance && hit.collider.tag == "BuildingBlock")
                {
                    caught = hit.collider.gameObject;
                }
            }
        }
        else if(caught != null && !Input.GetMouseButton(0))
            caught = null;
        if (caught != null)
            CmdPullObject(caught);
        if (Input.GetMouseButtonDown(1) && isLocalPlayer && caught == null)
            CmdSpawnCube();
	}

    public void End()
    {
        if (caught == null) return;
    }

    [Command]
    private void CmdSpawnCube()
    {
        if (cubeObj == null) return;
        Vector3 spawnPos = closestSpawnPoint.transform.position;
        GameObject spawned = (GameObject)Instantiate(cubeObj, spawnPos, Quaternion.identity);
        NetworkServer.Spawn(spawned);
    }
    [Command]
    private void CmdPullObject(GameObject go)
    {
        Vector3 gotopos = Vector3.Lerp(go.transform.position, closestSpawnPoint.transform.position, pullPower);
        go.GetComponent<Dragable>().Set(gotopos);
    }
}