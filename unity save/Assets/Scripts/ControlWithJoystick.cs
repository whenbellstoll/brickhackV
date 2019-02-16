using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlWithJoystick : MonoBehaviour {

    Vector3 position;
    public float speed = 0.1f;

	// Use this for initialization
	void Start () {
        position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        position.x += Input.GetAxis("Horizontal") * speed;
        position.y += Input.GetAxis("Vertical") * speed;

        transform.position = position;
	}
}
