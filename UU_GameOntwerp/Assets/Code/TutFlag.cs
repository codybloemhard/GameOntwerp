using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutFlag : MonoBehaviour {

    public static TutFlag instance;
    public bool needHelp;

	private void Awake () {
        if (instance != null)
            Destroy(this);
        else instance = this;
        needHelp = false;
    }
}