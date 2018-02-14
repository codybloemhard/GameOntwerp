using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [SerializeField]
    private Behaviour[] disableThese;
    private Camera lobbyCam;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            for(int i = 0; i < disableThese.Length; i++)
            {
                disableThese[i].enabled = false;
            }
        }
        else
        {
            lobbyCam = Camera.main;
            if(lobbyCam != null)
                lobbyCam.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (lobbyCam != null)
            lobbyCam.gameObject.SetActive(true);
    }
}