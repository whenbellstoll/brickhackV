using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftCursorBounds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if( transform.position.x > -1.3f )
        {
            transform.position = new Vector3(-1.4f, transform.position.y);
        }
        if (transform.position.x < -18.6f)
        {
            transform.position = new Vector3(-18.5f, transform.position.y);
        }
        if (transform.position.y < -12.7f)
        {
            transform.position = new Vector3(transform.position.x, -12.6f);
        }
        if (transform.position.y > 10.9f)
        {
            transform.position = new Vector3(transform.position.x, 10.8f);
        }

    }
}
