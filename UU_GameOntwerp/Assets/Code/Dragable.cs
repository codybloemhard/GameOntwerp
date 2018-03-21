using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Dragable : NetworkBehaviour
{
    [SyncVar]
    private Vector3 pos;
    [SyncVar]
    private float dragged;
    [SyncVar]
    private string name;
    private Rigidbody body;
    private Vector3 _pos;
    private Quaternion _rot;
    private int fromPlayer;

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
        if (gameObject.name != name && gameObject.name[0] != 'T')
            gameObject.name = name;
    }

	public void Set(Vector3 pos)
    {
        dragged = 1f;
        this.pos = pos;
        body.useGravity = false;
        transform.position = pos;
    }

    public void SaveState()
    {
        _pos = transform.position;
        _rot = transform.rotation;
    }

    public void ResetState()
    {
        transform.position = _pos;
        transform.rotation = _rot;
    }

    public void SetPlayer(int nr)
    {
        fromPlayer = nr;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public Vector2 GetDamage()
    {
        Vector2 result = new Vector2();
        result.x = body.mass * (transform.position - _pos).magnitude;
        result.y = fromPlayer;
        return result;
    }
}