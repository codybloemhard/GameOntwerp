using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnable : MonoBehaviour {

    [SerializeField]
    private GameObject lanMenu, gameUI, inventoryUI;
    private bool invOpen = false;

    private void Start () {
		
	}
    
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
            bool buildPhase = Center.instance.GetPhase() == Phase.BUILDING;
            if (buildPhase)
            {
                if (Input.GetKeyDown("f"))
                    invOpen = !invOpen;
            }
            else invOpen = false;

            if(invOpen && buildPhase)
            {
                gameUI.active = false;
                inventoryUI.active = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Center.instance.inventoryOpen = true;
            }
            else
            {
                gameUI.active = true;
                inventoryUI.active = false;
                Cursor.lockState = CursorLockMode.Locked;
                Center.instance.inventoryOpen = false;
            }
        }
    }
}