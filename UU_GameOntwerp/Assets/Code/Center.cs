using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Phase
{
    NONE,
    BUILDING,
    PLAYING
}

public class Center : MonoBehaviour {

    public static Center instance;
    [SerializeField]
    public Phase phase;

    private void Awake () {
        if (instance != null)
            Destroy(this);
        else instance = this;
        phase = Phase.BUILDING;
	}
	
	private void Update () {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (phase == Phase.BUILDING) phase = Phase.PLAYING;
            else if (phase == Phase.PLAYING) phase = Phase.BUILDING;
        }
	}
}