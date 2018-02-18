using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour {

    public CustomMouseLook mouseLook = new CustomMouseLook();
    public Camera cam;
    public float speed = 4f;

    private void Start () {
        mouseLook.Init(transform, cam.transform);
    }

    private void Update () {
        mouseLook.LookRotation(gameObject.transform, cam.transform);
        if (Input.GetKey("w"))
            transform.position += transform.forward * speed * Time.deltaTime;
        else if(Input.GetKey("s"))
            transform.position += transform.forward * -speed * Time.deltaTime;
        else if (Input.GetKey("a"))
            transform.position += transform.right * -speed * Time.deltaTime;
        else if (Input.GetKey("d"))
            transform.position += transform.right * speed * Time.deltaTime;
        if (Input.GetKey("q"))
            transform.position += transform.up * speed * Time.deltaTime;
        else if (Input.GetKey("e"))
            transform.position += transform.up * -speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        mouseLook.UpdateCursorLock();
    }
}