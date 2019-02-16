using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    //used to detect collision between cursor and platform
    GameObject cursor;

    public float speed = 0.1f;

    Vector3 position;

	// Use this for initialization
	void Start () {
        position = transform.position;
        gameObject.GetComponent<ControlWithJoystick>().enabled = false;

        cursor = GameObject.Find("Cursor");
        Debug.Log(cursor.GetComponent<BoxCollider2D>().bounds);
	}
	
	// Update is called once per frame
	void Update () {

        TestForClicked();

        transform.position = position;
	}

    bool TestForClicked()
    {
        Debug.Log("we got here");
        //tests collision between platform and cursor
        if (gameObject.GetComponent<BoxCollider2D>().bounds.Intersects(cursor.GetComponent<BoxCollider2D>().bounds))
        {
            if (Input.GetKeyDown("joystick button 0"))
            {
                gameObject.GetComponent<ControlWithJoystick>().enabled = !gameObject.GetComponent<ControlWithJoystick>().enabled;
                position = transform.position;
            }
            Debug.Log("collision detected");
        }
        return false;
    }
}
