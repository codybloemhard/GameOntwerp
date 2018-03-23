﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDespawn : MonoBehaviour {

    [SerializeField]
    private float lifeTime = 1f;

	private void Start () {
        Destroy(gameObject, lifeTime);
	}
}