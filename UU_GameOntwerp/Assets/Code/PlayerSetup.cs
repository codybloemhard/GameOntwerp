using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] localOff_bhv, externOff_bhv;
    [SerializeField]
    private GameObject[] localOff_obj, externOff_obj;
    private Camera lobbyCam;
    private Phase lastPhase;
    //components on this object
    private Rigidbody body;
    private CapsuleCollider collider;
    [SerializeField]
    private Behaviour physicsControls, flyControls, blockSpawner;

    private void Start()
    {
        lastPhase = Phase.NONE;
        body = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        InitNet();
        
        if (isLocalPlayer)
        {
            lobbyCam = Camera.main;
            if (lobbyCam != null)
                lobbyCam.gameObject.SetActive(false);
        }
    }
    //enable and disable the correct behaviours and objects based on if were the local player
    private void InitNet()
    {
        if (isLocalPlayer)
        {
            for (int i = 0; i < localOff_bhv.Length; i++)
                localOff_bhv[i].enabled = false;
            for (int i = 0; i < localOff_obj.Length; i++)
                localOff_obj[i].SetActive(false);
        }
        else
        {
            for (int i = 0; i < externOff_bhv.Length; i++)
                externOff_bhv[i].enabled = false;
            for (int i = 0; i < externOff_obj.Length; i++)
                externOff_obj[i].SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (lobbyCam != null)
            lobbyCam.gameObject.SetActive(true);
    }

    private void Update()
    {
        Phase currentPhase = Center.instance.phase;
        if (currentPhase != lastPhase)
        {
            lastPhase = currentPhase;
            if (currentPhase == Phase.BUILDING)
            {
                body.useGravity = false;
                collider.enabled = false;
                physicsControls.enabled = false;
                flyControls.enabled = true;
                blockSpawner.enabled = true;
            }
            else if (currentPhase == Phase.PLAYING)
            {
                body.useGravity = true;
                collider.enabled = true;
                physicsControls.enabled = true;
                flyControls.enabled = false;
                blockSpawner.enabled = false;
            }
            InitNet();
        }
        if(currentPhase == Phase.BUILDING)
        {
            body.velocity = Vector3.zero;
            if (transform.position.y < 0f)
                transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
    }
}