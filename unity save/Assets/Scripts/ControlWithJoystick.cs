using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlWithJoystick : MonoBehaviour {

    public int controllerNum;
    string horizontal;
    string vertical;
    string aButton;

    Vector3 position;
    public float speed = 0.1f;

	// Use this for initialization
	void Start () {
        position = transform.position;

        horizontal = "C" + controllerNum + "Horizontal";
        vertical = "C" + controllerNum + "Vertical";
	}
	
	// Update is called once per frame
	void Update () {
        position.x += Input.GetAxis(horizontal) * speed;
        position.y += Input.GetAxis(vertical) * speed;

        transform.position = position;
	}
}
