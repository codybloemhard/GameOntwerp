using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnable : MonoBehaviour {

    [SerializeField]
    private GameObject lanMenu, gameUI, buildInventoryUI, upgradeInventory;

    private void Start () { }
    
    private void Update () {
        bool isPlaying = CustomLanControls.instance.ConnectionExists();
        if (!isPlaying)
        {
            lanMenu.active = true;
            gameUI.active = false;
            buildInventoryUI.active = false;
            upgradeInventory.active = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            lanMenu.active = false;
            gameUI.active = true;
            Phase phase = Center.instance.GetPhase();
            Cursor.lockState = CursorLockMode.Locked;

            if (phase == Phase.BUILDING)
            {
                buildInventoryUI.active = true;
                upgradeInventory.active = false;
                
            }
            else if(phase == Phase.UPGRADE)
            {
                buildInventoryUI.active = false;
                upgradeInventory.active = true;
            }
            else
            {
                buildInventoryUI.active = false;
                upgradeInventory.active = false;      
            }
        }
    }
}