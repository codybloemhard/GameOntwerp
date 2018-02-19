﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Dragable : NetworkBehaviour
{
    [SyncVar]
    private Vector3 pos;
    [SyncVar]
    private float dragged;
    private Rigidbody body;

    public void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        dragged -= 0.2f;
        if (dragged > 0f)
        {
            body.useGravity = false;
            body.velocity = Vector3.zero;
        }
        else
            body.useGravity = true;
    }

	public void Set(Vector3 pos)
    {
        dragged = 1f;
        this.pos = pos;
        body.useGravity = false;
        transform.position = pos;
    }
}