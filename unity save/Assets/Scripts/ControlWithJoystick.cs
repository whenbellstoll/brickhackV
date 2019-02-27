using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlWithJoystick : MonoBehaviour {

    public int controllerNum;
    string horizontal;
    string vertical;

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
        //if anything extrnal to this function has updated the position
        //"clean" our variable
        position = transform.position;
        //Note the abscence of this is why our objects appreaded to be getting "stuck"

        position.x += Input.GetAxis(horizontal) * speed;
        position.y += Input.GetAxis(vertical) * speed;
        //keyboard control for player 1
        if(horizontal == "C1Horizontal")
        {
            if (Input.GetKey(KeyCode.A))
            {
                position.x -= speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                position.x += speed;
            }

            if (Input.GetKey(KeyCode.W))
            {
                position.y += speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                position.y -= speed;
            }
        }
        //keyboard control for player 2
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                position.x -= speed;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                position.x += speed;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                position.y += speed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                position.y -= speed;
            }
        }

        transform.position = position;
	}
}
