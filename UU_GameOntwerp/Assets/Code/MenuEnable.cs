using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnable : MonoBehaviour {

    [SerializeField]
    private GameObject lanMenu, gameUI;

    private void Start () {
		
	}

    private void Update () {
        bool isPlaying = CustomLanControls.instance.ConnectionExists();
        if (!isPlaying)
        {
            lanMenu.active = true;
            gameUI.active = false;
        }
        else
        {
            lanMenu.active = false;
            gameUI.active = true;
        }
    }
}