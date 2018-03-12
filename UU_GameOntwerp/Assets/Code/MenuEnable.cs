using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnable : MonoBehaviour {

    [SerializeField]
    private GameObject lanMenu, gameUI, inventoryUI;

    private void Start () { }
    
    private void Update () {
        bool isPlaying = CustomLanControls.instance.ConnectionExists();
        if (!isPlaying)
        {
            lanMenu.active = true;
            gameUI.active = false;
            inventoryUI.active = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            lanMenu.active = false;
            gameUI.active = true;
            bool buildPhase = Center.instance.GetPhase() == Phase.BUILDING;

            if(buildPhase)
            {
                inventoryUI.active = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                inventoryUI.active = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}