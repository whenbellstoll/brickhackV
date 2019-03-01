using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {


	// Use this for initialization
	void Start ()
    {

	}

    // Update is called once per frame
    void Update()
    {
        //Input.GetKeyDown("joystick 1 button 4") ||
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.Rotate(new Vector3(0, 0, 90));

        }
    }
}
