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
    private int playerNr;

    void Start () { }
	
	void Update () {
        if (Input.GetMouseButton(0) && isLocalPlayer && caught == null)
        {
            RaycastHit hit;
            Physics.Raycast(closestSpawnPoint.transform.position, cam.transform.forward, out hit);
            if (hit.collider != null)
            {
                int lastCharVal = hit.collider.name[hit.collider.name.Length - 1] - 48;
                int round = Center.instance.buildingRound;
                bool ok = round == lastCharVal;
                if (hit.distance < grapDistance && (hit.collider.tag == "BuildingBlock"
                    || hit.collider.tag == "TargetA" || hit.collider.tag == "TargetB") && ok)
                    caught = hit.collider.gameObject;
            }
        }
        else if(caught != null && !Input.GetMouseButton(0))
            caught = null;
        if (caught != null)
            CmdPullObject(caught);
        
        if(isLocalPlayer && Center.instance.toBeSpawned != -1)
        {
            CmdSpawnCube(Center.instance.toBeSpawned, playerNr);
            Center.instance.toBeSpawned = -1;
        }
	}

    public void End()
    {
        if (caught == null) return;
    }

    public void SetNr(int nr)
    {
        playerNr = nr;
    }
    
    [Command]
    private void CmdSpawnCube(int o, int nr)
    {
        if (o < 0 || o > blocks.Length - 1) return;
        Vector3 spawnPos = closestSpawnPoint.transform.position;
        GameObject spawned = (GameObject)Instantiate(blocks[o], spawnPos, Quaternion.identity);
        spawned.GetComponent<Dragable>().SetPlayer(nr);
        string name = "block" + nr + "-" + Center.instance.buildingRound;
        spawned.name = name;
        spawned.GetComponent<Dragable>().SetName(name);
        NetworkServer.Spawn(spawned);
    }
    [Command]
    private void CmdPullObject(GameObject go)
    {
        Vector3 gotopos = Vector3.Lerp(go.transform.position, closestSpawnPoint.transform.position, pullPower);
        go.GetComponent<Dragable>().Set(gotopos);
    }
}