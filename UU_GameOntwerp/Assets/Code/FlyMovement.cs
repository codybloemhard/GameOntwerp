using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour {

    public CustomMouseLook mouseLook = new CustomMouseLook();
    public Camera cam;
    public float speed = 4f;
    public float minimumHeight = 1f;
    private Vector3 lastPos;
    private Collider[] playingFields;

    private void Start () {
        mouseLook.Init(transform, cam.transform);
        GameObject[] temp = GameObject.FindGameObjectsWithTag("PlayingField");
        playingFields = new Collider[temp.Length];
        for (int i = 0; i < playingFields.Length; i++)
            playingFields[i] = temp[i].GetComponent<Collider>();
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

        if (transform.position.y < minimumHeight)
            transform.position = new Vector3(transform.position.x, minimumHeight, transform.position.z);
        
        bool setBack = true;
        for(int i = 0; i < playingFields.Length; i++)
            if(playingFields[i].bounds.Contains(transform.position))
            {
                setBack = false;
                break;
            }
        if (!setBack) lastPos = transform.position;
        else if(lastPos != Vector3.zero) transform.position = lastPos;
    }

    private void FixedUpdate()
    {
        mouseLook.UpdateCursorLock();
    }
}