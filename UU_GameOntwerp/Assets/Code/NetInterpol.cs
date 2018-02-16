using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetInterpol : MonoBehaviour {

    private Vector3 targetPos, checkPos;

	void Start () {
        targetPos = transform.position;
    }
	
	void Update () {
		if(transform.position != checkPos)
        {
            targetPos = transform.position;
            transform.position = checkPos;
        }
        transform.position = Vector3.Lerp(checkPos, targetPos, 0.5f);
        checkPos = transform.position;
	}
}