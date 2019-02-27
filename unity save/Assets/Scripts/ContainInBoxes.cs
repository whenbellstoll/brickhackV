using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainInBoxes : MonoBehaviour {

    float xPos;
    bool inLeft;

	// Use this for initialization
	void Start () {
        xPos = transform.position.x;
        if(xPos < 0)
        {
            inLeft = true;
        }
        else
        {
            inLeft = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        xPos = transform.position.x;
        if (xPos < 0)
        {
            inLeft = true;
        }
        else
        {
            inLeft = false;
        }
        if (inLeft)
        {
            if (transform.position.x > -1.4f)
            {
                transform.position = new Vector3(-1.5f, transform.position.y);
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
        else
        {
            if (transform.position.x > 18.8f)
            {
                transform.position = new Vector3(18.7f, transform.position.y);
            }
            if (transform.position.x < 1.4f)
            {
                transform.position = new Vector3(1.5f, transform.position.y);
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
}
