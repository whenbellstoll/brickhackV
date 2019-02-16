using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public bool inPlacementMode;
    public float speed = 0.1f;

    Vector3 position;

	// Use this for initialization
	void Start () {
        inPlacementMode = false;
        position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.P))
        {
            inPlacementMode = !inPlacementMode;
        }

        if (inPlacementMode)
        {
            MovePlatform();
        }

        transform.position = position;
	}

    void MovePlatform()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.W) || y > 0)
        {
            position.y += speed;
        }

        if (Input.GetKey(KeyCode.A) || x < 0)
        {
            position.x -= speed;
        }

        if (Input.GetKey(KeyCode.S) || y < 0)
        {
            position.y -= speed;
        }

        if (Input.GetKey(KeyCode.D) || x > 0)
        {
            position.x += speed;
        }
    }
}
