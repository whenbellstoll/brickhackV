using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainInBoxes : MonoBehaviour {

    Rect box = new Rect(0f, 10.9f, 16.8f, 23.6f);

	// Use this for initialization
	void Start () {
        UpdateBox();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateBox();

        if (transform.position.x > box.x + box.width)
        {
            transform.position = new Vector3(box.x + box.width - 0.1f, transform.position.y);
        }
        if (transform.position.x < box.x)
        {
            transform.position = new Vector3(box.x + 0.1f, transform.position.y);
        }
        if (transform.position.y < box.y - box.height)
        {
            transform.position = new Vector3(transform.position.x, box.y - box.height + 0.1f);
        }
        if (transform.position.y > box.y)
        {
            transform.position = new Vector3(transform.position.x, box.y - 0.1f);
        }
    }


    private void UpdateBox()
    {
        if (transform.position.x < 0)
        {
            box.x = -18.6f;
        }
        else
        {
            box.x = 1.4f;
        }
    }
}
